using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    [Header("Settings: ")]

    [Tooltip("Remove layers from this mask to prevent them from interacting as ground to the player.\n(anything you dont want the player to be able to jump on)")]
    public LayerMask ground_layers;
    [Tooltip("This point shoots a ray horizontally (to the left) by x_size, if it hits the player is grounded")]
    public Transform groundedRayRight;
    public Transform groundedRayLeft;

    [Header("Air Control")]

    [Tooltip("toggle the players ability to move in the air")]
    public bool air_control = false;
    [Tooltip("air control must be active.\n .9 = 90% control.\nSet to 1 to prevent any air control reduction.\nPositive values will work, but will make movement faster when in the air.")]
    public float reduce_air_control = .25f;
    [Tooltip("Toggles drag effects when in air. Applies to vertical too, can cause weird effect.")]
    public bool air_drag = false;

    [Header("Jump Stuff")]

    [Tooltip("How many jumps should the player have?")]
    public int jumps = 1;
    int jumpsLeft;
    float movementmod;
    [Tooltip("Adjust the force applied when the player jumps")]
    public float jumpForce = 500;
    [Tooltip("Adjust the force applied on jumps after the first one")]
    public float doubleJumpForce = 10;
    [Tooltip("Dampen Jumps? Turns on the gravity multiplier for jumps")]
    public bool jumpDampening = true;
    public bool fallDampening = true;
    [Tooltip("Multiplies players gravity when falling froma jump")]
    [Range(1f, 10f)]
    public float jumpGrav = 3;
    public float baseGravScale = 2;
    bool jumpCheat;//jumpCheat is false once the player is no longer grounded, its not a measure of if the player is moving up etc
    bool jumping;
    //variables to add
    //public bool wallResetJump = false;

    [Header("Movement")]

    [Tooltip("Adjust the players speed")]
    public float speed = 15;
    [Tooltip("The players max force in the x direction, if speed is greater than this, speed is bypassed")]
    public float max_movement_xforce = 15;
    [Tooltip("Adjust the players mass")]
    public float mass = 1;
    public float angularDrag = .05f;
    public float linearDrag = 0f;
    [Tooltip("Adjust movement dampening")]
    [Range(0f,10f)]
    public float dampenerStrength = 50;
    public bool counterSlide = false;
    [Tooltip("the lenience for how fast the player needs to be moving to counter the slide, a smaller number means the slide will be cancelled fuller")]
    [Range(0f,1f)]
    public float counterSlideLenience = .25f;
    Vector2 facingDirection = Vector2.right;

    [Header("Other")]
    public Animator m_animator;
    public bool lock_rotation = true;
    [Header("Viewing Only")]
    public bool grounded = false;
    private Rigidbody2D rb;
    bool facingRight = true;

    private void Awake()
    {
        DestroyIfDuplicate();
    }
    void DestroyIfDuplicate()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }
    }

    void Start()
    {
        m_animator = this.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;
        rb.angularDrag = angularDrag;
        rb.drag = linearDrag;
        rb.freezeRotation = lock_rotation;
    }

    void Update(){
        if (grounded && math.abs(rb.velocity.x) >  0){
            m_animator.SetBool("running", true);
        }
        else{
            m_animator.SetBool("running", false);
        }
        m_animator.SetBool("jumping", jumping);
        
        if (Input.GetKeyDown(KeyCode.Space)){
            TryJump();
        }
        //Debug.Log(rb.velocity.y);
        if (Input.GetKeyUp(KeyCode.Space) && jumping && jumpDampening) {
            rb.gravityScale = jumpGrav;
        }
    }
    void TryJump()
    {
        if (jumpsLeft > 0)
        {
            jumpCheat = true;
            jumping = true;
            rb.drag = linearDrag;

            if ((doubleJumpForce != jumpForce) && (jumpsLeft < jumps))
            {
                rb.AddForce(new Vector2(0, doubleJumpForce)); // since using addforce and grounded raycast the player can read as grounded after they jump and get the jump reset, prevented with more jump logic
            }
            else
            {
                rb.AddForce(new Vector2(0, jumpForce)); // since using addforce and grounded raycast the player can read as grounded after they jump and get the jump reset, prevented with more jump logic
            }

            jumpsLeft--;
        }
    }

    // FixedUpdate happens once every physics frame
    void FixedUpdate()
    {
        Jump();
        Move();
    }
    void Jump()
    {
        // || Physics2D.Raycast(new Vector2(this.transform.position.x-.5f, this.transform.position.y - .5f), Vector2.down, .1f)
        grounded = Physics2D.Linecast(groundedRayRight.transform.position, groundedRayLeft.transform.position, ground_layers);//.9f is to stop it from hitting the walls a bit
        if (jumpCheat)
        {
            if (!grounded)
            {
                jumpCheat = false;
            }
            grounded = false;
        }
        if (grounded)
        {
            rb.gravityScale = baseGravScale;
            if (jumping)
            {
                jumping = false;
            }
        }

        if (grounded && (jumpsLeft < jumps))
        {
            rb.gravityScale = baseGravScale;
            jumpsLeft = jumps;
        }
    }
    void Move()
    {
        SetMovementMod();
        GetInput();
    }
    void SetMovementMod()
    {
        //Movement Related Conditionals
        if (air_control || grounded) // dont check for movement if the player cannot move
        {
            if (air_control && !grounded)
            {
                movementmod = reduce_air_control;
            }
            else if (air_control) // player is grounded with air control
            {
                movementmod = 1;
            }
        }
    }
    void GetInput()
    {
        //-------All Movement Code Must Go Below--------//
        float x_force = rb.velocity.x * mass;
        MoveLeft(x_force);
        MoveRight(x_force);

        if (counterSlide && grounded)
        {
            if (((x_force > (max_movement_xforce * counterSlideLenience)) && !Input.GetKey(KeyCode.D)) || ((x_force < (-max_movement_xforce * counterSlideLenience)) && !Input.GetKey(KeyCode.A)))
            {
                rb.drag = dampenerStrength;
            }
        }

        if (!jumping && !grounded && fallDampening)
        {
            rb.gravityScale = jumpGrav;
        }
    }
    void MoveLeft(float x_force)
    {
        if (Input.GetKey(KeyCode.A))
        {
            facingDirection = Vector2.left;
            if (facingRight){
                facingRight = false;
                transform.Rotate(0, 180, 0);
            }
            if (x_force > 0 && (grounded || air_drag))
            {
                rb.drag = dampenerStrength;
            }
            else
            {
                rb.drag = linearDrag;
            }
            if (x_force > -max_movement_xforce) //allow the player to move?
            {
                if ((x_force + (-speed * mass * Time.deltaTime)) > -max_movement_xforce)
                {
                    rb.AddForce(new Vector2(-speed, 0));
                }
                else
                {
                    rb.AddForce(new Vector2((-max_movement_xforce - x_force) * movementmod, 0));
                }
            }
        }
    }
    void MoveRight(float x_force)
    {
        if (Input.GetKey(KeyCode.D))
        {
            facingDirection = Vector2.right;
            if (!facingRight){
                facingRight = true;
                transform.Rotate(0, 180, 0);
            }
            
            if (x_force < 0 && (grounded || air_drag))
            {
                rb.drag = dampenerStrength;
            }
            else
            {
                rb.drag = linearDrag;
            }
            if (x_force < max_movement_xforce) //allow the player to move?
            {
                if ((x_force + (speed * mass * Time.deltaTime)) < max_movement_xforce)
                {
                    rb.AddForce(new Vector2(speed, 0));
                }
                else
                {
                    rb.AddForce(new Vector2((max_movement_xforce - x_force) * movementmod, 0));
                }
            }
        }
    }

    public bool IsGrounded()
    {
        return grounded;
    }

    public Vector2 GetFacingDirection()
    {
        return facingDirection;
    }
    public void setJumps(int j){
        jumps = j;
        jumpsLeft = j;
    }
}

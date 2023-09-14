using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings: ")]
    [Tooltip("Remove layers from this mask to prevent them from interacting as ground to the player.\n(anything you dont want the player to be able to jump on)")]
    public LayerMask ground_layers;
    [Tooltip("This point shoots a ray horizontally (to the left) by x_size, if it hits the player is grounded")]
    public Transform grounded_ray;
    [Tooltip("toggle the players ability to move in the air")]
    public bool air_control = false;
    [Tooltip("air control must be active.\n .9 = 90% control.\nSet to 1 to prevent any air control reduction.\nPositive values will work, but will make movement faster when in the air.")]
    public float reduce_air_control = .25f;
    [Tooltip("Toggles drag effects when in air. Applies to vertical too, can cause weird effect.")]
    public bool air_drag = false;
    float movementmod;
    [Tooltip("Adjust the players speed")]
    public float speed = 15;
    [Tooltip("Adjust the players mass")]
    public float mass = 1;
    [Tooltip("This is used to determine how far to shoot the ground raycast, may have more uses in the future.")]
    public float x_size = 1;
    public float angularDrag = .05f;
    public float linearDrag = 0f;
    public float max_movement_xforce = 15;
    [Tooltip("Adjust movement dampening")]
    [Range(0f,100f)]
    public float dampenerStrength = 50;
    [Tooltip("Adjust the force applied when the player jumps")]
    public float jumpForce = 500;
    [Tooltip("Adjust the force applied on the second jump")]
    public float doubleJumpForce = 10;
    public bool lock_rotation = true;
    [Header("Viewing Only")]
    [Tooltip("here for debugging, changing it here will likely be overwritten in script")]
    public bool doubleJump = false;
    bool second_jump_used = false;
    public bool grounded = false;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;
        rb.angularDrag = angularDrag;
        rb.drag = linearDrag;
        rb.freezeRotation = lock_rotation;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Space)){
            tryJump();
        }
    }
    // FixedUpdate happens once every physics frame
    void FixedUpdate()
    {
        // || Physics2D.Raycast(new Vector2(this.transform.position.x-.5f, this.transform.position.y - .5f), Vector2.down, .1f)
        grounded = Physics2D.Raycast(grounded_ray.transform.position, new Vector2(-90, 0), x_size, ground_layers);
        float x_force = rb.velocity.x * mass;


        //Movement Related Conditionals
        if (!air_control && !grounded){//dont check for movement if the player cannot move
            return;//skipping code below, its just more elegant than nesting it
        }
        if (air_control && !grounded){
            float movementmod = reduce_air_control;
        }
        else if (air_control){//player is grounded with air control
            movementmod = 1;
        }

        //-------All Movement Code Must Go Below--------//
        if (Input.GetKey(KeyCode.D)){
            if (x_force < 0 && (grounded || air_drag)){
                rb.drag *= dampenerStrength;
            }else{
                rb.drag = linearDrag;
            }
            if (x_force < max_movement_xforce){
                if (speed < (max_movement_xforce - x_force)){
                    rb.AddForce(new Vector2(speed, 0));
                }else{
                    rb.AddForce(new Vector2((max_movement_xforce - rb.totalForce.x) * movementmod, 0));
                }
            }
        }
        if (Input.GetKey(KeyCode.A)){
            if (x_force > 0 && (grounded || air_drag)){
                rb.drag *= dampenerStrength;
            }else{
                rb.drag = linearDrag;
            }
            if (x_force > -max_movement_xforce){
                if (-speed < (-max_movement_xforce - x_force)){ //x force can be positive since the player can press left while going right
                    rb.AddForce(new Vector2(-speed, 0));
                }else{
                    rb.AddForce(new Vector2(-(max_movement_xforce - rb.totalForce.x) * movementmod, 0));
                }
            }
        }

    }
    void tryJump(){
        if (grounded){
            rb.drag = linearDrag;
            rb.AddForce(new Vector2(0, jumpForce));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

public class OnPlayerCollision : MonoBehaviour
{
    //public Transform HitBox_Front;
    public float pushForce = 0f; //the pushback force of the other object object
    private CircleCollider2D HitBoxFront;
    //reference to the position of the player front hitbox


    // Start is called before the first frame update
    void Start()
    {
        HitBoxFront = GetComponent<CircleCollider2D>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("We hit something");
        Rigidbody2D OtherRigidBody = collision.collider.GetComponent<Rigidbody2D>();
        if (OtherRigidBody != null ) 
            {
            Vector2 PushDirection = (collision.contacts[0].point - (Vector2)transform.position).normalized;
            OtherRigidBody.AddForce(PushDirection);
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

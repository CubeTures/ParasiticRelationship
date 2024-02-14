using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TheCollider : MonoBehaviour
{
    public BoxCollider2D entry;
    public SpriteRenderer renda;
    bool thru;
    bool collided = false;
    GameObject player;
    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.CompareTag("Player")){
            collided = true;
            player = col.gameObject;
        }
    }
    void OnCollisionExit2D(Collision2D col){
        if (col.gameObject.CompareTag("Player") && collided){
            entry.isTrigger = false;
            renda.enabled = true;
            renda.gameObject.SetActive(true);
        }
    }
}

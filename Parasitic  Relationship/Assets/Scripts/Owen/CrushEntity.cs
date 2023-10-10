using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushEntity : MonoBehaviour
{
    [SerializeField] string[] crushableEntityTags;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(string tag in crushableEntityTags)
        {
            if(collision.gameObject.CompareTag(tag))
            {
                Destroy(collision.gameObject);
                break;
            }
        }
    }
}

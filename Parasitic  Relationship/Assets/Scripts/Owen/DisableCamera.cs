using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCamera : MonoBehaviour
{
    void Start()
    {
        Camera thisCamera = GetComponent<Camera>();
        if(thisCamera != Camera.main)
        {
            Destroy(gameObject);
        }
    }
}

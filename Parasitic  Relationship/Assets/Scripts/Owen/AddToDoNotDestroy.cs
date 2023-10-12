using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToDoNotDestroy : MonoBehaviour
{
    private void Awake()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
}

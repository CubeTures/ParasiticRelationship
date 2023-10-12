using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectItem : MonoBehaviour
{
    [SerializeField] UnityEvent onCollect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCollect.Invoke();
        Destroy(gameObject);
    }
}

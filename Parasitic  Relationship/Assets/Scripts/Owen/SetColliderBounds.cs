using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColliderBounds : MonoBehaviour
{
    [SerializeField] PolygonCollider2D bounds;
    [SerializeField] Vector2[] points;

    public void SetBounds()
    {
        bounds.points = points;
    }
}

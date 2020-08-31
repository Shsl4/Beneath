using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class CameraConfinerShape : MonoBehaviour
{
    [HideInInspector]
    public PolygonCollider2D Collider2D;

    private void Awake()
    {
        Collider2D = GetComponent<PolygonCollider2D>();
    }
}
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class CameraConfinerShape : MonoBehaviour
{
    [HideInInspector]
    public PolygonCollider2D polygonCollider;

    private void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
    }
}
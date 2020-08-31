using Cinemachine;
using UnityEngine;

public class ConfinerShapeFinder : CinemachineConfiner
{

    protected override void Awake()
    {
        Refresh();
        base.Awake();
    }

    public void Refresh()
    {
        if (FindObjectOfType<CameraConfinerShape>())
        {
            m_BoundingShape2D = FindObjectOfType<CameraConfinerShape>().Collider2D;
            InvalidatePathCache();
        }
    }
        
}
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class BattleArea : MonoBehaviour
{

    private EdgeCollider2D _collider;
    private RectTransform _rect;
    private float _resizeTime = 5.0f;
    private (Vector2 min, Vector2 max) _targetSize = (new Vector2(0, 0), new Vector2(0, 0));
    private float TimeMultiplier => Time.deltaTime / (2 * _resizeTime);
    private bool ShouldResizeXMin => Math.Abs(_rect.anchorMin.x - _targetSize.min.x) <= 0.001f;
    private bool ShouldResizeYMin => Math.Abs(_rect.anchorMin.y - _targetSize.min.y) <= 0.001f;
    private bool ShouldResizeXMax => Math.Abs(_rect.anchorMax.x - _targetSize.max.x) <= 0.001f;
    private bool ShouldResizeYMax => Math.Abs(_rect.anchorMax.y - _targetSize.max.y) <= 0.001f;
    
    private void Awake()
    {
        
        _collider = GetComponent<EdgeCollider2D>();
        _rect = GetComponent<RectTransform>();
        _targetSize.max = _rect.anchorMax;
        _targetSize.min = _rect.anchorMin;
        
    }

    void Start()
    {
        _collider.points = new Vector2[5];
    }

    private void UpdateSize()
    {
        
        if (ShouldResizeXMin)
        {
            int xMinDirection = _targetSize.min.x > _rect.rect.min.x ? 1 : -1;
            _rect.anchorMin += new Vector2(xMinDirection * TimeMultiplier, 0);
            if (!ShouldResizeXMin) { _rect.anchorMin = new Vector2(_targetSize.min.x, _rect.anchorMin.y); }
            ResizeCollider();
        }
        
        if (ShouldResizeXMax)
        {
            int xMaxDirection = _targetSize.max.x > _rect.rect.max.x ? 1 : -1;
            _rect.anchorMax += new Vector2(xMaxDirection * TimeMultiplier, 0);
            if (!ShouldResizeXMax) { _rect.anchorMax = new Vector2(_targetSize.max.x, _rect.anchorMax.y); }
            ResizeCollider();
        }  
        
        if (ShouldResizeYMin)
        {
            int yMinDirection = _targetSize.min.y > _rect.rect.min.y ? 1 : -1;
            _rect.anchorMin += new Vector2(0, yMinDirection * TimeMultiplier);
            if (!ShouldResizeYMin) { _rect.anchorMin = new Vector2(_rect.anchorMin.x, _targetSize.min.y); }
            ResizeCollider();
        }
        
        if (ShouldResizeYMax)
        {
            int yMaxDirection = _targetSize.max.y > _rect.rect.max.y ? 1 : -1;
            _rect.anchorMax += new Vector2(0, yMaxDirection * TimeMultiplier);
            if (!ShouldResizeYMax) { _rect.anchorMax = new Vector2(_rect.anchorMax.x, _targetSize.max.y); }
            ResizeCollider();
        }
        
    }

    void Update()
    {
        UpdateSize();
    }

    private void ResizeCollider()
    {
        var points = _collider.points;
        var rect = _rect.rect;

        points[0] = rect.min;
        points[1] = new Vector2(rect.max.x, rect.min.y);
        points[2] = rect.max;
        points[3] = new Vector2(rect.min.x, rect.max.y);
        points[4] = rect.min;
        _collider.SetPoints(new List<Vector2>(points));
    }
    public void ResizeArea(Vector2 min, Vector2 max)
    {

        if (Mathf.Abs(min.x) > 1.0f || Mathf.Abs(max.x) > 1.0f || Mathf.Abs(min.y) > 1.0f ||
            Mathf.Abs(max.y) > 1.0f) return;

        if (min.x > max.x || min.y > max.y) return;
        
        _targetSize = (min, max);

    }
    
}

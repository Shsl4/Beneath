﻿using UnityEngine;
using UnityEngine.UI;

namespace UI.General
{
    public class BeneathOutline : Outline
    {

        public override void ModifyMesh(VertexHelper vh)
        {
            
            Rect rect = GetComponent<RectTransform>().rect;
            float width = rect.width;
            float height = rect.height;
            Vector2 dist;

            if (width > height)
            {
                dist = new Vector2(height / 20.0f,height / 20.0f);
            }
            else
            {
                dist = new Vector2(width / 20.0f,width / 20.0f);
            }

            effectDistance = dist;
            base.ModifyMesh(vh);
        }
        
    }
}
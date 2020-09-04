using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.General
{
    public class BeneathVerticalLayout : LayoutGroup
    {

        [Range(0, 1.0f)] public float spacing;
        public int offsetAmount;

        private void Process()
        {
            
            offsetAmount = offsetAmount % 2 == 0 ? offsetAmount : offsetAmount + 1;
            
            int childCount = transform.childCount + offsetAmount;
            float lSpace = spacing / 3.15f / childCount;
            float size = (1.0f - lSpace) / childCount;
            float nextMax = (1.0f - lSpace);

            for (int i = 0; i < offsetAmount / 2; i++)
            {
                nextMax -= size;
            }

            for (int i = 0; i < childCount; i++)
            {

                if (offsetAmount > 0 && i > childCount - offsetAmount - 1) { break;}
                
                var child = transform.GetChild(i);
                var childRect = child.GetComponent<RectTransform>();
                
                float anchorMax = (nextMax - lSpace);
                float anchorMin = nextMax - size + (2 * lSpace);
                nextMax -= size;

                childRect.anchorMin = new Vector2(0.0f, anchorMin);
                childRect.anchorMax = new Vector2(1.0f, anchorMax);
                childRect.offsetMax = Vector2.zero;
                childRect.offsetMin = Vector2.zero;
                
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            Process();
        }

        public override void SetLayoutHorizontal()
        {
            Process();
        }

        public override void SetLayoutVertical()
        {
            Process();
        }
        
        
    }

}
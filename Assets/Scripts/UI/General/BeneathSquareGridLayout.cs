using UnityEngine;
using UnityEngine.UI;
using Constraint = UnityEngine.UI.GridLayoutGroup.Constraint;
using Axis = UnityEngine.UI.GridLayoutGroup.Axis;
using Corner = UnityEngine.UI.GridLayoutGroup.Corner;

namespace UI.General
{
    
    public class BeneathSquareGridLayout : LayoutGroup
    {
        
        [SerializeField] protected Corner mStartCorner = Corner.UpperLeft;
        
        public Corner StartCorner { get { return mStartCorner; } set { SetProperty(ref mStartCorner, value); } }

        [SerializeField] protected Axis mStartAxis = Axis.Horizontal;
        
        public Axis StartAxis { get { return mStartAxis; } set { SetProperty(ref mStartAxis, value); } }

        [SerializeField] protected Vector2 mCellSize = new Vector2(100, 100);

        public float CellLength
        {
            get => CellLength; set => SetProperty(ref mCellSize, DetermineCellSize(new Vector2(value, value)));
        }
        
        private Vector2 CellSize { get { return DetermineCellSize(mCellSize); } set { SetProperty(ref mCellSize, DetermineCellSize(value)); } }

        [SerializeField] protected Vector2 mSpacing = Vector2.zero;
        
        public Vector2 Spacing { get { return DetermineSpacing(mSpacing); } set { SetProperty(ref mSpacing, DetermineSpacing(value)); } }

        [SerializeField] protected Constraint mConstraintProp = Constraint.Flexible;
        
        public Constraint ConstraintProp { get { return mConstraintProp; } set { SetProperty(ref mConstraintProp, value); } }

        [SerializeField] protected int mConstraintCount = 2;
        
        public float sizeRatio = 1.0f;
        
        public int ConstraintCount { get { return mConstraintCount; } set { SetProperty(ref mConstraintCount, Mathf.Max(1, value)); } }

        protected BeneathSquareGridLayout()
        {}

        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            ConstraintCount = ConstraintCount;
        }

        #endif
        
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            int minColumns = 0;
            int preferredColumns = 0;
            if (mConstraintProp == Constraint.FixedColumnCount)
            {
                minColumns = preferredColumns = mConstraintCount;
            }
            else if (mConstraintProp == Constraint.FixedRowCount)
            {
                minColumns = preferredColumns = Mathf.CeilToInt(rectChildren.Count / (float)mConstraintCount - 0.001f);
            }
            else
            {
                minColumns = 1;
                preferredColumns = Mathf.CeilToInt(Mathf.Sqrt(rectChildren.Count));
            }

            SetLayoutInputForAxis(
                padding.horizontal + (CellSize.x + Spacing.x) * minColumns - Spacing.x,
                padding.horizontal + (CellSize.x + Spacing.x) * preferredColumns - Spacing.x,
                -1, 0);
        }
        
        public override void CalculateLayoutInputVertical()
        {
            int minRows = 0;
            if (mConstraintProp == Constraint.FixedColumnCount)
            {
                minRows = Mathf.CeilToInt(rectChildren.Count / (float)mConstraintCount - 0.001f);
            }
            else if (mConstraintProp == Constraint.FixedRowCount)
            {
                minRows = mConstraintCount;
            }
            else
            {
                float width = rectTransform.rect.width;
                int cellCountX = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + Spacing.x + 0.001f) / (CellSize.x + Spacing.x)));
                minRows = Mathf.CeilToInt(rectChildren.Count / (float)cellCountX);
            }

            float minSpace = padding.vertical + (CellSize.y + Spacing.y) * minRows - Spacing.y;
            SetLayoutInputForAxis(minSpace, minSpace, -1, 1);
        }

        public override void SetLayoutHorizontal()
        {
            SetCellsAlongAxis(0);
        }
        
        public override void SetLayoutVertical()
        {
            SetCellsAlongAxis(1);
        }

        private void SetCellsAlongAxis(int axis)
        {
            if (axis == 0)
            {
                for (int i = 0; i < rectChildren.Count; i++)
                {
                    RectTransform rect = rectChildren[i];

                    m_Tracker.Add(this, rect,
                        DrivenTransformProperties.Anchors |
                        DrivenTransformProperties.AnchoredPosition |
                        DrivenTransformProperties.SizeDelta);

                    rect.anchorMin = Vector2.up;
                    rect.anchorMax = Vector2.up;
                    rect.sizeDelta = CellSize;
                }
                return;
            }

            float width = rectTransform.rect.size.x;
            float height = rectTransform.rect.size.y;

            int cellCountX = 1;
            int cellCountY = 1;
            if (mConstraintProp == Constraint.FixedColumnCount)
            {
                cellCountX = mConstraintCount;

                if (rectChildren.Count > cellCountX)
                    cellCountY = rectChildren.Count / cellCountX + (rectChildren.Count % cellCountX > 0 ? 1 : 0);
            }
            else if (mConstraintProp == Constraint.FixedRowCount)
            {
                cellCountY = mConstraintCount;

                if (rectChildren.Count > cellCountY)
                    cellCountX = rectChildren.Count / cellCountY + (rectChildren.Count % cellCountY > 0 ? 1 : 0);
            }
            else
            {
                if (CellSize.x + Spacing.x <= 0)
                    cellCountX = int.MaxValue;
                else
                    cellCountX = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + Spacing.x + 0.001f) / (CellSize.x + Spacing.x)));

                if (CellSize.y + Spacing.y <= 0)
                    cellCountY = int.MaxValue;
                else
                    cellCountY = Mathf.Max(1, Mathf.FloorToInt((height - padding.vertical + Spacing.y + 0.001f) / (CellSize.y + Spacing.y)));
            }

            int cornerX = (int)StartCorner % 2;
            int cornerY = (int)StartCorner / 2;

            int cellsPerMainAxis, actualCellCountX, actualCellCountY;
            if (StartAxis == Axis.Horizontal)
            {
                cellsPerMainAxis = cellCountX;
                actualCellCountX = Mathf.Clamp(cellCountX, 1, rectChildren.Count);
                actualCellCountY = Mathf.Clamp(cellCountY, 1, Mathf.CeilToInt(rectChildren.Count / (float)cellsPerMainAxis));
            }
            else
            {
                cellsPerMainAxis = cellCountY;
                actualCellCountY = Mathf.Clamp(cellCountY, 1, rectChildren.Count);
                actualCellCountX = Mathf.Clamp(cellCountX, 1, Mathf.CeilToInt(rectChildren.Count / (float)cellsPerMainAxis));
            }

            Vector2 requiredSpace = new Vector2(
                actualCellCountX * CellSize.x + (actualCellCountX - 1) * Spacing.x,
                actualCellCountY * CellSize.y + (actualCellCountY - 1) * Spacing.y
            );
            Vector2 startOffset = new Vector2(
                GetStartOffset(0, requiredSpace.x),
                GetStartOffset(1, requiredSpace.y)
            );

            for (int i = 0; i < rectChildren.Count; i++)
            {
                int positionX;
                int positionY;
                if (StartAxis == Axis.Horizontal)
                {
                    positionX = i % cellsPerMainAxis;
                    positionY = i / cellsPerMainAxis;
                }
                else
                {
                    positionX = i / cellsPerMainAxis;
                    positionY = i % cellsPerMainAxis;
                }

                if (cornerX == 1)
                    positionX = actualCellCountX - 1 - positionX;
                if (cornerY == 1)
                    positionY = actualCellCountY - 1 - positionY;

                SetChildAlongAxis(rectChildren[i], 0, startOffset.x + (CellSize[0] + Spacing[0]) * positionX, CellSize[0]);
                SetChildAlongAxis(rectChildren[i], 1, startOffset.y + (CellSize[1] + Spacing[1]) * positionY, CellSize[1]);
            }
        }
        Vector2 DetermineCellSize(Vector2 defaultValue)
        {
            
            if (ConstraintProp == Constraint.Flexible) { return defaultValue; }
            return CalculateSize() - DetermineSpacing(defaultValue);

        }
        
        Vector2 DetermineSpacing(Vector2 defaultValue)
        {
            if (ConstraintProp == Constraint.Flexible) { return defaultValue; }

            return CalculateSize() / 4.0f;
        }

        private Vector2 CalculateSize()
        {
            Rect rect = GetComponent<RectTransform>().rect;
            float width = rect.width;
            float height = rect.height;
            int hChildCount;
            int vChildCount;
            float size = 0.0f;

            switch (ConstraintProp)
            {
                case Constraint.FixedColumnCount:

                    if (width > height)
                    {
                        hChildCount = ConstraintCount;
                        vChildCount = Mathf.CeilToInt((float)transform.childCount / ConstraintCount);
                        size = height / vChildCount;

                        if (size * hChildCount > width)
                        {
                            size = width / hChildCount;
                        }
                    }
                    else
                    {
                        hChildCount = Mathf.CeilToInt((float)transform.childCount / ConstraintCount);
                        vChildCount = ConstraintCount;
                        size = width / vChildCount;

                        if (size * hChildCount > height)
                        {
                            size = height / hChildCount;
                        }
                    }
                    break;
                
                case Constraint.FixedRowCount:
                    
                    if (width > height)
                    {

                        hChildCount = Mathf.CeilToInt((float)transform.childCount / ConstraintCount);
                        vChildCount = ConstraintCount;
                        size = height / vChildCount;

                        if (size * hChildCount > width)
                        {
                            size = width / hChildCount;
                        }
                        
                    }
                    else
                    {
                        hChildCount = ConstraintCount;
                        vChildCount = Mathf.CeilToInt((float)transform.childCount / ConstraintCount);
                        size = width / vChildCount;

                        if (size * hChildCount > height)
                        {
                            size = height / hChildCount;
                        }

                    }
                    
                    break;
            }
            return new Vector2(size, size) * sizeRatio;
        }



    }
}
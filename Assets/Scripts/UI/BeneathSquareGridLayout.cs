using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BeneathSquareGridLayout : LayoutGroup
    {
        
        public enum Corner
        {
            UpperLeft = 0,
            UpperRight = 1,
            LowerLeft = 2,
            LowerRight = 3
        }
        
        public enum Axis
        {
            Horizontal = 0,
            Vertical = 1
        }
        
        public enum Constraint
        {
            Flexible = 0,
            FixedColumnCount = 1,
            FixedRowCount = 2
        }

        [SerializeField] protected Corner m_StartCorner = Corner.UpperLeft;
        
        public Corner startCorner { get { return m_StartCorner; } set { SetProperty(ref m_StartCorner, value); } }

        [SerializeField] protected Axis m_StartAxis = Axis.Horizontal;
        
        public Axis startAxis { get { return m_StartAxis; } set { SetProperty(ref m_StartAxis, value); } }

        [SerializeField] protected Vector2 m_CellSize = new Vector2(100, 100);

        public float cellLength
        {
            get => cellLength; set => SetProperty(ref m_CellSize, DetermineCellSize(new Vector2(value, value)));
        }
        
        private Vector2 cellSize { get { return DetermineCellSize(m_CellSize); } set { SetProperty(ref m_CellSize, DetermineCellSize(value)); } }

        [SerializeField] protected Vector2 m_Spacing = Vector2.zero;
        
        public Vector2 spacing { get { return DetermineSpacing(m_Spacing); } set { SetProperty(ref m_Spacing, DetermineSpacing(value)); } }

        [SerializeField] protected Constraint m_Constraint = Constraint.Flexible;
        
        public Constraint constraint { get { return m_Constraint; } set { SetProperty(ref m_Constraint, value); } }

        [SerializeField] protected int m_ConstraintCount = 2;
        
        public float sizeRatio = 1.0f;
        
        public int constraintCount { get { return m_ConstraintCount; } set { SetProperty(ref m_ConstraintCount, Mathf.Max(1, value)); } }

        protected BeneathSquareGridLayout()
        {}

        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            constraintCount = constraintCount;
        }

        #endif
        
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            int minColumns = 0;
            int preferredColumns = 0;
            if (m_Constraint == Constraint.FixedColumnCount)
            {
                minColumns = preferredColumns = m_ConstraintCount;
            }
            else if (m_Constraint == Constraint.FixedRowCount)
            {
                minColumns = preferredColumns = Mathf.CeilToInt(rectChildren.Count / (float)m_ConstraintCount - 0.001f);
            }
            else
            {
                minColumns = 1;
                preferredColumns = Mathf.CeilToInt(Mathf.Sqrt(rectChildren.Count));
            }

            SetLayoutInputForAxis(
                padding.horizontal + (cellSize.x + spacing.x) * minColumns - spacing.x,
                padding.horizontal + (cellSize.x + spacing.x) * preferredColumns - spacing.x,
                -1, 0);
        }
        
        public override void CalculateLayoutInputVertical()
        {
            int minRows = 0;
            if (m_Constraint == Constraint.FixedColumnCount)
            {
                minRows = Mathf.CeilToInt(rectChildren.Count / (float)m_ConstraintCount - 0.001f);
            }
            else if (m_Constraint == Constraint.FixedRowCount)
            {
                minRows = m_ConstraintCount;
            }
            else
            {
                float width = rectTransform.rect.width;
                int cellCountX = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));
                minRows = Mathf.CeilToInt(rectChildren.Count / (float)cellCountX);
            }

            float minSpace = padding.vertical + (cellSize.y + spacing.y) * minRows - spacing.y;
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
                    rect.sizeDelta = cellSize;
                }
                return;
            }

            float width = rectTransform.rect.size.x;
            float height = rectTransform.rect.size.y;

            int cellCountX = 1;
            int cellCountY = 1;
            if (m_Constraint == Constraint.FixedColumnCount)
            {
                cellCountX = m_ConstraintCount;

                if (rectChildren.Count > cellCountX)
                    cellCountY = rectChildren.Count / cellCountX + (rectChildren.Count % cellCountX > 0 ? 1 : 0);
            }
            else if (m_Constraint == Constraint.FixedRowCount)
            {
                cellCountY = m_ConstraintCount;

                if (rectChildren.Count > cellCountY)
                    cellCountX = rectChildren.Count / cellCountY + (rectChildren.Count % cellCountY > 0 ? 1 : 0);
            }
            else
            {
                if (cellSize.x + spacing.x <= 0)
                    cellCountX = int.MaxValue;
                else
                    cellCountX = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));

                if (cellSize.y + spacing.y <= 0)
                    cellCountY = int.MaxValue;
                else
                    cellCountY = Mathf.Max(1, Mathf.FloorToInt((height - padding.vertical + spacing.y + 0.001f) / (cellSize.y + spacing.y)));
            }

            int cornerX = (int)startCorner % 2;
            int cornerY = (int)startCorner / 2;

            int cellsPerMainAxis, actualCellCountX, actualCellCountY;
            if (startAxis == Axis.Horizontal)
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
                actualCellCountX * cellSize.x + (actualCellCountX - 1) * spacing.x,
                actualCellCountY * cellSize.y + (actualCellCountY - 1) * spacing.y
            );
            Vector2 startOffset = new Vector2(
                GetStartOffset(0, requiredSpace.x),
                GetStartOffset(1, requiredSpace.y)
            );

            for (int i = 0; i < rectChildren.Count; i++)
            {
                int positionX;
                int positionY;
                if (startAxis == Axis.Horizontal)
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

                SetChildAlongAxis(rectChildren[i], 0, startOffset.x + (cellSize[0] + spacing[0]) * positionX, cellSize[0]);
                SetChildAlongAxis(rectChildren[i], 1, startOffset.y + (cellSize[1] + spacing[1]) * positionY, cellSize[1]);
            }
        }
        Vector2 DetermineCellSize(Vector2 defaultValue)
        {
            
            if (constraint == Constraint.Flexible) { return defaultValue; }
            return CalculateSize() - DetermineSpacing(defaultValue);

        }
        
        Vector2 DetermineSpacing(Vector2 defaultValue)
        {
            if (constraint == Constraint.Flexible) { return defaultValue; }

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

            switch (constraint)
            {
                case Constraint.FixedColumnCount:

                    if (width > height)
                    {
                        hChildCount = constraintCount;
                        vChildCount = Mathf.CeilToInt((float)transform.childCount / constraintCount);
                        size = height / vChildCount;

                        if (size * hChildCount > width)
                        {
                            size = width / hChildCount;
                        }
                    }
                    else
                    {
                        hChildCount = Mathf.CeilToInt((float)transform.childCount / constraintCount);
                        vChildCount = constraintCount;
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

                        hChildCount = Mathf.CeilToInt((float)transform.childCount / constraintCount);
                        vChildCount = constraintCount;
                        size = height / vChildCount;

                        if (size * hChildCount > width)
                        {
                            size = width / hChildCount;
                        }
                        
                    }
                    else
                    {
                        hChildCount = constraintCount;
                        vChildCount = Mathf.CeilToInt((float)transform.childCount / constraintCount);
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
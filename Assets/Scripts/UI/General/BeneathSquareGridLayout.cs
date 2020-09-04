using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Constraint = UnityEngine.UI.GridLayoutGroup.Constraint;
using Axis = UnityEngine.UI.GridLayoutGroup.Axis;
using Corner = UnityEngine.UI.GridLayoutGroup.Corner;

namespace UI.General
{
    
    public class BeneathSquareGridLayout : LayoutGroup
    {
        
        [SerializeField] protected Corner startCorner = Corner.UpperLeft;
        
        public Corner StartCorner { get { return startCorner; } set { SetProperty(ref startCorner, value); } }

        [SerializeField] protected Axis startAxis = Axis.Horizontal;
        
        public Axis StartAxis { get { return startAxis; } set { SetProperty(ref startAxis, value); } }

        [SerializeField] protected Vector2 cellSize = new Vector2(100, 100);

        public float CellLength
        {
            get => CellLength; set => SetProperty(ref cellSize, DetermineCellSize(new Vector2(value, value)));
        }
        
        private Vector2 CellSize { get { return DetermineCellSize(cellSize); } set { SetProperty(ref cellSize, DetermineCellSize(value)); } }

        [SerializeField] protected Vector2 spacing = Vector2.zero;
        
        public Vector2 Spacing { get { return DetermineSpacing(spacing); } set { SetProperty(ref spacing, DetermineSpacing(value)); } }

        [SerializeField] protected Constraint constraintProp = Constraint.Flexible;
        
        public Constraint ConstraintProp { get { return constraintProp; } set { SetProperty(ref constraintProp, value); } }

        [SerializeField] protected int constraintCount = 2;
        
        public float sizeRatio = 1.0f;
        
        public int ConstraintCount { get { return constraintCount; } set { SetProperty(ref constraintCount, Mathf.Max(1, value)); } }

        protected BeneathSquareGridLayout()
        {}

        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            ConstraintCount = constraintCount;
        }

        #endif
        
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            int minColumns = 0;
            int preferredColumns = 0;
            if (constraintProp == Constraint.FixedColumnCount)
            {
                minColumns = preferredColumns = constraintCount;
            }
            else if (constraintProp == Constraint.FixedRowCount)
            {
                minColumns = preferredColumns = Mathf.CeilToInt(rectChildren.Count / (float)constraintCount - 0.001f);
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
            if (constraintProp == Constraint.FixedColumnCount)
            {
                minRows = Mathf.CeilToInt(rectChildren.Count / (float)constraintCount - 0.001f);
            }
            else if (constraintProp == Constraint.FixedRowCount)
            {
                minRows = constraintCount;
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
            if (constraintProp == Constraint.FixedColumnCount)
            {
                cellCountX = constraintCount;

                if (rectChildren.Count > cellCountX)
                    cellCountY = rectChildren.Count / cellCountX + (rectChildren.Count % cellCountX > 0 ? 1 : 0);
            }
            else if (constraintProp == Constraint.FixedRowCount)
            {
                cellCountY = constraintCount;

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
    
#if UNITY_EDITOR

    [CustomEditor(typeof(BeneathSquareGridLayout), true)]
    [CanEditMultipleObjects]
    public class BeneathSquareGridLayoutEditor : Editor
    {
        
        private SerializedProperty _childAlignment;
        private SerializedProperty _startCorner;
        private SerializedProperty _startAxis;
        private SerializedProperty _cellSize;
        private SerializedProperty _spacing;
        private SerializedProperty _constraintProp;
        private SerializedProperty _constraintCount;
        private SerializedProperty _sizeRatio;
        
        protected void OnEnable()
        {
            _childAlignment = serializedObject.FindProperty("m_ChildAlignment");
            _startCorner = serializedObject.FindProperty("startCorner");
            _startAxis = serializedObject.FindProperty("startAxis");
            _cellSize = serializedObject.FindProperty("cellSize");
            _spacing = serializedObject.FindProperty("spacing");
            _constraintProp = serializedObject.FindProperty("constraintProp");
            _constraintCount = serializedObject.FindProperty("constraintCount");
            _sizeRatio = serializedObject.FindProperty("sizeRatio");
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_childAlignment);
            EditorGUILayout.PropertyField(_startCorner);
            EditorGUILayout.PropertyField(_startAxis);
            
            EditorGUILayout.PropertyField(_constraintProp);

            if (_constraintProp.enumValueIndex == 0)
            {
                EditorGUILayout.PropertyField(_cellSize);
                EditorGUILayout.PropertyField(_spacing);
            }
            else
            {
                EditorGUILayout.PropertyField(_constraintCount);
                EditorGUILayout.PropertyField(_sizeRatio);
            }
            
            serializedObject.ApplyModifiedProperties();
            
        }
    }
    
#endif
    
    
    
}
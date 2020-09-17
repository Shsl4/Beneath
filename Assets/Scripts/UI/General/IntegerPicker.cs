using UnityEditor;
using UnityEngine;

namespace UI.General
{
    public class IntegerPicker : OptionPicker
    {

        public int maxInteger = 3;
        
        private string[] _ints;

        public override string[] GetChoices()
        {
            
            _ints = new string[maxInteger + 1];
            
            for (int i = 0; i < maxInteger + 1; i++)
            {
                _ints[i] = i.ToString();
            }

            return _ints;

        }
    }
     
#if UNITY_EDITOR

    [CustomEditor(typeof(IntegerPicker), true)]
    [CanEditMultipleObjects]
    public class IntegerPickerEditor : OptionPickerEditor
    {
        
        private SerializedProperty _maxInteger;

        private bool _pickerFoldout;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _maxInteger = serializedObject.FindProperty("maxInteger");
        }
        
        protected override void MakeAdditionalGUI()
        {
            _pickerFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_pickerFoldout, "Picker Properties");

            if (_pickerFoldout)
            {
                EditorGUILayout.PropertyField(_maxInteger, new GUIContent("Max Value"));
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        
    }
    
    #endif
    
}
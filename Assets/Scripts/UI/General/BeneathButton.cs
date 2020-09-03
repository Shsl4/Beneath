using Interfaces.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.General
{
    
    public enum NavigationType
    {
        WrittenAction = 0,
        SelectOther = 1,
        CloseAllUI = 2
    }

    public class BeneathButton : BeneathButton<UIManager> {}
    
    public abstract class BeneathButton<T> : BeneathSelectable<T> where T : UIManager
    {
        public TMP_Text TextBox => GetComponent<TMP_Text>();

        public Selectable selectOnBack;
        public Selectable selectOnSubmit;
        public NavigationType backNavigationType;
        public NavigationType submitType;
        public bool allowBackNavigation;
        
        public sealed override void OnCancel(BaseEventData eventData)
        {
            if (!allowBackNavigation) return;
            
            switch (backNavigationType)
            {
                case NavigationType.WrittenAction:
                        
                    CancelAction();
                    break;
                    
                case NavigationType.CloseAllUI:
                        
                    Manager.CloseAll();
                    break;
                    
                case NavigationType.SelectOther:

                    if (selectOnBack)
                    {
                        selectOnBack.Select();
                            
                        if(selectOnBack is IBeneathUIComponent button)
                        {
                            if (button.GetManager() != Manager)
                            {
                                Manager.Close();
                            }
                        }
                    }
                    break;
            }
        }

        public sealed override void OnSubmit(BaseEventData eventData)
        {
            if (submitSound)
            {
                Manager.source.PlayOneShot(submitSound);
            }
            
            switch (submitType)
            {
                case NavigationType.CloseAllUI:
                        
                    Manager.CloseAll();
                    break;
                
                case NavigationType.SelectOther:

                    if (selectOnSubmit)
                    {
                        selectOnSubmit.Select();
                        
                        if(selectOnSubmit is IBeneathUIComponent button)
                        {

                            if (!button.GetManager().IsOpen())
                            {
                                button.GetManager().Open();
                            }
                            
                            if (Manager.HasParent() && button.GetManager() != Manager)
                            {
                                Manager.Close();
                            }
                            
                        }
                        
                  
                        
                    }
                    break;
                
                case NavigationType.WrittenAction:
                    SubmitAction();
                    return;
            }
            
        }
        
        protected virtual void SubmitAction() { Debug.Log("Called base SubmitAction on object " + gameObject.name + ". Please inherit the class to add functionality.");}
        protected virtual void CancelAction() { Debug.Log("Called base CancelAction on object " + gameObject.name + ". Please inherit the class to add functionality.");}
        
    }
    
    #if UNITY_EDITOR
    
    [CustomEditor(typeof(BeneathButton<>), true)]
    [CanEditMultipleObjects]
    public class BeneathButtonEditor : Editor
    {
        private SerializedProperty _selectOnBack;
        private SerializedProperty _selectOnSubmit;
        private SerializedProperty _backNavigationType;
        private SerializedProperty _submitType;
        private SerializedProperty _allowBackNavigation;
        private SerializedProperty _colors;
        private SerializedProperty _navigation;
        private SerializedProperty _selectSound;
        private SerializedProperty _submitSound;

        protected bool ShowDefaultNavigation = true;

        private bool _colorFoldout = true;
        private bool _navFoldout = true;
        private bool _audioFoldout = true;
        
        protected virtual void OnEnable()
        {
            
            _selectOnBack = serializedObject.FindProperty("selectOnBack");
            _selectOnSubmit = serializedObject.FindProperty("selectOnSubmit");
            _backNavigationType = serializedObject.FindProperty("backNavigationType");
            _submitType = serializedObject.FindProperty("submitType");
            _allowBackNavigation = serializedObject.FindProperty("allowBackNavigation");
            _colors = serializedObject.FindProperty("m_Colors");
            _navigation = serializedObject.FindProperty("m_Navigation");
            _selectSound = serializedObject.FindProperty("selectSound");
            _submitSound = serializedObject.FindProperty("submitSound");

        }

        protected virtual void MakeColors()
        {
            
            EditorGUILayout.PropertyField(_colors, true);
            
        }

        protected virtual void MakeNavigation()
        {
            
            if (ShowDefaultNavigation)
            {
                EditorGUILayout.PropertyField(_navigation, true);
                EditorGUILayout.Space();
            }
                
            EditorGUILayout.PropertyField(_allowBackNavigation);

            if (_allowBackNavigation.boolValue)
            {
                    
                EditorGUILayout.PropertyField(_backNavigationType, new GUIContent("On Back"));
                
                if ((NavigationType)_backNavigationType.intValue == NavigationType.SelectOther)
                {
                    EditorGUILayout.PropertyField(_selectOnBack, new GUIContent("Select On Back"));
                }
                
            }
            
            EditorGUILayout.PropertyField(_submitType, new GUIContent("On Submit"));

            if ((NavigationType)_submitType.intValue == NavigationType.SelectOther)
            {
                EditorGUILayout.PropertyField(_selectOnSubmit, new GUIContent("Select On Submit"));
            }

        }

        protected virtual void MakeAudio()
        {
            EditorGUILayout.PropertyField(_submitSound, new GUIContent("Submit SFX"));
            EditorGUILayout.PropertyField(_selectSound, new GUIContent("Select SFX"));
        }

        protected virtual void MakeAdditionalGUI() { }

        public override void OnInspectorGUI()
        {
            
            serializedObject.Update();

            _colorFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_colorFoldout, "Colors");
            
            if (_colorFoldout)
            {
                MakeColors();
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            
            _navFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_navFoldout, "Navigation");
            
            if (_navFoldout)
            {
                MakeNavigation();
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            
            _audioFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_audioFoldout, "Audio");

            if (_audioFoldout)
            {
                MakeAudio();
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            MakeAdditionalGUI();
            
            EditorGUILayout.Space();
            
            serializedObject.ApplyModifiedProperties();
            
        }

    }
#endif
    
}

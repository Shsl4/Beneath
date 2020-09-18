using UI.General;
using UI.ResumeMenu;
using UnityEditor;
using UnityEngine;

namespace UI.MainMenu
{
    public class EnterButton : BeneathButton<MainMenuManager>
    {
        
        public AudioClip menuTheme;

        protected override void SubmitAction()
        {

            if (Beneath.SaveManager.HasProgress())
            {
                ((GameObject)Instantiate(Beneath.AssetReferences.ResumeMenu.Asset)).GetComponent<ResumeMenuManager>().Open();
            }
            else
            {
                Manager.OnEnter();
            }
        }
    }
    
        
#if UNITY_EDITOR

    [CustomEditor(typeof(EnterButton))]
    [CanEditMultipleObjects]
    public class EnterButtonEditor : BeneathButtonEditor
    {
        private SerializedProperty _menuTheme;
        protected override void OnEnable()
        {
            base.OnEnable();
            _menuTheme = serializedObject.FindProperty("menuTheme");
        }
        
        protected override void MakeAudio()
        {
            base.MakeAudio();
            EditorGUILayout.PropertyField(_menuTheme, new GUIContent("Menu Theme"));
        }
    }
    
#endif
    
}
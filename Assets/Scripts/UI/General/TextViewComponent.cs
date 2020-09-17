using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI.General
{
    public class TextViewComponent : BeneathButton<UIManager>
    {

        private new TMP_Text TextBox => GetComponentInChildren<TMP_Text>();

        public AudioClip textSound;
        public float revealSpeed = 1.0f;
        public int lineCount = 4;
        
        private string _textToReveal;
        private bool _isRevealing;
        private bool _instantReveal;
        private int _pageToReveal = 1;
        private IEnumerator _activeRoutine;

        protected override void OnEnable()
        {
            base.OnEnable();
            Beneath.TextHelpers.SetIdealPointSize(TextBox, lineCount);
        }

        protected override void Awake()
        {
            Beneath.TextHelpers.SetIdealPointSize(TextBox, lineCount);
            base.Awake();
        }

        #if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            transition = Transition.None;
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.None;
            navigation = nav;
            Beneath.TextHelpers.SetIdealPointSize(TextBox, lineCount);
        }
        
        #endif
        
        public void SelectAndReveal(String text)
        {
            Manager.EventSys.SetSelectedGameObject(gameObject);
            RevealText(text);
        }
        public void RevealText(String text)
        {
            _textToReveal = "* " + text;
            List<int> indexes = new List<int>();
            
            bool process = true;
            int startIndex = _textToReveal.Length;

            while (process)
            {
                
                var foundIndex = _textToReveal.LastIndexOf("\n", startIndex);

                if (foundIndex != -1)
                {

                    indexes.Add(foundIndex);
                    startIndex = foundIndex - 1;

                }
                else
                {
                    process = false;
                }
            }
            
            for(int i = 0; i < indexes.Count; i++)
            {
                
                if(indexes[i] == 0 || indexes[i] == _textToReveal.Length - 1) { continue; }
                if(i > 0 && indexes[i - 1] - indexes[i] == 1) { continue; }
                _textToReveal = _textToReveal.Insert(indexes[i] + 1, "* ");

            }
            
            if (_activeRoutine != null)
            {
                StopCoroutine(_activeRoutine);
            }
            
            HandleReveal();
            
        }

        private void HandleReveal()
        {
            
            TMP_TextInfo info = TextBox.GetTextInfo(_textToReveal);
            
            _activeRoutine = RevealText(info.pageInfo[_pageToReveal - 1].firstCharacterIndex,
                info.pageInfo[_pageToReveal - 1].lastCharacterIndex + 1);
            
            StartCoroutine(_activeRoutine);

        }
        
        protected override void SubmitAction()
        {
            CancelAction();
        }

        protected override void CancelAction()
        {
            if (HandleNext())
            {
                if (Manager is IDialogBoxResponder responder)
                {
                    responder.DisplayEnded();
                }
                TextBox.SetText("");
                _pageToReveal = 1;
            }
        }

        private bool HandleNext()
        {
            
            if (_isRevealing)
            {
                _instantReveal = true;
                return false;

            }
            
            TMP_TextInfo info = TextBox.GetTextInfo(_textToReveal);
                
            if(info.pageCount > _pageToReveal)
            {

                _pageToReveal++;
                HandleReveal();
                return false;

            }
            
            return true;

        }
        
        private IEnumerator RevealText(int from, int to)
        {

            TextBox.SetText("");
            int revealedChars = 0;

            _isRevealing = true;
            
            while (!_instantReveal && revealedChars < to - from)
            {
                if (textSound)
                {
                    Manager.source.PlayOneShot(textSound);
                }
                ++revealedChars;
                TextBox.SetText(_textToReveal.Substring(from, revealedChars));
                yield return new WaitForSeconds(0.025f / revealSpeed);
            }

            _isRevealing = false;

            if (_instantReveal)
            {
                _instantReveal = false;
                TextBox.SetText(_textToReveal.Substring(from, to - from));
            }

        }
        
    }
    
#if UNITY_EDITOR

    [CustomEditor(typeof(TextViewComponent), true)]
    [CanEditMultipleObjects]
    public class TextViewEditor : BeneathButtonEditor
    {
        
        private SerializedProperty _textSound;
        private SerializedProperty _revealSpeed;
        private SerializedProperty _lineCount;

        private bool _boxFoldout;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _textSound = serializedObject.FindProperty("textSound");
            _revealSpeed = serializedObject.FindProperty("revealSpeed");
            _lineCount = serializedObject.FindProperty("lineCount");
        }

        protected override void MakeAudio()
        {
            base.MakeAudio();
            EditorGUILayout.PropertyField(_textSound, new GUIContent("Text SFX"));
        }

        protected override void MakeAdditionalGUI()
        {
            _boxFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_boxFoldout, "Box Properties");

            if (_boxFoldout)
            {
                EditorGUILayout.PropertyField(_revealSpeed, new GUIContent("Reveal speed"));
                EditorGUILayout.PropertyField(_lineCount, new GUIContent("Line count"));
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
    
#endif

}

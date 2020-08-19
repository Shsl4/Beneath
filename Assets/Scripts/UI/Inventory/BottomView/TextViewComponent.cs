using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory.BottomView
{
    public class TextViewComponent : BeneathButton<MasterInterface>
    {
        
        public new TMP_Text TextBox => GetComponentInChildren<TMP_Text>();

        private string _textToReveal;
        private bool _isRevealing;
        private bool _instantReveal;
        private int _pageToReveal = 1;
        private IEnumerator _activeRoutine;

        public void RevealAndSelect(String text)
        {
            Select();
            RevealText(text);
        }
        public void RevealText(String text)
        {
            _textToReveal = text.ToUpper();

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
        
        protected override void ExecuteAction()
        {
            if (HandleNext())
            {
                Master.EventSys.SetSelectedGameObject(Master.LastSubmit);
                TextBox.SetText("");
                _pageToReveal = 1;
            }
            
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            if (HandleNext())
            {
                Master.EventSys.SetSelectedGameObject(Master.LastSubmit);
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
                ++revealedChars;
                TextBox.SetText(_textToReveal.Substring(from, revealedChars));
                yield return new WaitForSeconds(0.025f);
            }

            _isRevealing = false;

            if (_instantReveal)
            {

                _instantReveal = false;
                TextBox.SetText(_textToReveal.Substring(from, to - from));

            }

        }
        
    }
}

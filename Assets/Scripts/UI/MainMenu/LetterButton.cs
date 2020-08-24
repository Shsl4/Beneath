using TMPro;
using UnityEngine;

namespace UI.MainMenu
{
    public class LetterButton : BeneathButton<MainMenuManager>
    {
        private TMP_Text _text;
        public Type buttonType;
        private Vector2 _startingPos;
        RectTransform _rectTransform;
        private float _rumbleSpeed = 5.0f;
        private float _rumbleIntensity = 15.0f;

        public enum Type
        {
            Letter,
            Backspace,
            Done
        }
        
        void FixedUpdate()
        {
            if (!_rectTransform) { return; }
            var x = _startingPos.x + (Random.Range(-_rumbleIntensity, _rumbleIntensity) * Time.deltaTime * _rumbleSpeed);
            var y = _startingPos.y + (Random.Range(-_rumbleIntensity, _rumbleIntensity) * Time.deltaTime * _rumbleSpeed);
            if (buttonType == Type.Letter)
            {
                _rectTransform.anchoredPosition = new Vector2(x, y);
            }
        }
        
        protected override void Start()
        {
            base.Start();
            _text = GetComponent<TMP_Text>();
            _rectTransform = GetComponent<RectTransform>();
            Beneath.DelayOneFrameThen(this,() =>
            {
                var position = _rectTransform.anchoredPosition;
                _startingPos = new Vector2(position.x, position.y);
            });
        }
        
        protected override void ExecuteAction()
        {

            switch (buttonType)
            {
                case Type.Letter:
                    Manager.AppendLetterToPlayerName(_text.text[0]);
                    break;
                case Type.Backspace:
                    Manager.RemoveLetterFromPlayerName();
                    break;
                case Type.Done:
                    Manager.ConfirmName();
                    break;
            }
        }
    }
}
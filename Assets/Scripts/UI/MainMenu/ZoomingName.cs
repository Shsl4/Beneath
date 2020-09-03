using TMPro;
using UnityEngine;

namespace UI.MainMenu
{
    public class ZoomingName : MonoBehaviour
    {

        public string actualName = "";
        private TMP_Text _text;
        private Vector2 _startingPos;
        private RectTransform _rectTransform;
        private float _rumbleSpeed = 5.0f;
        private float _rumbleIntensity = 20.0f;
        private float _zoomTime = 3.0f;
        
        void FixedUpdate()
        {
            if (!_rectTransform) { return; }
            var x = _startingPos.x + (Random.Range(-_rumbleIntensity, _rumbleIntensity) * Time.deltaTime * _rumbleSpeed);
            var y = _startingPos.y + (Random.Range(-_rumbleIntensity, _rumbleIntensity) * Time.deltaTime * _rumbleSpeed);
            _rectTransform.anchoredPosition = new Vector2(x, y);
            
        }

        private float _currentScale;
        private void OnGUI()
        {
       
            if (_currentScale < 1.0f)
            {
                _currentScale += Time.deltaTime / _zoomTime;
                _rectTransform.localScale = new Vector3(_currentScale, _currentScale);
            }
        }
        
        protected void Start()
        {
            _text = GetComponent<TMP_Text>();
            _rectTransform = GetComponent<RectTransform>();
            Beneath.DelayOneFrameThen(this,() =>
            {
                var position = _rectTransform.anchoredPosition;
                _startingPos = new Vector2(position.x, position.y);
            });
            _text.text = actualName;
        }

        private void OnEnable()
        {
            _currentScale = 0.0f;
            if (!_text) { return; }
            _text.text = actualName;
        }
    }
}
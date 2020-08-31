using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.MainMenu
{
    public class MainMenuManager : UIManager
    {
        
        public GameObject titleView;
        public GameObject controlsView;
        public GameObject nameView;
        public GameObject nameValidationView;
        public TMP_Text characterNameText;
        public BeneathButton<MainMenuManager> aButton;
        public BeneathButton<MainMenuManager> noButton;
        public GameObject playerClass;
        
        private StartButton _startButton;
        private EventSystem _eventSystem;
        private CameraFader _fader;
        private string _playerName = "";

        public void AppendLetterToPlayerName(char letter)
        {

            int nameLength = _playerName.Length;
            char formatted;
            
            if (nameLength >= 10 || (nameLength <= 0 && letter == '-')) { return; }

            if (nameLength <= 0 || _playerName[nameLength - 1] == '-')
            {
                formatted = Char.ToUpper(letter);
            }
            else
            {
                formatted = Char.ToLower(letter);
            }
            
            _playerName += formatted;
            characterNameText.text = _playerName;
            
            if (_playerName.ToLower() == "frisk")
            {
                Beneath.ReloadScene();
            }
            
        }

        public void RemoveLetterFromPlayerName()
        {
            if (_playerName.Length <= 0) { return; }

            _playerName = _playerName.Remove(_playerName.Length - 1);
            characterNameText.text = _playerName;
        }
        
        private void Start()
        {
            _fader = GetComponent<CameraFader>();
            _eventSystem = GetComponent<EventSystem>();
            _startButton = GetComponentInChildren<StartButton>(true);
            _fader.FadeOut(3);
        }

        public void OnEnter()
        {

            _fader.FadeIn(0.5f);
            _eventSystem.SetSelectedGameObject(null);
            Beneath.DelayThen(this, 0.5f, () =>
            {
                titleView.SetActive(false);
                controlsView.SetActive(true);
                _fader.FadeOut(0.5f);
                _startButton.Select();
            });
            
        }
        
        public void OnStart()
        {

            _fader.FadeIn(0.5f);
            _eventSystem.SetSelectedGameObject(null);
            Beneath.DelayThen(this, 0.5f, () =>
            {
                controlsView.SetActive(false);
                nameView.SetActive(true);
                _fader.FadeOut(0.5f);
                aButton.Select();
            });
            
        }

        public void ConfirmName()
        {
            if (String.IsNullOrEmpty(_playerName)) { return; }
            
            _fader.FadeIn(0.5f);
            _eventSystem.SetSelectedGameObject(null);
            GetComponentInChildren<ZoomingName>(true).ActualName = _playerName;
            Beneath.DelayThen(this, 0.5f, () =>
            {
                nameView.SetActive(false);
                nameValidationView.SetActive(true);
                _fader.FadeOut(0.5f);
                noButton.Select();
            });
            
        }

        public void DenyName()
        {
            
            _fader.FadeIn(0.5f);
            _eventSystem.SetSelectedGameObject(null);
            Beneath.DelayThen(this, 0.5f, () =>
            {
                nameValidationView.SetActive(false);
                nameView.SetActive(true);
                _fader.FadeOut(0.5f);
                aButton.Select();
            });
            
        }

        public void BeginAdventure()
        {
            _fader.fadeToColor = Color.white;
            _fader.FadeIn(5.0f);
            _eventSystem.SetSelectedGameObject(null);
            Beneath.DelayThen(this, 5.0f, (() => Beneath.SaveManager.BeginGameWithName(_playerName)));
        }
        
        public override void NavigateBack() { }
    }
}
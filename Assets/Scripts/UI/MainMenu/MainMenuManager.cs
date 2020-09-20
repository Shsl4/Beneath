using System;
using TMPro;
using UI.General;
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
        public AudioClip startCymbal;

        private StartButton _startButton;
        private EventSystem _eventSystem;
        private string _playerName = "";
        private EnterButton _enterButton;

        protected override void Awake()
        {
            base.Awake();
            _enterButton = GetComponentInChildren<EnterButton>(true);
        }
        public void AppendLetterToPlayerName(char letter)
        {

            int nameLength = _playerName.Length;
            char formatted;
            
            if (nameLength >= 10 || (nameLength <= 0 && letter == '-')) { return; }

            if (nameLength <= 0 || _playerName[nameLength - 1] == '-')
            {
                formatted = char.ToUpper(letter);
            }
            else
            {
                formatted = char.ToLower(letter);
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
            _eventSystem = GetComponent<EventSystem>();
            _startButton = GetComponentInChildren<StartButton>(true);
            Beneath.instance.Fader.FadeOut(3);
        }

        public void OnEnter()
        {

            Beneath.instance.Fader.FadeIn(0.5f);
            _eventSystem.SetSelectedGameObject(null);
            Beneath.DelayThen(this, 0.5f, () =>
            {
                titleView.SetActive(false);
                controlsView.SetActive(true);
                Beneath.instance.Fader.FadeOut(0.5f);
                _startButton.Select();
                source.clip = _enterButton.menuTheme;
                source.loop = true;
                source.Play();
            });
            
        }
        
        public void OnStart()
        {

            Beneath.instance.Fader.FadeIn(0.5f);
            _eventSystem.SetSelectedGameObject(null);
            Beneath.DelayThen(this, 0.5f, () =>
            {
                controlsView.SetActive(false);
                nameView.SetActive(true);
                Beneath.instance.Fader.FadeOut(0.5f);
                aButton.Select();
            });
            
        }

        public void ConfirmName()
        {
            if (string.IsNullOrEmpty(_playerName)) { return; }
            
            Beneath.instance.Fader.FadeIn(0.5f);
            _eventSystem.SetSelectedGameObject(null);
            GetComponentInChildren<ZoomingName>(true).actualName = _playerName;
            Beneath.DelayThen(this, 0.5f, () =>
            {
                nameView.SetActive(false);
                nameValidationView.SetActive(true);
                Beneath.instance.Fader.FadeOut(0.5f);
                noButton.Select();
            });
            
        }

        public void DenyName()
        {
            
            Beneath.instance.Fader.FadeIn(0.5f);
            _eventSystem.SetSelectedGameObject(null);
            Beneath.DelayThen(this, 0.5f, () =>
            {
                nameValidationView.SetActive(false);
                nameView.SetActive(true);
                Beneath.instance.Fader.FadeOut(0.5f);
                aButton.Select();
            });
            
        }

        public void BeginAdventure()
        {
            Beneath.instance.Fader.fadeColor = Color.white;
            source.Stop();
            source.PlayOneShot(startCymbal);
            Beneath.instance.Fader.FadeIn(5.0f);
            _eventSystem.SetSelectedGameObject(null);
            Beneath.DelayThen(this, 3.0f, (() =>
            {

                nameValidationView.SetActive(false);
                Beneath.instance.Fader.FadeOut(5.0f);
                Beneath.DelayThen(this, 5.0f, () => Beneath.SaveManager.BeginGameWithName(_playerName));

            }));
        }
        
    }
    
}
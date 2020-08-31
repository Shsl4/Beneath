using System;
using System.Runtime.Serialization;
using Dummies;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.SaveMenu
{
    public class SaveMenuManager : UIManager
    {

        public SaveButton SaveBtn => GetComponentInChildren<SaveButton>(true);
        public BackDummy ReturnBtn => GetComponentInChildren<BackDummy>(true);
        public SaveConfirmationButton SaveConfirmation => GetComponentInChildren<SaveConfirmationButton>(true);
        
        public TMP_Text nameText;
        public TMP_Text levelText;
        public TMP_Text timeText;
        public TMP_Text areaText;

        private void UpdateDisplay()
        {
            if (Beneath.SaveManager.HasProgress())
            {
                Beneath.SaveData data = Beneath.SaveManager.LoadProgress();

                nameText.text = data.playerName;
                levelText.text = "LV" + data.playerExp;
                
                TimeSpan time = TimeSpan.FromSeconds(data.playTime);
                string str = time.ToString(@"mm\:ss");
                
                timeText.text = str;
                areaText.text = data.roomName;

            }
            else
            {
                nameText.text = "???";
                levelText.text = "LV0";
                timeText.text = "00:00";
                areaText.text = "No save data";
            }
        }
        
        public void OnSaved()
        {
            UpdateDisplay();
            nameText.color = Color.yellow;
            levelText.color = Color.yellow;
            timeText.color = Color.yellow;
            areaText.color = Color.yellow;
            SaveBtn.gameObject.SetActive(false);
            ReturnBtn.gameObject.SetActive(false);
            SaveConfirmation.gameObject.SetActive(true);
            SaveConfirmation.Select();
        }

        public override void Open()
        {
            UpdateDisplay();
            base.Open(); 
        }

        public override void Close()
        {
            base.Close();
            Destroy(gameObject);
        }
    }
}

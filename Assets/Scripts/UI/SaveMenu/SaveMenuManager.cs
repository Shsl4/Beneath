using System;
using TMPro;
using UI.General;
using UnityEngine;

namespace UI.SaveMenu
{
    public class SaveMenuManager : UIManager
    {

        public SaveButton SaveBeneathBtn => GetComponentInChildren<SaveButton>(true);
        public BeneathButton<UIManager> ReturnBtn => GetComponentInChildren<BeneathButton<UIManager>>(true);
        public SaveConfirmationButton SaveConfirmationBeneath => GetComponentInChildren<SaveConfirmationButton>(true);
        
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
                areaText.text = data.GetSavedRoomName();

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
            SaveBeneathBtn.gameObject.SetActive(false);
            ReturnBtn.gameObject.SetActive(false);
            SaveConfirmationBeneath.gameObject.SetActive(true);
            SaveConfirmationBeneath.Select();
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

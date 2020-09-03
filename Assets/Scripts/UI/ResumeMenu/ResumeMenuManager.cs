using System;
using TMPro;
using UI.General;

namespace UI.ResumeMenu
{
    public class ResumeMenuManager : UIManager
    {
        
        public ContinueButton ContinueBtn => GetComponentInChildren<ContinueButton>(true);

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

        public override void Open()
        {
            UpdateDisplay();
            base.Open(); 
        }
    }
}
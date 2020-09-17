using UI.General;
using UnityEngine;

namespace UI.EscapeMenu.Buttons.Settings
{
    public class ApplySettings : BeneathButton<SettingsManager>
    {
        protected override void SubmitAction()
        {
            Resolution res = Screen.resolutions[Manager.ResPicker.Selected];
            Screen.SetResolution(res.width, res.height, Manager.FullScreenPicker.BoolValue(), res.refreshRate);
            PlayerPrefs.SetFloat("masterVolume", Manager.VolumePicker.Selected / 10.0f);
            AudioListener.volume = PlayerPrefs.GetFloat("masterVolume");
            
        }
        
    }
}
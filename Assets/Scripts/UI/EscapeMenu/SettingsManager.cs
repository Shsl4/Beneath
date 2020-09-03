using UI.General;
using UnityEngine;

namespace UI.EscapeMenu
{
    public class SettingsManager : UIManager
    {
        public ResolutionPicker ResPicker => GetComponentInChildren<ResolutionPicker>();
        public BoolPicker FullScreenPicker => GetComponentInChildren<BoolPicker>();

        public override void OnEnable()
        {
            
            base.OnEnable();

            int selectedResolution = 0;
            
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                if (Screen.resolutions[i].Equals(Screen.currentResolution))
                {
                    selectedResolution = i;
                    break;
                }
            }
            
            ResPicker.SetSelected(selectedResolution);
            FullScreenPicker.SetSelected(Screen.fullScreen ? 1 : 0);
            
        }
    }
}
using UnityEngine;

namespace UI.General
{
    public class ResolutionPicker : OptionPicker
    {
        public override string[] GetChoices()
        {

            string[] res = new string[Screen.resolutions.Length];

            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                res[i] = Screen.resolutions[i].ToString();
            }

            return res;
        }

    }
    
}
using Assets.Scripts.UI;
using UnityEngine;

namespace UI
{
    
    public class DialogBoxManager : UIManager
    {

        public TextViewComponent TextView => GetComponentInChildren<TextViewComponent>();

        
        
        public void OpenWithText(ControllableCharacter character, string text)
        {
            Open(character);
            TextView.RevealText(text);
        }

    }
}
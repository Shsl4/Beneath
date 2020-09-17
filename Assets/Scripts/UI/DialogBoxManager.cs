using Interfaces;
using UI.General;

namespace UI
{
    
    public class DialogBoxManager : UIManager, IDialogBoxResponder
    {

        public TextViewComponent TextView => GetComponentInChildren<TextViewComponent>();

        public void OpenWithText(string text)
        {
            Open();
            TextView.RevealText(text);
        }

        public void DisplayWithText(string text, bool select)
        {
            if (select)
            {
                TextView.SelectAndReveal(text);
            }
            else
            {
                TextView.RevealText(text);
            }
        }

        public void DisplayEnded()
        {
            Close();
        }
    }
}
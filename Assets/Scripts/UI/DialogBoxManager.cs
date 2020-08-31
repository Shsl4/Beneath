namespace UI
{
    
    public class DialogBoxManager : UIManager
    {

        public TextViewComponent TextView => GetComponentInChildren<TextViewComponent>();

        
        
        public void OpenWithText(string text)
        {
            Open();
            TextView.RevealText(text);
        }

    }
}
using UI.General;

namespace UI.SaveMenu
{
    public class SaveButton : BeneathButton<SaveMenuManager>
    {
        protected override void SubmitAction()
        {
            Beneath.SaveManager.SaveProgress();
            Manager.OnSaved();
        }
        
    }
}
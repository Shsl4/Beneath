using UI.General;

namespace UI.ResumeMenu
{
    public class ContinueButton : BeneathButton
    {
        protected override void SubmitAction()
        {
            Beneath.SaveManager.ResumeGame();
        }
    }
}
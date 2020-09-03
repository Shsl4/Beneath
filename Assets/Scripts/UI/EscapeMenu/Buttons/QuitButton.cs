using UI.General;

namespace UI.EscapeMenu.Buttons
{
    public class QuitButton : BeneathButton<EscapeMenuManager>
    {
        protected override void SubmitAction()
        {
            Beneath.QuitGame();
        }
    }
}
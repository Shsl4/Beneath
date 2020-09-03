using UI.General;

namespace UI.MainMenu
{
    public class StartButton : BeneathButton<MainMenuManager>
    {
        protected override void SubmitAction()
        {
            Manager.OnStart();
        }
    }
}
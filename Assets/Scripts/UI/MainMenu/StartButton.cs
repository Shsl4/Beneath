namespace UI.MainMenu
{
    public class StartButton : BeneathButton<MainMenuManager>
    {
        protected override void ExecuteAction()
        {
            Manager.OnStart();
        }
    }
}
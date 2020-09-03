using UI.General;

namespace UI.MainMenu
{
    public class NameDecisionButton : BeneathButton<MainMenuManager>
    {

        public bool isAccept;
        protected override void SubmitAction()
        {
            if (isAccept)
            {
                Manager.BeginAdventure();
            }
            else
            {
                Manager.DenyName();
            }
        }
    }
}
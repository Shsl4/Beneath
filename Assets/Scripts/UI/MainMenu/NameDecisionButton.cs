using UnityEngine;

namespace UI.MainMenu
{
    public class NameDecisionButton : BeneathButton<MainMenuManager>
    {

        public bool isAccept;
        protected override void ExecuteAction()
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
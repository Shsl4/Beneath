using UnityEngine;

namespace UI.MainMenu
{
    public class EnterButton : BeneathButton<MainMenuManager>
    {
        protected override void ExecuteAction()
        {
            Manager.OnEnter();
        }
    }
}
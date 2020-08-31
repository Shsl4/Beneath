using UI.ResumeMenu;
using UnityEngine;

namespace UI.MainMenu
{
    public class EnterButton : BeneathButton<MainMenuManager>
    {
        protected override void ExecuteAction()
        {

            if (Beneath.SaveManager.HasProgress())
            {
                ((GameObject)Instantiate(Beneath.Assets.ResumeMenu.Asset)).GetComponent<ResumeMenuManager>().Open();
            }
            else
            {
                Manager.OnEnter();
            }
            
        }
    }
}
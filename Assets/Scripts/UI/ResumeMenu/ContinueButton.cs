using UnityEngine;

namespace UI.ResumeMenu
{
    public class ContinueButton : BeneathButton<ResumeMenuManager>
    {
        protected override void ExecuteAction()
        {
            Beneath.SaveManager.ResumeGame();
        }
    }
}
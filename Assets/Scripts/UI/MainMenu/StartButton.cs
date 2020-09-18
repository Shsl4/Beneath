using UI.General;
using UnityEditor;
using UnityEngine;

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
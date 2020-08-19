using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu.Buttons
{
    public class ExitButton : BeneathButton<MenuManager>
    {
        protected override void ExecuteAction()
        {
            Beneath.QuitGame();
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Master.NavigateBack();
        }
    }
}
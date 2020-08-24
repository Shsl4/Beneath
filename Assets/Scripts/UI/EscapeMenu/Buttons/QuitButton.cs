using UnityEngine.EventSystems;

namespace UI.EscapeMenu.Buttons
{
    public class QuitButton : BeneathButton<EscapeMenuManager>
    {
        protected override void ExecuteAction()
        {
            Beneath.QuitGame();
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Manager.NavigateBack();
        }
    }
}
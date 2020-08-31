using UnityEngine.EventSystems;

namespace UI.EscapeMenu.Buttons
{
    public class InventoryButton : BeneathButton<EscapeMenuManager>
    {
        protected override void ExecuteAction()
        {
            Manager.LastSubmit = gameObject;
            Manager.DisableSelection();
            Manager.InventoryMgr.Open();
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Manager.NavigateBack();
        }
        
    }
}

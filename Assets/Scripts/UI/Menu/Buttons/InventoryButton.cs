using UnityEngine.EventSystems;

namespace UI.Menu.Buttons
{
    public class InventoryButton : BeneathButton<MenuManager>
    {
        protected override void ExecuteAction()
        {
            Master.LastSubmit = gameObject;
            Master.DisableSelection();
            Master.InventoryMgr.Open(Master.Viewer);
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Master.NavigateBack();
        }
        
    }
}

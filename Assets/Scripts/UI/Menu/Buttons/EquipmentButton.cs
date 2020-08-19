using UnityEngine.EventSystems;

namespace UI.Menu.Buttons
{
    public class EquipmentButton : BeneathButton<MenuManager>
    {
        protected override void ExecuteAction()
        {
            Master.LastSubmit = gameObject;
            Master.DisableSelection();
            Master.EquipmentMgr.Open(Master.Viewer);
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Master.NavigateBack();
        }
        
    }
}
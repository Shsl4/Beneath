using UnityEngine.EventSystems;

namespace UI.EscapeMenu.Buttons
{
    public class EquipmentButton : BeneathButton<EscapeMenuManager>
    {
        protected override void ExecuteAction()
        {
            Manager.LastSubmit = gameObject;
            Manager.DisableSelection();
            Manager.EquipmentMgr.Open();
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Manager.NavigateBack();
        }
        
    }
}
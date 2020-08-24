using Interfaces;
using UnityEngine.EventSystems;

namespace UI.EscapeMenu.Equipment
{
    public class UnEquipButton : BeneathButton<EquipmentManager>
    {
        protected override void ExecuteAction()
        {
            if (Manager is ISlotEventResponder responder)
            {
                responder.DropItemFromActiveSlot(false);
            }
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Manager.EventSys.SetSelectedGameObject(Manager.LastSubmit);
            Manager.LastSubmit = null;
            Manager.DisableSelection();
        }
        
    }
}
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu
{
    public class UnEquipButton : BeneathButton<EquipmentManager>
    {
        protected override void ExecuteAction()
        {
            if (Master is ISlotEventResponder responder)
            {
                responder.DropItemFromActiveSlot(false);
            }
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Master.EventSys.SetSelectedGameObject(Master.LastSubmit);
            Master.LastSubmit = null;
            Master.DisableSelection();
        }
        
    }
}
using Interfaces;
using UnityEngine.EventSystems;

namespace UI.Inventory.BottomView.Selection
{
    public class DiscardButton : BeneathButton<MasterInterface>
    {
        protected override void ExecuteAction()
        {
            if (Master is ISlotEventResponder responder)
            {
                responder.DropItemFromActiveSlot(true);
            }
            
            if (Master is IDialogBoxResponder dialogBoxResponder)
            {
                Master.DisableSelection();
                dialogBoxResponder.DisplayWithText("The item was thrown away.", true);
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

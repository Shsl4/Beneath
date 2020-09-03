using Interfaces;
using UI.General;

namespace UI.EscapeMenu.Equipment
{
    public class UnEquipButton : BeneathButton<EquipmentManager>
    {
        protected override void SubmitAction()
        {
            Manager.DropItemFromActiveSlot(false);
        }

        protected override void CancelAction()
        {
            
            if (Manager is ISlotEventResponder responder)
            {
                responder.GetActiveSlot()?.Select();
                Manager.DisableSelection();
            }
            
        }
    }
}
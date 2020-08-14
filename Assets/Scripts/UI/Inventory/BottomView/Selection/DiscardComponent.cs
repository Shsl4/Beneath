using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Inventory.BottomView.Selection
{
    public class DiscardComponent : InventoryInterface
    {
        public override void OnSelect(BaseEventData eventData) { }

        public override void OnSubmit(BaseEventData eventData)
        {
            DiscardObject();
        }

        public override void OnCancel(BaseEventData eventData)
        {
            Manager.SelectedSlot.Selectable.Select();
            Manager.SelectedSlot = null;
        }

        private void DiscardObject()
        {
            
            
        }
    }
}

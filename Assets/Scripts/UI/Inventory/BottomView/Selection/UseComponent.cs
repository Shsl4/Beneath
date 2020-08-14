using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Inventory.BottomView.Selection
{
    public class UseComponent : InventoryInterface
    {

        public override void OnSelect(BaseEventData eventData) { }

        public override void OnSubmit(BaseEventData eventData)
        {
            UseObject();
        }

        public override void OnCancel(BaseEventData eventData)
        {
            Manager.SelectedSlot.Selectable.Select();
            Manager.SelectedSlot = null;
        }

        private void UseObject()
        {
            
            
        }
        
    }
}

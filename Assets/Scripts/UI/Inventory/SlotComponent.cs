using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class SlotComponent : BeneathButton<UIManager>
    {
        
        private InventoryItem _heldItem;
        public Selectable Selectable => GetComponent<Selectable>();
        public int slotIndex;

        public void SetHeldItem(InventoryItem item)
        {

            Image imageComponent = GetComponentsInChildren<Image>()[1];
            _heldItem = item;

            if (_heldItem == null || !_heldItem.sprite) { imageComponent.color = Color.black; }
            else
            {
                imageComponent.sprite = _heldItem.sprite;
                imageComponent.color = Color.white;
            }

        }

        public override void OnCancel(BaseEventData eventData)
        {
            Manager.NavigateBack();
        }

        public InventoryItem GetHeldItem()
        {
            return _heldItem;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            
            if (Manager is ISlotEventResponder responder)
            {
                responder.RefreshInfoFromSlot(this);
            }
        }

        protected override void ExecuteAction()
        {
            if (_heldItem != null)
            {
                Manager.LastSubmit = gameObject;
                if (Manager is ISlotEventResponder responder)
                {
                    responder.JumpToSelection();
                }
            }        
        }
    }
}

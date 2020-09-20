using Interfaces;
using UI.General;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class SlotComponent : BeneathButton<UIManager>
    {
        
        private ItemData _heldItemData;
        public Selectable Selectable => GetComponent<Selectable>();
        public int slotIndex;

        public void SetHeldItem(ItemData itemData)
        {

            Image imageComponent = GetComponentsInChildren<Image>()[1];
            _heldItemData = itemData;

            if (_heldItemData == null || !_heldItemData.Sprite) { imageComponent.color = Color.black; }
            else
            {
                imageComponent.sprite = _heldItemData.Sprite;
                imageComponent.color = Color.white;
            }

        }
        
        public ItemData GetHeldItem()
        {
            return _heldItemData;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            
            if (Manager is ISlotEventResponder responder)
            {
                responder.RefreshInfoFromSlot(this);
            }
        }

        protected override void SubmitAction()
        {
            if (_heldItemData != null)
            {
                if (Manager is ISlotEventResponder responder)
                {
                    responder.SetActiveSlot(this);
                    responder.JumpToSelection();
                }
            }        
        }
    }
    
}

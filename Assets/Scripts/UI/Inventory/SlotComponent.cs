using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class SlotComponent : InventoryInterface
    {
        
        private InventoryItem _heldItem;
        public Selectable Selectable => GetComponent<Selectable>();
        
        public void SetHeldItem(InventoryItem item)
        {

            Image imageComponent = GetComponentsInChildren<Image>()[1];
            _heldItem = item;
            imageComponent.sprite = _heldItem.Sprite;

            if (imageComponent.sprite == null) { imageComponent.color = Color.black; }
            else { imageComponent.color = Color.white; }

        }

        public override void OnSelect(BaseEventData eventData)
        {
            Manager.Bottom.UpdateInfo(_heldItem);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            Manager.SelectedSlot = this;
            Manager.Bottom.Selection.Use.gameObject.GetComponent<Selectable>().Select();
        }

        public override void OnCancel(BaseEventData eventData)
        {
            Manager.gameObject.SetActive(false);
        }
    }
}

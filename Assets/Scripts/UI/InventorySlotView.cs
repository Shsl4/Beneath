using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InventorySlotView : MonoBehaviour
    {
    
        private InventoryItem _heldItem;

        public void SetHeldItem(InventoryItem item)
        {

            Image imageComponent = GetComponentsInChildren<Image>()[1];
            _heldItem = item;
            imageComponent.sprite = _heldItem.Sprite;

            if (imageComponent.sprite == null) { imageComponent.color = Color.black; }
            else { imageComponent.color = Color.white; }

        }
    }
}

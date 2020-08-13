using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ChestView : MonoBehaviour
    {

        public InventoryInfoView InfoView;
        public void SetupSlots(Inventory target)
        {

            InventorySlotView[] slots = GetComponentsInChildren<InventorySlotView>();

            for (int i = 0; i < slots.Length; i++)
            {
                if (target.GetSlot(i) != null && target.GetSlot(i).GetItem() != null)
                {
                    slots[i].SetHeldItem(target.GetSlot(i).GetItem());
                    InfoView.UpdateInfo(target.GetSlot(i).GetItem());
                }
            }

        }
    
    }
}

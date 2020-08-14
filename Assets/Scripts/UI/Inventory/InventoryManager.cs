using Assets.Scripts.UI.Inventory.BottomView.Info;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public SlotComponent[] Slots => GetComponentsInChildren<SlotComponent>();
        public BottomComponent Bottom => GetComponentInChildren<BottomComponent>();

        [CanBeNull]
        public SlotComponent SelectedSlot;

        public void SetupSlots(Scripts.Inventory target)
        {
            
            for (int i = 0; i < Slots.Length; i++)
            {
                if (target.GetSlot(i) != null && target.GetSlot(i).GetItem() != null)
                {
                    Slots[i].SetHeldItem(target.GetSlot(i).GetItem());
                }
            }
        }

        public void OnDisable()
        {
            SelectedSlot = null;
        }

        public void OnEnable()
        {
            SelectedSlot = null;
            Slots[0].Selectable.Select();
        }
    }
    
}

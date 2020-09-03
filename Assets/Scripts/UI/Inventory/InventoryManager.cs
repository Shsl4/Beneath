using Interfaces;
using UI.General;
using UI.Inventory.BottomView.Info;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryManager : UIManager, ISlotEventResponder, IDialogBoxResponder
    {
        private SlotComponent[] Slots => GetComponentsInChildren<SlotComponent>();
        public BottomComponent Bottom => GetComponentInChildren<BottomComponent>();
        
        private SlotComponent _activeSlot;
        
        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i].slotIndex = i;
            }
            
        }

        public void RefreshSlots()
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Beneath.Data.PlayerInventory.GetSlot(i) != null)
                {
                    Slots[i].SetHeldItem(Beneath.Data.PlayerInventory.GetSlot(i).GetItem());
                }
            }
        }
        public override void Open()
        {
            base.Open();
            RefreshSlots();
        }

        public override void EnableSelection()
        {

            Bottom.Selection.UseBeneath.interactable = true;
            Bottom.Selection.DiscardBeneath.interactable = true;

        }

        public override void DisableSelection()
        {

            Bottom.Selection.UseBeneath.interactable = false;
            Bottom.Selection.DiscardBeneath.interactable = false;

        }

        public void JumpToSelection()
        {
            EnableSelection();
            Bottom.Selection.UseBeneath.gameObject.GetComponent<Selectable>().Select();
        }

        public void SetActiveSlot(SlotComponent slot)
        {
            _activeSlot = slot;
        }

        public SlotComponent GetActiveSlot()
        {
            return _activeSlot;
        }

        public void RefreshInfoFromSlot(SlotComponent slot)
        {
            
            Bottom.UpdateInfo(slot.GetHeldItem());

            if (slot.GetHeldItem() != null && (slot.GetHeldItem().type == ItemTypes.Armor || slot.GetHeldItem().type == ItemTypes.Weapon))
            {
                
                Bottom.Selection.UseBeneath.TextBox.SetText("EQUIP");
                
            }
            else
            {
                Bottom.Selection.UseBeneath.TextBox.SetText("USE");
            }
        }

        public void DropItemFromActiveSlot(bool discard)
        {
            Beneath.Data.player.DropItemFromSlot(_activeSlot.GetComponent<SlotComponent>().slotIndex);
            RefreshSlots();
        }

        public void DisplayWithText(string text, bool select)
        {

            if (select)
            {
                Bottom.TextView.SelectAndReveal(text);
            }
            else
            {
                Bottom.TextView.RevealText(text);
            }
            
        }

        public void DisplayEnded()
        {
            _activeSlot.Select();
            _activeSlot = null;
        }
    }
    
}

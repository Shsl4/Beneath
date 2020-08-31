using Interfaces;
using UI.Inventory.BottomView.Info;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryManager : UIManager, ISlotEventResponder, IDialogBoxResponder
    {
        private SlotComponent[] Slots => GetComponentsInChildren<SlotComponent>();
        public BottomComponent Bottom => GetComponentInChildren<BottomComponent>();
        
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

            Bottom.Selection.Use.interactable = true;
            Bottom.Selection.Discard.interactable = true;

        }

        public override void DisableSelection()
        {

            Bottom.Selection.Use.interactable = false;
            Bottom.Selection.Discard.interactable = false;

        }

        public void JumpToSelection()
        {
            EnableSelection();
            Bottom.Selection.Use.gameObject.GetComponent<Selectable>().Select();
        }

        public void RefreshInfoFromSlot(SlotComponent slot)
        {
            
            Bottom.UpdateInfo(slot.GetHeldItem());

            if (slot.GetHeldItem() != null && (slot.GetHeldItem().type == ItemTypes.Armor || slot.GetHeldItem().type == ItemTypes.Weapon))
            {
                
                Bottom.Selection.Use.TextBox.SetText("EQUIP");
                
            }
            else
            {
                Bottom.Selection.Use.TextBox.SetText("USE");
            }
        }

        public void DropItemFromActiveSlot(bool discard)
        {
            Beneath.Data.player.DropItemFromSlot(LastSubmit.GetComponent<SlotComponent>().slotIndex);
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
    }
    
}

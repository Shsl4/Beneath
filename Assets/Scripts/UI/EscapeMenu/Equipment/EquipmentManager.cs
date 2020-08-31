using System;
using Interfaces;
using UI.Inventory;

namespace UI.EscapeMenu.Equipment
{
    public class EquipmentManager : UIManager, IDialogBoxResponder, ISlotEventResponder
    {

        public UnEquipButton UnEquipBtn => GetComponentInChildren<UnEquipButton>(true);
        public DiscardButton DiscardBtn => GetComponentInChildren<DiscardButton>(true);
        public TextViewComponent TextView => GetComponentInChildren<TextViewComponent>(true);
        public SlotComponent WeaponSlot => GetComponentsInChildren<SlotComponent>(true)[0];
        public SlotComponent ArmorSlot => GetComponentsInChildren<SlotComponent>(true)[1];
        
        public void RefreshSlots()
        {
            WeaponSlot.SetHeldItem(Beneath.Data.PlayerWeapon.GetItem());
            ArmorSlot.SetHeldItem(Beneath.Data.PlayerArmor.GetItem());
        }

        public override void Open()
        {
            base.Open();
            RefreshSlots();
        }

        public void RefreshInfoFromSlot(SlotComponent slot)
        {

            InventoryItem item = slot.GetHeldItem();
            String output = "";

            if (item != null)
            {
                
                output += "Name: " + item.name + "\n\n";
                output += item.FormatDescription();

            }
            else
            {
                output = "This slot is empty.";
            }
            
            TextView.RevealText(output);
            
        }

        public void DropItemFromActiveSlot(bool discard)
        {

            Beneath.UnEquipResult result;
            string itemName = "";
            
            if (LastSubmit == ArmorSlot.gameObject)
            {
                itemName = Beneath.Data.PlayerArmor.GetItem().name;
                result = Beneath.Data.player.UnEquipArmor(discard);
            }
            else
            {
                itemName = Beneath.Data.PlayerWeapon.GetItem().name;
                result = Beneath.Data.player.UnEquipWeapon(discard);
            }

            string message = "";
            
            switch (result)
            {
  
                case Beneath.UnEquipResult.Success:

                    if (discard)
                    {
                        message = "You threw \"" + itemName + "\" away.";
                    }
                    else
                    {
                        message = "You placed \"" + itemName + "\" back in your inventory.";
                    }
                    break;

                case Beneath.UnEquipResult.InventoryFull:
                    message = "You tried to unequip \"" + itemName + "\", but your inventory is full...";
                    break;
                    
                case Beneath.UnEquipResult.Error:
                    message = "You tried to unequip \"" + itemName + "\", but it failed...";
                    break;
                
            }
            
            DisableSelection();
            TextView.SelectAndReveal(message);
            RefreshSlots();
        }

        public void JumpToSelection()
        {
            EnableSelection();
            UnEquipBtn.Select();        
        }

        public override void EnableSelection()
        {
            
            UnEquipBtn.interactable = true;
            DiscardBtn.interactable = true;
            
        }

        public override void DisableSelection()
        {

            UnEquipBtn.interactable = false;
            DiscardBtn.interactable = false;

        }

        public void DisplayWithText(string text, bool select)
        {
            if (select)
            {
                TextView.SelectAndReveal(text);

            }
            else
            {
                TextView.RevealText(text);
            }
        }
    }
}

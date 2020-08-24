using System;
using Assets.Scripts.UI;
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
            WeaponSlot.SetHeldItem(Viewer.CharacterWeapon.GetItem());
            ArmorSlot.SetHeldItem(Viewer.CharacterArmor.GetItem());
        }

        public override void Open(ControllableCharacter character)
        {
            base.Open(character);
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
                output = output.ToUpper();

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
                itemName = Viewer.CharacterArmor.GetItem().name;
                result = Viewer.UnEquipArmor(discard);
            }
            else
            {
                itemName = Viewer.CharacterWeapon.GetItem().name;
                result = Viewer.UnEquipWeapon(discard);
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

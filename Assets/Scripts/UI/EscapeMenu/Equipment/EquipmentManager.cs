using System;
using Interfaces;
using UI.General;
using UI.Inventory;
using UI.Inventory.BottomView.Selection;

namespace UI.EscapeMenu.Equipment
{
    public class EquipmentManager : UIManager, IDialogBoxResponder, ISlotEventResponder
    {

        public UnEquipButton UnEquipBeneathBtn => GetComponentInChildren<UnEquipButton>(true);
        public DiscardButton DiscardBeneathBtn => GetComponentInChildren<DiscardButton>(true);
        public TextViewComponent TextView => GetComponentInChildren<TextViewComponent>(true);
        public SlotComponent WeaponSlot => GetComponentsInChildren<SlotComponent>(true)[0];
        public SlotComponent ArmorSlot => GetComponentsInChildren<SlotComponent>(true)[1];

        private SlotComponent _activeSlot;
        
        public void RefreshSlots()
        {
            WeaponSlot.SetHeldItem(Beneath.instance.WeaponSlot.GetItem());
            ArmorSlot.SetHeldItem(Beneath.instance.ArmorSlot.GetItem());
        }

        public override void Open()
        {
            base.Open();
            RefreshSlots();
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

            ItemData itemData = slot.GetHeldItem();
            String output = "";

            if (itemData != null)
            {
                
                output += "Name: " + itemData.name + "\n\n";
                output += itemData.FormatDescription();

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
            string itemName;
            
            if (_activeSlot == ArmorSlot)
            {
                itemName = Beneath.instance.ArmorSlot.GetItem().name;
                result = Beneath.instance.player.UnEquipArmor(discard);
            }
            else
            {
                itemName = Beneath.instance.WeaponSlot.GetItem().name;
                result = Beneath.instance.player.UnEquipWeapon(discard);
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
            UnEquipBeneathBtn.Select();        
        }

        public override void EnableSelection()
        {
            
            UnEquipBeneathBtn.interactable = true;
            DiscardBeneathBtn.interactable = true;
            
        }

        public override void DisableSelection()
        {

            UnEquipBeneathBtn.interactable = false;
            DiscardBeneathBtn.interactable = false;

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
        
        public void DisplayEnded()
        {
            _activeSlot.Select();
            _activeSlot = null;
        }
    }
}

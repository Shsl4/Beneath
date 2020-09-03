using UI.General;

namespace UI.Inventory.BottomView.Selection
{
    public class UseButton : BeneathButton<InventoryManager>
    {
        
        protected override void SubmitAction()
        {

            SlotComponent slot = Manager.GetActiveSlot().GetComponent<SlotComponent>();
            
            if (slot.GetHeldItem().type == ItemTypes.Weapon)
            {

                Beneath.EquipResult result = Beneath.Data.player.EquipWeapon(slot.slotIndex);
                string message = "";
                
                switch (result)
                {
  
                    case Beneath.EquipResult.Success:
                        message = "You equipped \"" + Beneath.Data.PlayerWeapon.GetItem().name + "\".";
                        break;

                    case Beneath.EquipResult.AlreadyEquipped:
                        message = "You tried to equip \"" + slot.GetHeldItem().name + "\", but a weapon was already equipped...";
                        break;
                    
                    case Beneath.EquipResult.Error:
                        message = "You tried to equip \"" + slot.GetHeldItem().name + "\", but it failed...";
                        break;
                    
                }
                
                Manager.DisableSelection();
                Manager.Bottom.TextView.SelectAndReveal(message);

            }          
            else if (slot.GetHeldItem().type == ItemTypes.Armor)
            {
                    
                Beneath.EquipResult result = Beneath.Data.player.EquipArmor(slot.slotIndex);
                string message = "";
                
                switch (result)
                {
  
                    case Beneath.EquipResult.Success:
                        message = "You equipped \"" + Beneath.Data.PlayerArmor.GetItem().name + "\".";
                        Manager.RefreshSlots();
                        break;

                    case Beneath.EquipResult.AlreadyEquipped:
                        message = "You tried to equip \"" + slot.GetHeldItem().name + "\", but an armor was already equipped...";
                        break;
                    
                    case Beneath.EquipResult.Error:
                        message = "You tried to equip \"" + slot.GetHeldItem().name + "\", but it failed...";
                        break;
                }
                
                Manager.DisableSelection();
                Manager.Bottom.TextView.SelectAndReveal(message);
                    
            }
            
            Manager.RefreshSlots();
            
        }

        protected override void CancelAction()
        {
            Manager.GetActiveSlot().Select();
        }
    }
}

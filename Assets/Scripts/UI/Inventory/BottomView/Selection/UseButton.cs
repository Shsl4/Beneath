using UnityEngine.EventSystems;

namespace UI.Inventory.BottomView.Selection
{
    public class UseButton : BeneathButton<InventoryManager>
    {
        
        protected override void ExecuteAction()
        {

            SlotComponent Slot = Manager.LastSubmit.GetComponent<SlotComponent>();
            
            if (Slot.GetHeldItem().type == ItemTypes.Weapon)
            {

                Beneath.EquipResult result = Beneath.Data.player.EquipWeapon(Slot.slotIndex);
                string message = "";
                
                switch (result)
                {
  
                    case Beneath.EquipResult.Success:
                        message = "You equipped \"" + Beneath.Data.PlayerWeapon.GetItem().name + "\".";
                        break;

                    case Beneath.EquipResult.AlreadyEquipped:
                        message = "You tried to equip \"" + Slot.GetHeldItem().name + "\", but a weapon was already equipped...";
                        break;
                    
                    case Beneath.EquipResult.Error:
                        message = "You tried to equip \"" + Slot.GetHeldItem().name + "\", but it failed...";
                        break;
                    
                }
                
                Manager.DisableSelection();
                Manager.Bottom.TextView.SelectAndReveal(message);

            }          
            else if (Slot.GetHeldItem().type == ItemTypes.Armor)
            {
                    
                Beneath.EquipResult result = Beneath.Data.player.EquipArmor(Slot.slotIndex);
                string message = "";
                
                switch (result)
                {
  
                    case Beneath.EquipResult.Success:
                        message = "You equipped \"" + Beneath.Data.PlayerArmor.GetItem().name + "\".";
                        Manager.RefreshSlots();
                        break;

                    case Beneath.EquipResult.AlreadyEquipped:
                        message = "You tried to equip \"" + Slot.GetHeldItem().name + "\", but an armor was already equipped...";
                        break;
                    
                    case Beneath.EquipResult.Error:
                        message = "You tried to equip \"" + Slot.GetHeldItem().name + "\", but it failed...";
                        break;
                }
                
                Manager.DisableSelection();
                Manager.Bottom.TextView.SelectAndReveal(message);
                    
            }
            
            Manager.RefreshSlots();
            
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Manager.EventSys.SetSelectedGameObject(Manager.LastSubmit);
            Manager.LastSubmit = null;
            Manager.DisableSelection();
        }
    }
}

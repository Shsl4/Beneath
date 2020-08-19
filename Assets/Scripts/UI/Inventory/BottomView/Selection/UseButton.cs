using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory.BottomView.Selection
{
    public class UseButton : BeneathButton<InventoryManager>
    {
        
        protected override void ExecuteAction()
        {

            Character Player = Master.Viewer;
            SlotComponent Slot = Master.LastSubmit.GetComponent<SlotComponent>();
            
            if (Slot.GetHeldItem().type == ItemTypes.Weapon)
            {

                Beneath.EquipResult result = Player.EquipWeapon(Slot.slotIndex);
                string message = "";
                
                switch (result)
                {
  
                    case Beneath.EquipResult.Success:
                        message = "You equipped \"" + Player.CharacterWeapon.GetItem().name + "\".";
                        break;

                    case Beneath.EquipResult.AlreadyEquipped:
                        message = "You tried to equip \"" + Slot.GetHeldItem().name + "\", but a weapon was already equipped...";
                        break;
                    
                    case Beneath.EquipResult.Error:
                        message = "You tried to equip \"" + Slot.GetHeldItem().name + "\", but it failed...";
                        break;
                    
                }
                
                Master.DisableSelection();
                Master.Bottom.TextView.RevealAndSelect(message);

            }          
            else if (Slot.GetHeldItem().type == ItemTypes.Armor)
            {
                    
                Beneath.EquipResult result = Player.EquipArmor(Slot.slotIndex);
                string message = "";
                
                switch (result)
                {
  
                    case Beneath.EquipResult.Success:
                        message = "You equipped \"" + Player.CharacterWeapon.GetItem().name + "\".";
                        Master.RefreshSlots();
                        break;

                    case Beneath.EquipResult.AlreadyEquipped:
                        message = "You tried to equip \"" + Slot.GetHeldItem().name + "\", but an armor was already equipped...";
                        break;
                    
                    case Beneath.EquipResult.Error:
                        message = "You tried to equip \"" + Slot.GetHeldItem().name + "\", but it failed...";
                        break;
                }
                
                Master.DisableSelection();
                Master.Bottom.TextView.RevealAndSelect(message);
                    
            }
            
            Master.RefreshSlots();
            
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Master.EventSys.SetSelectedGameObject(Master.LastSubmit);
            Master.LastSubmit = null;
            Master.DisableSelection();
        }
    }
}

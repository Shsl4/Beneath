using UI.EscapeMenu.Buttons;
using UI.EscapeMenu.Equipment;
using UI.General;
using UI.Inventory;
using UnityEngine.UI;

namespace UI.EscapeMenu
{
    public class EscapeMenuManager : UIManager
    {
        
        public InventoryManager InventoryMgr => GetComponentInChildren<InventoryManager>(true);
        public EquipmentManager EquipmentMgr => GetComponentInChildren<EquipmentManager>(true);
        public StatsManager StatsMgr => GetComponentInChildren<StatsManager>(true);
        public SettingsManager SettingsMgr => GetComponentInChildren<SettingsManager>(true);

        public Selectable inventoryBtn;
        public Selectable equipmentBtn;
        public Selectable statsBtn;
        public Selectable settingsBtn;
        public Selectable exitBtn;
        
        public override void EnableSelection()
        {

            inventoryBtn.interactable = true;
            equipmentBtn.interactable = true;
            statsBtn.interactable = true;
            settingsBtn.interactable = true;
            exitBtn.interactable = true;

        }

        public override void DisableSelection()
        {
            
            inventoryBtn.interactable = false;
            equipmentBtn.interactable = false;
            statsBtn.interactable = false;
            settingsBtn.interactable = false;
            exitBtn.interactable = false;
            
        }

    }
}

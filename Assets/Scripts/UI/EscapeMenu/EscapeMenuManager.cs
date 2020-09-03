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

        public Selectable InventoryBtn;
        public Selectable EquipmentBtn;
        public Selectable StatsBtn;
        public Selectable SettingsBtn;
        public Selectable ExitBtn;
        
        public override void EnableSelection()
        {

            InventoryBtn.interactable = true;
            EquipmentBtn.interactable = true;
            StatsBtn.interactable = true;
            SettingsBtn.interactable = true;
            ExitBtn.interactable = true;

        }

        public override void DisableSelection()
        {
            
            InventoryBtn.interactable = false;
            EquipmentBtn.interactable = false;
            StatsBtn.interactable = false;
            SettingsBtn.interactable = false;
            ExitBtn.interactable = false;
            
        }

    }
}

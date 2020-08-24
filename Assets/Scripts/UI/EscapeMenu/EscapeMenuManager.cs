using Assets.Scripts.UI;
using UI.EscapeMenu.Buttons;
using UI.EscapeMenu.Equipment;
using UI.Inventory;

namespace UI.EscapeMenu
{
    public class EscapeMenuManager : UIManager
    {
        
        public InventoryManager InventoryMgr => GetComponentInChildren<InventoryManager>(true);
        public EquipmentManager EquipmentMgr => GetComponentInChildren<EquipmentManager>(true);
        public StatsManager StatsMgr => GetComponentInChildren<StatsManager>(true);
        public SettingsManager SettingsMgr => GetComponentInChildren<SettingsManager>(true);

        public BeneathButton<EscapeMenuManager> InventoryBtn => GetComponentInChildren<InventoryButton>();
        public BeneathButton<EscapeMenuManager> EquipmentBtn => GetComponentInChildren<EquipmentButton>();
        public BeneathButton<EscapeMenuManager> StatsBtn => GetComponentInChildren<StatsButton>();
        public BeneathButton<EscapeMenuManager> SettingsBtn => GetComponentInChildren<SettingsButton>();
        public BeneathButton<EscapeMenuManager> ExitBtn => GetComponentInChildren<QuitButton>();
        
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

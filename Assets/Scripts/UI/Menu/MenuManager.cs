using System;
using System.Collections;
using Interfaces.UI;
using UI.Inventory;
using UI.Menu.Buttons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MenuManager : MasterInterface
    {
        
        public InventoryManager InventoryMgr => GetComponentInChildren<InventoryManager>(true);
        public EquipmentManager EquipmentMgr => GetComponentInChildren<EquipmentManager>(true);
        public SettingsManager SettingsMgr => GetComponentInChildren<SettingsManager>(true);

        public BeneathButton<MenuManager> InventoryBtn => GetComponentInChildren<InventoryButton>();
        public BeneathButton<MenuManager> EquipmentBtn => GetComponentInChildren<EquipmentButton>();
        public BeneathButton<MenuManager> SettingsBtn => GetComponentInChildren<SettingsButton>();
        public BeneathButton<MenuManager> ExitBtn => GetComponentInChildren<ExitButton>();
        
        public override void EnableSelection()
        {

            InventoryBtn.interactable = true;
            EquipmentBtn.interactable = true;
            SettingsBtn.interactable = true;
            ExitBtn.interactable = true;

        }

        public override void DisableSelection()
        {
            
            InventoryBtn.interactable = false;
            EquipmentBtn.interactable = false;
            SettingsBtn.interactable = false;
            ExitBtn.interactable = false;
            
        }

    }
}

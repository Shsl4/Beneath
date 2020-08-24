using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class DataHolder : MonoBehaviour
    {
        
        public InventorySlot CharacterArmor { get; } = new InventorySlot();
        public InventorySlot CharacterWeapon { get; } = new InventorySlot();
        public Inventory CharacterInventory;
        
        [HideInInspector]
        public ControllableCharacter Character;
        
        protected void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Beneath.Data = this;

            Beneath.Assets.Item.LoadAssetAsync<GameObject>();
            Beneath.Assets.DialogBox.LoadAssetAsync<GameObject>();
            Beneath.Assets.EscapeMenu.LoadAssetAsync<GameObject>();
            Beneath.Assets.PlayerCharacter.LoadAssetAsync<GameObject>();
            Beneath.Assets.SaveMenu.LoadAssetAsync<GameObject>();
            
            SceneManager.LoadScene("MainMenu");
            
        }

        public void LoadDataFromSave(Beneath.SaveData data)
        {
            
        }

        public void Spawn()
        {
            Beneath.Assets.PlayerCharacter.InstantiateAsync();
        }
        
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class Beneath
{

    public enum EquipResult
    {
        
        Success,
        AlreadyEquipped,
        Error
        
    }    
    
    public enum UnEquipResult
    {
        
        Success,
        InventoryFull,
        Error
        
    }
    public static class Assets
    {
        public static readonly AssetReference SansSprite = new AssetReference("Assets/Art/Sprites/sans.png");
        public static readonly AssetReference RawItem = new AssetReference("Assets/Prefabs/RawItem.prefab");
        
        public static readonly AssetReference InventoryInterface = new AssetReference("Assets/Prefabs/UI/InventoryInterface.prefab");
        public static readonly AssetReference DialogBox = new AssetReference("Assets/Prefabs/UI/DialogBox.prefab");
        public static readonly AssetReference Menu = new AssetReference("Assets/Prefabs/UI/Menu.prefab");
    }
    
    public static void LoadThen<T>(AssetReference asset, Action<AsyncOperationHandle<T>> action) { asset.LoadAssetAsync<T>().Completed += action; }
    public static void Load<T>(AssetReference asset) { asset.LoadAssetAsync<T>(); }

    public static void InstantiateSafeThen(AssetReference asset, Action<AsyncOperationHandle<GameObject>> action)
    {
        LoadThen<GameObject>(asset, handle => { asset.InstantiateAsync().Completed += action; } );
    }

    public static void DropItem(Vector2 location, InventoryItem item)
    {
        InstantiateSafeThen(Assets.RawItem, handle =>
        {
            
            GameObject newObject = handle.Result;
            newObject.SetActive(false);
            PickupItem itemComponent = newObject.GetComponent<PickupItem>();
            newObject.transform.position = location;
            itemComponent.FromInventoryItem(item);
            newObject.SetActive(true);

        });
        
    }

    public static void QuitGame()
    {
        
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        
    }
    
}
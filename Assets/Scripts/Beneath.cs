using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Scripts
{
    public static class Beneath
    {

        public static class Assets
        {
            public static readonly AssetReference DialogBox = new AssetReference("Assets/Prefabs/UI/DialogBox.prefab");
            public static readonly AssetReference InventoryInterface = new AssetReference("Assets/Prefabs/UI/InventoryInterface.prefab");
            public static readonly AssetReference SansSprite = new AssetReference("Assets/Art/Sprites/sans.png");
        }
        
        public static void LoadThen<T>(AssetReference asset, Action<AsyncOperationHandle<T>> action) { asset.LoadAssetAsync<T>().Completed += action; }
        public static void Load<T>(AssetReference asset) { asset.LoadAssetAsync<T>(); }

        public static void InstantiateSafeThen(AssetReference asset, Action<AsyncOperationHandle<GameObject>> action)
        {
            LoadThen<GameObject>(asset, handle => { asset.InstantiateAsync().Completed += action; });
        }

    }
}

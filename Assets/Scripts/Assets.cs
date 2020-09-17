using UnityEngine;
using UnityEngine.AddressableAssets;

public static partial class Beneath
{
    public static class AssetReferences
    {
        public static readonly AssetReference Item = new AssetReference("Assets/Prefabs/Items/Item.prefab");
        public static readonly AssetReference PlayerCharacter = new AssetReference("Assets/Prefabs/Characters/BeneathPlayer.prefab");
        public static readonly AssetReference DialogBox = new AssetReference("Assets/Prefabs/UI/DialogBox.prefab");
        public static readonly AssetReference EscapeMenu = new AssetReference("Assets/Prefabs/UI/EscapeMenu.prefab");
        public static readonly AssetReference SaveMenu = new AssetReference("Assets/Prefabs/UI/SaveMenu.prefab");
        public static readonly AssetReference ResumeMenu = new AssetReference("Assets/Prefabs/UI/ResumeMenu.prefab");
        public static readonly AssetReference NullKeySprite = new AssetReference("Assets/Art/Sprites/NullKey.png");
    }
    
}
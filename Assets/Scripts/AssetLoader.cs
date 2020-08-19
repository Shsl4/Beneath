using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetLoader : MonoBehaviour
{

    public AssetReference[] preloadedGameObjects;
        
    void Start()
    {

        Beneath.Load<GameObject>(Beneath.Assets.DialogBox);
        Beneath.Load<GameObject>(Beneath.Assets.InventoryInterface);
        Beneath.Load<Sprite>(Beneath.Assets.SansSprite);

        foreach (var asset in preloadedGameObjects)
        {

            if (!asset.IsValid()) return;
            Beneath.Load<GameObject>(asset);
                
        }
            
    }
}
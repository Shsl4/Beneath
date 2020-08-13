using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts
{
    public class AssetLoader : MonoBehaviour
    {

        public AssetReference[] PreloadedGameObjects;
        
        void Start()
        {

            Beneath.Load<GameObject>(Beneath.Assets.DialogBox);
            Beneath.Load<GameObject>(Beneath.Assets.InventoryInterface);
            Beneath.Load<Sprite>(Beneath.Assets.SansSprite);

            foreach (var asset in PreloadedGameObjects)
            {

                if (!asset.IsValid()) return;
                Beneath.Load<GameObject>(asset);
                
            }
            
        }
    }
}

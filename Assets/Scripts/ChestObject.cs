using Assets.Scripts.Attributes;
using Assets.Scripts.Interfaces;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class ChestObject : MonoBehaviour, IInteractable, IInventory
    {

        private Inventory _chestInventory;
    
        public Inventory GetInventory()
        {
            return _chestInventory;
        }

        public void Interact(GameObject source)
        {
            if (source.GetComponent<ControllableCharacter>() != null)
            {
                Beneath.LoadThen<Sprite>(Beneath.Assets.SansSprite, handle =>
                {
                    _chestInventory.GetNextEmptySlot().SetItem(new InventoryItem("Sans", "Unknown", null,
                        handle.Result, ItemTypes.Unknown, 0, new ItemAttribute[0]));
                    
                    Beneath.InstantiateSafeThen(Beneath.Assets.InventoryInterface, instHandle =>
                    {
                        instHandle.Result.GetComponent<ChestView>().SetupSlots(_chestInventory);
                    });
                });
            }
        }
        
        void Start()
        {
            _chestInventory = new Inventory(10);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

    }
}

using Attributes;
using Interfaces;
using UI.Inventory;
using UnityEngine;

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

            ControllableCharacter character = source.GetComponent<ControllableCharacter>();
            
            Beneath.LoadThen<Sprite>(Beneath.Assets.SansSprite, handle =>
            {
                _chestInventory.GetNextEmptySlot().SetItem(new InventoryItem("Sans", "Unknown", null, ItemTypes.Unknown, 0, new Vector2(10, 10), new ItemAttribute[0]));
                    
                Beneath.InstantiateSafeThen(Beneath.Assets.InventoryInterface, instHandle =>
                {
                    
                    InventoryManager manager = instHandle.Result.GetComponent<InventoryManager>();
                    
                    character.OpenUserInterface(manager);

                });
                
            });
        }
    }
        
    void Start()
    {
        _chestInventory = new Inventory(10);
    }

}
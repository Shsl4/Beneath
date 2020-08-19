using System;
using Attributes;
using Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PickupItem : MonoBehaviour, IInteractable, IIdentifiable, IInventoryItem
{

    public String itemName;
    [TextArea]
    public String description;
    public ItemTypes type;
    public int value;
    public ItemAttribute[] Attributes;

    public SpriteRenderer Renderer => GetComponent<SpriteRenderer>();
    public BoxCollider2D Collider => GetComponent<BoxCollider2D>();
    
    public void Interact(GameObject source)
    {
        if (source.GetComponent<IInventory>() != null)
        {
            Inventory targetInventory = source.GetComponent<IInventory>().GetInventory();

            if (!targetInventory.IsFull())
            {
                targetInventory.GetNextEmptySlot().SetItem(ToInventoryItem());
                Destroy(gameObject);
            }
        }
    }

    public string GetIdentifiableName()
    {
        return itemName;
    }

    public void FromInventoryItem(InventoryItem item)
    {

        itemName = item.name;
        description = item.description;
        type = item.type;
        value = item.value;
        Attributes = item.Attributes;
        Collider.size = item.colliderSize;
        Renderer.sprite = item.sprite;

    }

    public InventoryItem ToInventoryItem()
    {
        return new InventoryItem(itemName, description, Renderer.sprite, type, value, Collider.size, Attributes);
    }
}
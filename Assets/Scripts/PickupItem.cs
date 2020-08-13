using System;
using Assets.Scripts.Attributes;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts
{
    public class PickupItem : MonoBehaviour, IInteractable, IInventoryItem, IIdentifiable
    {

        public String ItemName;
        public String ItemDescription;
        public AssetReference ItemAsset;
        public ItemTypes ItemType;
        public int ItemValue;
        public ItemAttribute[] ItemAttributes;

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

        public InventoryItem ToInventoryItem()
        {
            return new InventoryItem(GetIdentifiableName(), ItemDescription, ItemAsset, gameObject.GetComponent<SpriteRenderer>().sprite, ItemType, ItemValue, ItemAttributes);
        }

        public string GetIdentifiableName()
        {
            return ItemName;
        }
    }
}

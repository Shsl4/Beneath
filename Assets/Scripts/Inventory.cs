using System;
using Attributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public enum ItemTypes
{
        
    Unknown,
    Food,
    Weapon,
    Armor,
    Cosmetic
        
}
    
[Serializable]
public class ItemData
{

    public readonly int id;
    public String name;
    public String description;
    public AssetReference spriteAsset;
    public ItemTypes type;
    public int value;
    public Vector2 colliderSize;
    public ItemAttribute[] attributes;

    public Sprite sprite => _sprite;
    private Sprite _sprite;

    public ItemData(int itemID, string name, string description, AssetReference spriteAsset, ItemTypes type, int value, ItemAttribute[] attributes)
    {

        id = itemID;
        
        this.attributes = attributes ?? new ItemAttribute[0];

        if (type == ItemTypes.Weapon && GetAttribute<DamageAttribute>() == null)
        {
            throw new ArgumentException("Tried to create a weapon item with no damage attribute. Aborting.");
        }
        
        if (type == ItemTypes.Armor && GetAttribute<DefenseAttribute>() == null)
        {
            throw new ArgumentException("Tried to create an armor item with no defense attribute. Aborting.");
        }
        
        this.name = name;
        this.description = description;
        this.spriteAsset = spriteAsset;
        this.type = type;
        this.value = value;

        Beneath.LoadThen<Sprite>(spriteAsset, handle =>
        {
            _sprite = handle.Result;
            colliderSize = _sprite.rect.size;
        });

    }

    public string FormatDescription()
    {

        string result = "";
        result += description + " ";

        foreach (var attribute in attributes)
        {
            result += attribute.Format() + ", ";
        }

        result += "Value: $" + value + ".";
        return result;

    }

    public T GetAttribute<T>() where T : class
    {

        foreach (var attr in attributes)
        {
            if (attr is T attribute)
            {
                return attribute;
            }
        }

        return null;

    }

}
    
public class InventorySlot
{
        
    private ItemData _itemData;

    public ItemData GetItem() { return _itemData; }

    public bool HasItem() { return GetItem() != null; }

    public bool Clear()
    {

        if (_itemData == null) { return false;}
        _itemData = null;
        return true;

    }

    public bool SetItem(ItemData itemData)
    {

        if (_itemData != null) { return false;}
        _itemData = itemData;
        return true;

    }

}
    
public class Inventory
{
        
    private InventorySlot[] _slots;

    public Inventory(int slotAmount)
    {
        if(slotAmount > 0) _slots = new InventorySlot[slotAmount];

        for (int i = 0; i < slotAmount; i++)
        {
            _slots[i] = new InventorySlot();
        }
            
    }

    public bool IsFull()
    {

        foreach (var slot in _slots)
        {
            if (slot.GetItem() == null) { return false; }
        }
            
        return true;

    }

    public InventorySlot GetNextEmptySlot()
    {
            
        foreach (var slot in _slots)
        {
            if (slot.GetItem() == null) { return slot; }
        }
            
        return null;
            
    }

    public InventorySlot[] GetSlots() { return _slots; }

    public InventorySlot GetSlot(int slotIndex)
    {
            
        return _slots.Length > slotIndex ? (InventorySlot)_slots.GetValue(slotIndex) : null;
    }

    public bool ClearSlot(int slotIndex)
    {
        return GetSlot(slotIndex) != null && GetSlot(slotIndex).Clear();
    }

    public bool SetItemInSlot(int slotIndex, ItemData itemData)
    {
        return GetSlot(slotIndex) != null && GetSlot(slotIndex).SetItem(itemData);
    }

    public void ClearInventory()
    {
        foreach (var slot in _slots) { slot.Clear(); }
    }

}
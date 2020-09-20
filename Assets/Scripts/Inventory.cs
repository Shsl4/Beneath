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
    public string name;
    public string description;
    public AssetReference spriteAsset;
    public ItemTypes type;
    public int value;
    public ItemAttribute[] attributes;

    public Sprite Sprite { get; private set; }

    public ItemData(int itemID, string name, string description, AssetReference spriteAsset, ItemTypes type, int value, ItemAttribute[] attributes)
    {

        id = itemID;
        
        this.attributes = attributes ?? new ItemAttribute[0];

        if (type == ItemTypes.Weapon)
        {

            if (GetAttribute<DamageAttribute>() == null)
            {
                throw new ArgumentException("Tried to create a weapon item with no damage attribute. " + "(" + name + ")");
            }
            
            if (GetAttribute<DamageMultiplierAttribute>() != null)
            {
                throw new ArgumentException("Only armors may have a damage multiplier attribute. " + "(" + name + ")");
            }
            
            if (GetAttribute<HealthMultiplierAttribute>() != null)
            {
                throw new ArgumentException("Only armors may have a health multiplier attribute. " + "(" + name + ")");
            }
            
            if (GetAttribute<DefenseAttribute>() != null)
            {
                throw new ArgumentException("Only armors may have a defense attribute. " + "(" + name + ")");
            }
            
        }
        
        if (type == ItemTypes.Armor)
        {

            if (GetAttribute<DefenseAttribute>() == null)
            {
                throw new ArgumentException("Tried to create an armor item with no defense attribute. " + "(" + name + ")");
            }
            
            if (GetAttribute<DamageAttribute>() != null)
            {
                throw new ArgumentException("Only weapons may have a damage attribute. " + "(" + name + ")");
            }
            
        }
        
        this.name = name;
        this.description = description;
        this.spriteAsset = spriteAsset;
        this.type = type;
        this.value = value;

        Beneath.LoadThen<Sprite>(spriteAsset, handle =>
        {
            Sprite = handle.Result;
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
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
public class InventoryItem
{

    public String name;
    public String description;
    public Sprite sprite;
    public ItemTypes type;
    public int value;
    public Vector2 colliderSize;
    public ItemAttribute[] Attributes;

    public InventoryItem(string name, string description, Sprite sprite, ItemTypes type, int value, Vector2 colliderSize, ItemAttribute[] attributes)
    {
        this.name = name;
        this.description = description;
        this.sprite = sprite;
        this.type = type;
        this.value = value;
        this.colliderSize = colliderSize;
        Attributes = attributes ?? new ItemAttribute[0];
    }

    public string FormatDescription()
    {

        string result = "";
        result += description + " ";

        foreach (var attribute in Attributes)
        {
            result += attribute.Format() + ", ";
        }

        result += "Value: $" + value + ".";
        return result;

    }
    
}
    
public class InventorySlot
{
        
    private InventoryItem _item;

    public InventoryItem GetItem() { return _item; }

    public bool Clear()
    {

        if (_item == null) { return false;}
        _item = null;
        return true;

    }

    public bool SetItem(InventoryItem item)
    {

        if (_item != null) { return false;}
        _item = item;
        return true;

    }

    public InventorySlot() {}

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

    public bool SetItemInSlot(int slotIndex, InventoryItem item)
    {
        return GetSlot(slotIndex) != null && GetSlot(slotIndex).SetItem(item);
    }

    public void ClearInventory()
    {
        foreach (var slot in _slots) { slot.Clear(); }
    }

}
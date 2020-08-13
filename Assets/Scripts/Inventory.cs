using System;
using Assets.Scripts.Attributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts
{

    public enum ItemTypes
    {
        
        Unknown,
        Food,
        Weapon,
        Armor,
        Cosmetic
        
    }
    
    public class InventoryItem
    {

        public readonly String Name;
        public readonly String Description;
        public readonly AssetReference Asset;
        public readonly Sprite Sprite;
        public readonly ItemTypes Type;
        public readonly int Value;
        public readonly ItemAttribute[] Attributes;

        public InventoryItem(string name, string description, AssetReference asset, Sprite sprite, ItemTypes type, int value, ItemAttribute[] attributes)
        {
            Name = name;
            Description = description;
            Asset = asset;
            Sprite = sprite;
            Type = type;
            Value = value;
            Attributes = attributes;
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

        public InventorySlot GetSlot(int slotIndex) { return (InventorySlot)_slots.GetValue(slotIndex); }

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
}

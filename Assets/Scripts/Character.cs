using System;
using Attributes;
using Events;
using Interfaces;
using Unity.Mathematics;
using UnityEngine;

public class Character : DamageableObject, IIdentifiable, IInventory
{

    public String characterName;
    public static int InventorySlotsAmount = 8;

    public InventorySlot CharacterArmor { get; private set; } = new InventorySlot();
    public InventorySlot CharacterWeapon { get; private set; } = new InventorySlot();
        
    protected Rigidbody2D RigidBody;
    private Inventory _characterInventory;
        
    public virtual void Start()
    {
            
        RigidBody = GetComponent<Rigidbody2D>();
        _characterInventory = new Inventory(InventorySlotsAmount);
        CurrentHealth = maxHealth;
            
    }

    public virtual void Update() { }
        
    protected override void DestructionEvent() { Destroy(gameObject); }

    protected override void DamageReceived(DamageEvent damageEvent) { }
    protected override void HealReceived(HealEvent healEvent) { }

    public string GetIdentifiableName() { return characterName; }
    public Inventory GetInventory() { return _characterInventory; }

    public bool DropItemFromSlot(int index)
    {

        if (_characterInventory.GetSlot(index) != null)
        {
            Beneath.DropItem(RigidBody.position, _characterInventory.GetSlot(index).GetItem());
            _characterInventory.GetSlot(index).Clear();
            return true;
        }
        
        return false;

    }

    public void ClearItemFromSlot(int index)
    {
        
        if (_characterInventory.GetSlot(index) != null)
        {
            _characterInventory.GetSlot(index).Clear();
        }
    }
    
    public Beneath.EquipResult EquipWeapon(int index)
    {

        if (GetInventory().GetSlot(index).GetItem() != null && GetInventory().GetSlot(index).GetItem().type == ItemTypes.Weapon)
        {

            if (CharacterWeapon.GetItem() == null)
            {
                CharacterWeapon.SetItem(GetInventory().GetSlot(index).GetItem());
                ClearItemFromSlot(index);
                return Beneath.EquipResult.Success;
            }

            return Beneath.EquipResult.AlreadyEquipped;

        }

        return Beneath.EquipResult.Error;
    }    
    
    public Beneath.EquipResult EquipArmor(int index)
    {

        if (GetInventory().GetSlot(index).GetItem() != null && GetInventory().GetSlot(index).GetItem().type == ItemTypes.Armor)
        {

            if (CharacterArmor.GetItem() == null)
            {
                CharacterArmor.SetItem(GetInventory().GetSlot(index).GetItem());
                ClearItemFromSlot(index);
                return Beneath.EquipResult.Success;
            }

            return Beneath.EquipResult.AlreadyEquipped;

        }

        return Beneath.EquipResult.Error;
    }

    public Beneath.UnEquipResult UnEquipArmor(bool discard)
    {

        if (CharacterArmor.GetItem() == null) { return Beneath.UnEquipResult.Error; }

        if (discard)
        {
            Beneath.DropItem(RigidBody.position, CharacterArmor.GetItem());
            CharacterArmor.Clear();
            return Beneath.UnEquipResult.Success;
        }

        if (_characterInventory.IsFull())
        {
            return Beneath.UnEquipResult.InventoryFull;
        }

        _characterInventory.GetNextEmptySlot().SetItem(CharacterArmor.GetItem());
        CharacterArmor.Clear();
        return Beneath.UnEquipResult.Success;

    }
    
    public Beneath.UnEquipResult UnEquipWeapon(bool discard)
    {

        if (CharacterWeapon.GetItem() == null) { return Beneath.UnEquipResult.Error; }

        if (discard)
        {
            Beneath.DropItem(RigidBody.position, CharacterWeapon.GetItem());
            CharacterWeapon.Clear();
            return Beneath.UnEquipResult.Success;
        }

        if (_characterInventory.IsFull())
        {
            return Beneath.UnEquipResult.InventoryFull;
        }

        _characterInventory.GetNextEmptySlot().SetItem(CharacterWeapon.GetItem());
        CharacterWeapon.Clear();
        return Beneath.UnEquipResult.Success;

    }

    public int GetCharacterDamage()
    {

        int characterDamage = 5;
        
        if (CharacterWeapon.GetItem() != null)
        {
            foreach (var attribute in CharacterWeapon.GetItem().Attributes)
            {
                if (attribute is DamageAttribute damageAttribute)
                {
                    characterDamage = damageAttribute.DamageAmount;
                }
            }
        }        
        
        if (CharacterArmor.GetItem() != null)
        {
            foreach (var attribute in CharacterArmor.GetItem().Attributes)
            {
                if (attribute is DamageModifierAttribute modifier)
                {
                    if (modifier.multiplier > 0.0f)
                    {
                        characterDamage = Mathf.RoundToInt(characterDamage * modifier.multiplier);
                    }
                }
            }
        }

        return characterDamage;

    }

}
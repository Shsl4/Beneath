using System;
using Assets.Scripts.Events;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class Character : DamageableObject, IIdentifiable, IInventory
    {

        public String CharacterName;
        public bool HasInventory = false;
        public int InventorySlotsAmount = 8;
        
        protected Rigidbody2D RigidBody;
        protected Inventory CharacterInventory;
        
        public virtual void Start()
        {
            
            RigidBody = GetComponent<Rigidbody2D>();
            if (HasInventory) { CharacterInventory = new Inventory(InventorySlotsAmount); }
            
            CurrentHealth = MaxHealth;
            
        }

        public virtual void Update() { }
        
        protected override void DestructionEvent() { Destroy(gameObject); }

        protected override void DamageReceived(DamageEvent damageEvent) { }
        protected override void HealReceived(HealEvent healEvent) { }

        public string GetIdentifiableName() { return CharacterName; }
        public Inventory GetInventory() { return CharacterInventory; }
    }
}

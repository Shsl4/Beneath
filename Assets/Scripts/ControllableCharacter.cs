﻿using Assets.Scripts.Interfaces;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ControllableCharacter : Character
    {
        public float CharacterSpeed = 1.0f;
        public float CharacterRange = 1.5f;
        
        protected float HorizontalInput;
        protected float VerticalInput;
        
        private Vector2 _lookDirection = new Vector2(1,0);
        private InventoryManager _inventoryManager;

        private IUserInterface _activeInterface;

        public bool HasActiveInterface() { return _activeInterface != null; }

        public override void Update() 
        {
        
            base.Update();

            Vector2 move = new Vector2(HorizontalInput, VerticalInput);
            
            if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                _lookDirection.Set(move.x, move.y);
                _lookDirection.Normalize();
            }

        }

        public void OnInteract()
        {
            
            Vector2 start = RigidBody.position + Vector2.up * 0.2f;
            RaycastHit2D hit = Physics2D.Raycast(start, _lookDirection, CharacterRange, LayerMask.GetMask("Interaction"));
            
            Debug.DrawRay(new Vector3(start.x, start.y, 0), new Vector3(_lookDirection.x, _lookDirection.y, 0) * CharacterRange, Color.red, 2.0f);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<IInteractable>() != null)
            {
                IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                interactable.Interact(gameObject);
            }
            
        }

        public void OnFire()
        {
            
            Vector2 start = RigidBody.position /*+ Vector2.up * 0.2f*/;
            RaycastHit2D[] hits = Physics2D.RaycastAll(start, _lookDirection, CharacterRange);
            Debug.DrawRay(new Vector3(start.x, start.y, 0), new Vector3(_lookDirection.x, _lookDirection.y, 0) * CharacterRange, Color.green, 2.0f);
            
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.Equals(gameObject)) { }
                else
                {
                    if (hit.collider.gameObject.GetComponent<DamageableObject>() != null)
                    {
                        DamageableObject damageable = hit.collider.gameObject.GetComponent<DamageableObject>();
                        damageable.Damage(GetCharacterDamage(), gameObject);
                        return;
                    }
                }
            }
            
        }
        
        public void OnMove(InputValue value)
        {

            Vector2 val = value.Get<Vector2>();
            HorizontalInput = val.x;
            VerticalInput = val.y;

        }

        void FixedUpdate()
        {
            
            Vector2 position = RigidBody.position;
            position.x += 10.0f * HorizontalInput * Time.deltaTime * CharacterSpeed;
            position.y += 10.0f * VerticalInput * Time.deltaTime * CharacterSpeed;
            RigidBody.MovePosition(position);
            
        }

        bool DropInventoryItem(int slotIndex)
        {
            if (GetInventory().GetSlot(slotIndex) != null)
            {
                if (GetInventory().GetSlot(slotIndex).GetItem() != null)
                {
                    Vector3 spawnLocation = RigidBody.position + _lookDirection;
                    
                    Beneath.InstantiateSafeThen(GetInventory().GetSlot(slotIndex).GetItem().Asset, handle =>
                    {
                        GameObject newObject = handle.Result;
                        newObject.transform.position = spawnLocation;
                        GetInventory().ClearSlot(slotIndex);
                    });
                    return true;
                }
            }
            return false;
        }

        public void OnInventory()
        {
            if (_inventoryManager == null)
            {
                Beneath.LoadThen<Sprite>(Beneath.Assets.SansSprite, handle =>
                {
                    Beneath.InstantiateSafeThen(Beneath.Assets.InventoryInterface, instHandle =>
                    {
                         _inventoryManager = instHandle.Result.GetComponent<InventoryManager>();
                         _inventoryManager.SetupSlots(CharacterInventory);
                    });
                });
            }
            else
            {
                if (_inventoryManager.gameObject.activeSelf)
                {
                    _inventoryManager.gameObject.SetActive(false);
                }
                else
                {
                    _inventoryManager.SetupSlots(CharacterInventory);
                    _inventoryManager.gameObject.transform.root.gameObject.SetActive(true);
                }
            }
        }
        
        private int GetCharacterDamage() { return 3; }
        
    }
}

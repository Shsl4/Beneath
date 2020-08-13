using System;
using Assets.Scripts.Events;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class DamageableObject : MonoBehaviour, IDamageable
    {

        protected int CurrentHealth;
        
        public int MaxHealth = 10;
        public bool IsDestroyed() { return GetHealth() <= 0; }
        public int GetHealth() { return CurrentHealth; }
        public int GetMaxHealth() { return MaxHealth; }
        
        public int Damage(int damageAmount, GameObject source)
        {
            
            if (damageAmount <= 0 || CurrentHealth <= 0) { return 0; }
            
            DamageEvent damageEvent = new DamageEvent(damageAmount, source);
            DamageReceived(damageEvent);
            
            if (damageEvent.Cancelled) { return 0; }
            
            String sourceName = source.name;
            String targetName = gameObject.name;
            
            if (source.GetComponent<IIdentifiable>() != null)
            {
                sourceName = source.GetComponent<IIdentifiable>().GetIdentifiableName();
            }
            
            if (gameObject.GetComponent<IIdentifiable>() != null)
            {
                targetName = gameObject.GetComponent<IIdentifiable>().GetIdentifiableName();
            }
            
            if (CurrentHealth - damageAmount < 0)
            {
                CurrentHealth = 0;
                DestructionEvent();
                Debug.Log(sourceName + " applied " + damageAmount + " damage to " + targetName);
                return damageAmount - CurrentHealth;
            }
            
            CurrentHealth -= damageAmount;
            Debug.Log(sourceName + " applied " + damageAmount + " damage to " + targetName);
            return damageAmount;

        }
        
        public int Heal(int healAmount, GameObject source)
        {
            if (healAmount <= 0 || CurrentHealth >= MaxHealth) { return 0; }

            HealEvent healEvent = new HealEvent(healAmount, source);
            HealReceived(healEvent);    
            
            if (healEvent.Cancelled) { return 0; }
            
            String sourceName = source.name;
            String targetName = gameObject.name;
            
            if (source.GetComponent<IIdentifiable>() != null)
            {
                sourceName = source.GetComponent<IIdentifiable>().GetIdentifiableName();
            }
            
            if (gameObject.GetComponent<IIdentifiable>() != null)
            {
                targetName = gameObject.GetComponent<IIdentifiable>().GetIdentifiableName();
            }
            
            if (CurrentHealth + healAmount > MaxHealth)
            {
                CurrentHealth = MaxHealth;
                Debug.Log(sourceName + " healed " + healEvent.HealAmount + " LP to " + targetName);
                return healAmount - CurrentHealth;
            }

            CurrentHealth += healAmount;
            Debug.Log(sourceName + " healed " + healEvent.HealAmount + " LP to " + targetName);
            return healAmount;
            
        }
        
        protected abstract void DestructionEvent();
        protected abstract void DamageReceived(DamageEvent damageEvent);
        protected abstract void HealReceived(HealEvent healEvent);
        
    }
}

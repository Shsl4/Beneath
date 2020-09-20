using UnityEngine;

namespace Events
{
    public class DamageEvent : CancellableEvent
    {
        
        public readonly int damageAmount;
        public readonly GameObject damageSource;

        public DamageEvent(int amount, GameObject source)
        {

            damageAmount = amount;
            damageSource = source;

        }
    }
}

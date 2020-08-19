using UnityEngine;

namespace Events
{
    public class DamageEvent : CancellableEvent
    {
        
        public readonly int DamageAmount;
        public readonly GameObject DamageSource;

        public DamageEvent(int amount, GameObject source)
        {

            DamageAmount = amount;
            DamageSource = source;

        }
    }
}

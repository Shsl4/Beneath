using UnityEngine;

namespace Events
{
    public class HealEvent : CancellableEvent
    {
        
        public readonly int healAmount;
        public readonly GameObject healSource;

        public HealEvent(int amount, GameObject source)
        {

            healAmount = amount;
            healSource = source;

        }
    }
}

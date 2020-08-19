using UnityEngine;

namespace Events
{
    public class HealEvent : CancellableEvent
    {
        
        public readonly int HealAmount;
        public readonly GameObject HealSource;

        public HealEvent(int amount, GameObject source)
        {

            HealAmount = amount;
            HealSource = source;

        }
    }
}

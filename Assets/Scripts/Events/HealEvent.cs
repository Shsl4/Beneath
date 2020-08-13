using UnityEngine;

namespace Assets.Scripts.Events
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

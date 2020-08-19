using System;

namespace Attributes
{
    [Serializable]
    public class HealthModifierAttribute : ItemAttribute
    {

        public float multiplier;

        public HealthModifierAttribute(float multiplier) : base("Health Modifier")
        {
            this.multiplier = multiplier;
        }
        
        public override string Format()
        {
            return ("HP: " + (multiplier - 1) * 100 + "%");
        }
    }
}

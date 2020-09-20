using System;

namespace Attributes
{
    [Serializable]
    public class HealthMultiplierAttribute : ItemAttribute
    {

        public float multiplier;

        public HealthMultiplierAttribute(float multiplier) : base("Health Multiplier")
        {
            this.multiplier = multiplier;
        }
        
        public override string Format()
        {
            return ("HP: " + (multiplier - 1) * 100 + "%");
        }
    }
}

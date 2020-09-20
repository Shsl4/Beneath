using System;

namespace Attributes
{
    [Serializable]
    public class DamageMultiplierAttribute : ItemAttribute
    {
        public float multiplier;

        public DamageMultiplierAttribute(float multiplier) : base("Damage Multiplier")
        {
            this.multiplier = multiplier;
        }
        
        public override string Format()
        {
            return ("DMG: " + (multiplier - 1) * 100 + "%");
        }
    }
}

using System;

namespace Attributes
{
    [Serializable]
    public class DamageAttribute : ItemAttribute
    {
        public readonly int damageAmount;

        public DamageAttribute(int damageAmount) : base("Damage")
        {
            this.damageAmount = damageAmount;
        }

        public override string Format()
        {
            return ("DMG: " + damageAmount);
        }
    }
}

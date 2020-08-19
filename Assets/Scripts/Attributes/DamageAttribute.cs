using System;

namespace Attributes
{
    [Serializable]
    public class DamageAttribute : ItemAttribute
    {
        public readonly int DamageAmount;

        public DamageAttribute(int damageAmount) : base("Damage")
        {
            DamageAmount = damageAmount;
        }

        public override string Format()
        {
            return ("DMG: " + DamageAmount);
        }
    }
}

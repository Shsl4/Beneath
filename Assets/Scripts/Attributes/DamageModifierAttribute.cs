using System;
using UnityEngine;

namespace Attributes
{
    [Serializable]
    public class DamageModifierAttribute : ItemAttribute
    {
        public float multiplier;

        public DamageModifierAttribute(float multiplier) : base("Damage Modifier")
        {
            this.multiplier = multiplier;
        }
        
        public override string Format()
        {
            return ("DMG: " + (multiplier - 1) * 100 + "%");
        }
    }
}

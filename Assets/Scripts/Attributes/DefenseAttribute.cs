using System;

namespace Attributes
{
    [Serializable]
    public class DefenseAttribute : ItemAttribute
    {
        public readonly int defenseAmount;

        public DefenseAttribute(int defenseAmount) : base("Defense")
        {
            this.defenseAmount = defenseAmount;
        }
        
        public override string Format()
        {
            return ("DEF: " + defenseAmount);
        }
    
    }
}

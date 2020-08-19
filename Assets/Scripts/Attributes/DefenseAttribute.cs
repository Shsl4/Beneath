using System;

namespace Attributes
{
    [Serializable]
    public class DefenseAttribute : ItemAttribute
    {
        public readonly int DefenseAmount;

        public DefenseAttribute(int defenseAmount) : base("Defense")
        {
            DefenseAmount = defenseAmount;
        }
        
        public override string Format()
        {
            return ("DEF: " + DefenseAmount);
        }
    
    }
}

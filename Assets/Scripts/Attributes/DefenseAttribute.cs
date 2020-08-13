namespace Assets.Scripts.Attributes
{
    public class DefenseAttribute : ItemAttribute
    {
        public readonly int DefenseAmount;

        public DefenseAttribute(int defenseAmount) : base("Defense")
        {
            DefenseAmount = defenseAmount;
        }
    
    }
}

namespace Assets.Scripts.Attributes
{
    public class DamageAttribute : ItemAttribute
    {
        public readonly int DamageAmount;

        public DamageAttribute(int damageAmount) : base("Damage")
        {
            DamageAmount = damageAmount;
        }
    }
}

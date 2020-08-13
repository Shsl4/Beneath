using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IDamageable
    {
        bool IsDestroyed();
        int GetHealth();
        int GetMaxHealth();

        int Damage(int damageAmount, GameObject source);
        int Heal(int healAmount, GameObject source);

    }
}

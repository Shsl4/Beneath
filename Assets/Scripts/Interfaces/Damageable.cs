using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {

        bool CanBeDamaged();
        
        int GetHealth();
        int GetMaxHealth();

        int Damage(int damageAmount, GameObject source);
        int Heal(int healAmount, GameObject source);

    }
}

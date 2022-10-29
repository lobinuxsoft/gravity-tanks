using UnityEngine;

namespace HNW
{
    public static class Damager
    {
        public static void DamageTo(GameObject go, int damageAmount)
        {
            if(go.TryGetComponent(out Damageable damageable))
            {
                damageable.SetDamage(damageAmount);
            }
        }
    }
}
using UnityEngine;

namespace HNW
{
    public class Damager : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 1;

        public int DamageAmount
        {
            get => damageAmount;
            set => damageAmount = value;
        }

        public void DamageTo(GameObject go)
        {
            if(go.TryGetComponent(out Damageable damageable))
            {
                damageable.SetDamage(damageAmount);
            }
        }
    }
}
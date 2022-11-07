using UnityEngine;
using UnityEngine.Events;

namespace HNW
{
    public class EnemyDamageControl : Damageable
    {
        [SerializeField] private int health = 5;
        [SerializeField] private int maxHealth = 5;

        public UnityEvent<int> onHealthChanged;
        public UnityEvent<int> onMaxHealthChanged;

        public override int Health
        {
            get => health;
            set
            {
                health = value;

                if (health <= 0)
                    onDie?.Invoke(this.gameObject);
                else
                    onHealthChanged?.Invoke(health);
            }
        }

        public override int MaxHealth
        {
            get => maxHealth;
            set
            {
                maxHealth = value;
                onMaxHealthChanged?.Invoke(maxHealth);
            }
        }
    }
}
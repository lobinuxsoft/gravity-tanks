using UnityEngine;
using UnityEngine.Events;

namespace GravityTanks
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private int health = 5;
        [SerializeField] private int maxHealth = 5;
        [SerializeField] GameObject deathEffect;

        public UnityEvent<int> onHealthChanged;
        public UnityEvent<int> onMaxHealthChanged;
        public UnityEvent onDie;

        public int Health
        {
            get => health;
            set
            {
                health = value;

                if (health <= 0)
                {
                    if(deathEffect != null)
                        Instantiate(deathEffect, transform.position, transform.rotation);

                    onDie?.Invoke();
                }
                else
                    onHealthChanged?.Invoke(health);
            }
        }

        public int MaxHealth
        {
            get => maxHealth;
            set
            {
                maxHealth = value;
                onMaxHealthChanged?.Invoke(maxHealth);
            }
        }

        public void SetDamage(int value) => Health -= value;

        public void FullHeal() => Health = MaxHealth;
    }
}
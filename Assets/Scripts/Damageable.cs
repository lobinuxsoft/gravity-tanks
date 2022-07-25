using UnityEngine;
using UnityEngine.Events;

namespace GravityTanks
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private int health = 5;
        [SerializeField] private int maxHealth = 5;

        public UnityAction<int> onHealthChanged;
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
    }
}
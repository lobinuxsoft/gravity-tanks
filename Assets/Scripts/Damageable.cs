using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace HNW
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private int health = 5;
        [SerializeField] private int maxHealth = 5;
        [SerializeField, GradientUsage(true)] Gradient damageGradient;
        [SerializeField] GameObject deathEffect;

        public UnityEvent<int> onHealthChanged;
        public UnityEvent<int> onMaxHealthChanged;
        public UnityEvent onDie;

        Renderer[] renderers;

        private void Awake() => renderers = GetComponentsInChildren<Renderer>();

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

                    StopAllCoroutines();
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

        public void SetDamage(int value)
        {
            if(isActiveAndEnabled)
                StartCoroutine(BlinkEffect());

            Health -= value;
        }

        public void FullHeal() => Health = MaxHealth;

        IEnumerator BlinkEffect(float duration = 1)
        {
            float lerp = 0;
            float blinkSpeed = 4;

            while(lerp < duration)
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material.color = damageGradient.Evaluate(Mathf.PingPong(lerp * blinkSpeed, 1));
                }

                lerp += Time.deltaTime;

                yield return null;
            }
        }
    }
}
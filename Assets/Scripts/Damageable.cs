using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace HNW
{
    public abstract class Damageable : MonoBehaviour
    {
        [SerializeField, GradientUsage(true)] Gradient damageGradient;
        [SerializeField] GameObject deathEffect;

        public UnityEvent<GameObject> onDie;

        protected Renderer[] renderers;

        public virtual int Health { get; set; }
        public virtual int MaxHealth { get; set; }

        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
            onDie.AddListener(ExplodeEffect);
            FullHeal();
        }

        private void OnDestroy()
        {
            onDie.RemoveListener(ExplodeEffect);
        }

        private void ExplodeEffect(GameObject obj)
        {
            if (deathEffect != null)
                Instantiate(deathEffect, obj.transform.position, obj.transform.rotation);

            StopAllCoroutines();

            #if UNITY_ANDROID
            Handheld.Vibrate();
            #endif
        }

        public virtual void SetDamage(int value)
        {
            if (isActiveAndEnabled)
                StartCoroutine(BlinkEffect());

            Health -= value;
        }

        public void FullHeal()
        {
            Health = MaxHealth;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = damageGradient.Evaluate(0);
            }
        }

        protected IEnumerator BlinkEffect(float duration = 1)
        {
            float lerp = 0;
            float blinkSpeed = 4;

            while (lerp < duration)
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
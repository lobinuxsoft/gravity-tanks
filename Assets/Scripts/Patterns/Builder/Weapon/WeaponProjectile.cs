using CryingOnionTools.AudioTools;
using System;
using System.Collections;
using UnityEngine;

namespace HNW
{
    [RequireComponent(typeof(SFXTrigger))]
    public class WeaponProjectile : MonoBehaviour
    {
        [SerializeField] private AudioClip shotSfx;
        [SerializeField] private AudioClip impactSfx;

        private LayerMask layerToCollide;
        private int emmitAmount = 1;
        private float shotRate = .5f;
        private int shotAngle = 0;

        private ParticleSystem particle;
        private ParticleSystem.EmissionModule emissionModule;
        private ParticleSystem.CollisionModule collisionModule;
        private ParticleSystem.ShapeModule shapeModule;

        private SFXTrigger sfxTrigger;

        private Coroutine shootRoutine = null;

        public event Action<GameObject> OnProjectileHit;

        public int EmmitAmount
        {
            get => emmitAmount;
            set
            {
                emmitAmount = value;
                
                if (emissionModule.enabled)
                {
                    ParticleSystem.Burst burst = new ParticleSystem.Burst(0, emmitAmount);
                    emissionModule.SetBursts(new ParticleSystem.Burst[] { burst });
                }
            }
        }

        public int ShotAngle
        {
            get => shotAngle;
            set
            {
                shotAngle = Mathf.Clamp(value, 0, 360);

                if (shapeModule.enabled)
                {
                    shapeModule.radius = 0;
                    shapeModule.radiusThickness = 0;
                    shapeModule.arcMode = ParticleSystemShapeMultiModeValue.BurstSpread;
                    shapeModule.arcSpread = 0;
                    shapeModule.shapeType = ParticleSystemShapeType.Circle;

                    shapeModule.arc = shotAngle;
                    shapeModule.rotation = new Vector3(90f, -90f + (shapeModule.arc / 2), 0f);
                }
            }
        }

        public float ShotRate
        {
            get => shotRate;
            set => shotRate = value;
        }

        public LayerMask LayerToCollide
        {
            get => layerToCollide;
            set
            {
                layerToCollide = value;

                if (collisionModule.enabled)
                {
                    collisionModule.collidesWith = layerToCollide;
                    collisionModule.sendCollisionMessages = true;
                }
            }
        }

        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();

            emissionModule = particle.emission;
            collisionModule = particle.collision;
            shapeModule = particle.shape;

            EmmitAmount = emmitAmount;
            LayerToCollide = layerToCollide;
            ShotAngle = emmitAmount;

            sfxTrigger = GetComponent<SFXTrigger>();
        }

        private void OnDisable()
        {
            if (shootRoutine != null)
            {
                StopAllCoroutines();
                shootRoutine = null;
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            sfxTrigger.PlaySFX(impactSfx);
            OnProjectileHit?.Invoke(other);
        }

        public void Shoot()
        {
            if (shootRoutine == null)
                shootRoutine = StartCoroutine(ShootRoutine(shotRate));
        }

        IEnumerator ShootRoutine(float delay)
        {
            particle.Play();
            sfxTrigger.PlaySFX(shotSfx);
            yield return new WaitForSeconds(delay);
            shootRoutine = null;
        }
    }
}
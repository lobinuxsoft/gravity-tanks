using UnityEngine;
using CryingOnionTools.AudioTools;
using System.Collections;

namespace HNW
{
    [RequireComponent(typeof(SFXTrigger))]
    public class CannonControl : MonoBehaviour
    {
        [SerializeField] int emmitAmount = 1;
        [SerializeField] float arcShootAngle = 45;
        [SerializeField] float shootRate = .5f;
        [SerializeField] LayerMask layerToCollide;
        [SerializeField] AudioClip shootSfx;
        [SerializeField] AudioClip impactSfx;

        ParticleSystem particle;
        ParticleSystem.CollisionModule collisionModule;
        ParticleSystem.ShapeModule shapeModule;

        SFXTrigger sfxTrigger;

        Coroutine shootRoutine = null;

        public int EmmitAmount
        {
            get => emmitAmount;
            set => emmitAmount = value;
        }

        public float ArcShootAngle
        {
            get => arcShootAngle;
            set
            {
                arcShootAngle = value;
                shapeModule.arc = arcShootAngle;
                shapeModule.rotation = new Vector3(90f, -90f + (shapeModule.arc / 2), 0f);
            }
        }

        public float ShootRate
        {
            get => shootRate;
            set => shootRate = value;
        }

        private void Awake() 
        {
            particle = GetComponent<ParticleSystem>();
            collisionModule = particle.collision;
            collisionModule.collidesWith = layerToCollide;
            collisionModule.sendCollisionMessages = true;

            shapeModule = particle.shape;
            shapeModule.arc = arcShootAngle;
            shapeModule.radius = 0;
            shapeModule.radiusThickness = 0;
            shapeModule.arcMode = ParticleSystemShapeMultiModeValue.BurstSpread;
            shapeModule.arcSpread = 0;
            shapeModule.shapeType = ParticleSystemShapeType.Circle;
            shapeModule.rotation = new Vector3(90f, -90f + (shapeModule.arc / 2), 0f);

            sfxTrigger = GetComponent<SFXTrigger>();
        }

        private void OnEnable()
        {
            if (shootRoutine != null)
            {
                StopAllCoroutines();
                shootRoutine = null;
            }
        }

        public void Shoot() 
        {
            if (shootRoutine == null)
                shootRoutine = StartCoroutine(ShotRoutine(shootRate));
        }

        IEnumerator ShotRoutine(float delay)
        {
            sfxTrigger.PlaySFX(shootSfx);
            particle.Emit(emmitAmount);
            yield return new WaitForSeconds(delay);
            shootRoutine = null;
        }

        void OnParticleCollision(GameObject other) 
        {
            sfxTrigger.PlaySFX(impactSfx);
            Damager.DamageTo(other, 1);
        }
    }
}
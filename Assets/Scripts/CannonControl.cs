using UnityEngine;
using CryingOnionTools.AudioTools;
using System.Collections;

namespace GravityTanks
{
    [RequireComponent(typeof(Damager), typeof(SFXTrigger))]
    public class CannonControl : MonoBehaviour
    {
        [SerializeField] int emmitAmount = 1;
        [SerializeField] float arcShootAngle = 45;
        [SerializeField] LayerMask layerToCollide;
        [SerializeField] AudioClip shootSfx;
        [SerializeField] AudioClip impactSfx;
        [SerializeField] float shotRate = .5f;

        Damager damager;

        ParticleSystem particle;
        ParticleSystem.CollisionModule collisionModule;
        ParticleSystem.ShapeModule shapeModule;

        SFXTrigger sfxTrigger;

        Coroutine shotRoutine = null;

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

            damager = GetComponent<Damager>();
            sfxTrigger = GetComponent<SFXTrigger>();
        }

        public void Shoot() 
        {
            if (shotRoutine == null)
                shotRoutine = StartCoroutine(ShotRoutine(shotRate));
        }

        IEnumerator ShotRoutine(float delay)
        {
            sfxTrigger.PlaySFX(shootSfx);
            particle.Emit(emmitAmount);
            yield return new WaitForSeconds(delay);
            shotRoutine = null;
        }

        void OnParticleCollision(GameObject other) 
        {
            sfxTrigger.PlaySFX(impactSfx);
            damager.DamageTo(other);
        }
    }
}
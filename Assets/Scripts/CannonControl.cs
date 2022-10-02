using UnityEngine;
using CryingOnionTools.AudioTools;
using System.Collections;

namespace GravityTanks
{
    [RequireComponent(typeof(Damager), typeof(SFXTrigger))]
    public class CannonControl : MonoBehaviour
    {
        [SerializeField] LayerMask layerToCollide;
        [SerializeField] AudioClip shootSfx;
        [SerializeField] AudioClip impactSfx;
        [SerializeField] float shotRate = .5f;

        Damager damager;

        ParticleSystem particle;
        ParticleSystem.CollisionModule collisionModule;

        SFXTrigger sfxTrigger;

        Coroutine shotRoutine = null;

        private void Awake() 
        {
            particle = GetComponent<ParticleSystem>();
            collisionModule = particle.collision;
            collisionModule.collidesWith = layerToCollide;
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
            particle.Emit(1);
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
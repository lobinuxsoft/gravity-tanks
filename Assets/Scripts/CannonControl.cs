using UnityEngine;
using CryingOnionTools.AudioTools;

namespace GravityTanks
{
    [RequireComponent(typeof(Damager), typeof(SFXTrigger))]
    public class CannonControl : MonoBehaviour
    {
        [SerializeField] AudioClip shootSfx;
        [SerializeField] AudioClip impactSfx;

        Damager damager;

        ParticleSystem particle;

        SFXTrigger sfxTrigger;

        private void Awake() 
        {
            particle = GetComponent<ParticleSystem>();
            damager = GetComponent<Damager>();
            sfxTrigger = GetComponent<SFXTrigger>();
        }

        public void Shoot() 
        {
            sfxTrigger.PlaySFX(shootSfx);
            particle.Emit(1); 
        }

        void OnParticleCollision(GameObject other) 
        {
            sfxTrigger.PlaySFX(impactSfx);
            damager.DamageTo(other);
        }
    }
}
using UnityEngine;

namespace GravityTanks
{
    [RequireComponent(typeof(Damager))]
    public class CannonControl : MonoBehaviour
    {
        Damager damager;

        ParticleSystem particle;

        private void Awake() 
        {
            particle = GetComponent<ParticleSystem>();
            damager = GetComponent<Damager>();
        }

        public void Shoot() => particle.Emit(1);

        void OnParticleCollision(GameObject other) => damager.DamageTo(other);
    }
}
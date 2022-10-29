using UnityEngine;

namespace HNW
{
    public class Weapon : MonoBehaviour
    {
        private int emmitAmount;
        private float shootRate;
        private int shootAngle;
        private LayerMask layerToDamage;
        WeaponProjectile[] projectiles;

        public int EmmitAmount
        {
            get => emmitAmount;
            set
            {
                emmitAmount = value;

                for (int i = 0; i < projectiles.Length; i++)
                {
                    projectiles[i].EmmitAmount = emmitAmount;
                }
            }
        }

        public float ShootRate
        {
            get => shootRate;
            set
            {
                shootRate = value;

                for (int i = 0; i < projectiles.Length; i++)
                {
                    projectiles[i].ShootRate = shootRate;
                }
            }
        }

        public int ShootAngle
        {
            get => shootAngle;
            set
            {
                shootAngle = value;

                for (int i = 0; i < projectiles.Length; i++)
                {
                    projectiles[i].ShootAngle = shootAngle;
                }
            }
        }

        public LayerMask LayerToDamage
        {
            get => layerToDamage;
            set
            {
                layerToDamage = value;

                for (int i = 0; i < projectiles.Length; i++)
                {
                    projectiles[i].LayerToCollide = layerToDamage;
                }
            }
        }

        public WeaponBody Body { get; set; }

        public WeaponProjectile[] Projectiles
        {
            set
            {
                projectiles = value;

                for (int i = 0; i < projectiles.Length; i++)
                {
                    projectiles[i].OnProjectileHit += OnProjectileHit;
                }
            }
        }

        public void Shoot()
        {
            for (int i = 0; i < projectiles.Length; i++)
            {
                projectiles[i].Shoot();
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < projectiles.Length; i++)
            {
                projectiles[i].OnProjectileHit -= OnProjectileHit;
            }
        }

        private void OnProjectileHit(GameObject obj) => Damager.DamageTo(obj, 1);
    }
}
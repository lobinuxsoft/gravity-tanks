using UnityEngine;

namespace HNW
{
    public class Weapon : MonoBehaviour
    {
        private int shotDamage;
        private int emmitAmount;
        private float shotRate;
        private int shotAngle;
        private LayerMask layerToDamage;
        WeaponProjectile[] projectiles;

        public int ShotDamage
        {
            get => shotDamage;
            set => shotDamage = value;
        }

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

        public float ShotRate
        {
            get => shotRate;
            set
            {
                shotRate = value;

                for (int i = 0; i < projectiles.Length; i++)
                {
                    projectiles[i].ShotRate = shotRate;
                }
            }
        }

        public int ShotAngle
        {
            get => shotAngle;
            set
            {
                shotAngle = value;

                for (int i = 0; i < projectiles.Length; i++)
                {
                    projectiles[i].ShotAngle = shotAngle;
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

        private void OnProjectileHit(GameObject obj) => Damager.DamageTo(obj, shotDamage);
    }
}
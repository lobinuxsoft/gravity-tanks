
using System.Collections.Generic;
using UnityEngine;

namespace HNW
{
    public class WeaponBuilder
    {
        string name;
        Transform owner;
        WeaponBody weaponBody;
        WeaponProjectile projectileControl;
        LayerMask layerToDamage;
        float attackMultiplier;
        int emmitAmount;
        float shotRate;
        int shotAngle;

        public WeaponBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public WeaponBuilder WithOwner(Transform owner)
        {
            this.owner = owner;
            return this;
        }

        public WeaponBuilder WithBody(WeaponBody weaponBody)
        {
            this.weaponBody = weaponBody;
            return this;
        }

        public WeaponBuilder WithProjectile(WeaponProjectile projectileControl)
        {
            this.projectileControl = projectileControl;
            return this;
        }

        public WeaponBuilder WithLayerToDamage(LayerMask layerToDamage)
        {
            this.layerToDamage = layerToDamage;
            return this;
        }

        public WeaponBuilder WithAttackMultiplier(float attackMultiplier)
        {
            this.attackMultiplier = attackMultiplier;
            return this;
        }

        public WeaponBuilder WithEmmitAmount(int emmitAmount)
        {
            this.emmitAmount = emmitAmount;
            return this;
        }

        public WeaponBuilder WithShotRate(float shootRate)
        {
            this.shotRate = shootRate;
            return this;
        }

        public WeaponBuilder WithShotAngle(int shootAngle)
        {
            this.shotAngle = shootAngle;
            return this;
        }

        public Weapon Build()
        {
            Weapon weapon = new GameObject(name).AddComponent<Weapon>();
            weapon.transform.SetParent(owner);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;

            var body = Object.Instantiate(weaponBody, weapon.transform, false);
            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;

            List<WeaponProjectile> projectiles = new List<WeaponProjectile>();

            for (int i = 0; i < body.ShootPoints.Length; i++)
            {
                var projectile = Object.Instantiate(projectileControl, body.ShootPoints[i]);
                projectile.transform.localPosition = Vector3.zero;
                projectile.transform.localRotation = Quaternion.identity;
                projectiles.Add(projectile);
            }

            weapon.Body = body;
            weapon.Projectiles = projectiles.ToArray();
            weapon.LayerToDamage = layerToDamage;
            weapon.AttackMultiplier = attackMultiplier;
            weapon.EmmitAmount = emmitAmount;
            weapon.ShotRate = shotRate;
            weapon.ShotAngle = shotAngle;

            return weapon;
        }
    }
}
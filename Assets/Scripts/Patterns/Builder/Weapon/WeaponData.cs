using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Weapon Builder/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] int cost = 1000;

        [SerializeField] LayerMask layerToDamage;
        [SerializeField, Min(1)] float attackMultiplier = 1;
        [SerializeField, Min(1)] int emmitAmount = 1;
        [SerializeField] float shotRate = .5f;
        [SerializeField, Range(0, 360)] int shotAngle = 0;
        [SerializeField] WeaponBody body;
        [SerializeField] WeaponProjectile projectile;

        public int Cost => cost;

        public float AttackMultiplier => attackMultiplier;

        public int EmmitAmount => emmitAmount;

        public float ShotRate => shotRate;

        public int ShotAngle => shotAngle;

        public Weapon BuildWeapon(Transform container)
        {
            return new WeaponBuilder()
                .WithName(this.name)
                .WithOwner(container)
                .WithBody(body)
                .WithProjectile(projectile)
                .WithLayerToDamage(layerToDamage)
                .WithAttackMultiplier(attackMultiplier)
                .WithEmmitAmount(emmitAmount)
                .WithShotRate(shotRate)
                .WithShotAngle(shotAngle)
                .Build();
        }
    }
}
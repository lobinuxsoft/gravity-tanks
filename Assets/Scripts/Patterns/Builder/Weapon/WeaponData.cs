using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Weapon Builder/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] LayerMask layerToDamage;
        [SerializeField, Min(1)] float attackMultiplier = 1;
        [SerializeField, Min(1)] int emmitAmount = 1;
        [SerializeField] float shotRate = .5f;
        [SerializeField, Range(0, 360)] int shotAngle = 0;
        [SerializeField] WeaponBody body;
        [SerializeField] WeaponProjectile projectile;

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
using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Weapon Builder/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] LayerMask layerToDamage;
        [SerializeField, Min(1)] int emmitAmount = 1;
        [SerializeField] float shootRate = .5f;
        [SerializeField, Range(0, 360)] int shootAngle = 0;
        [SerializeField] WeaponBody body;
        [SerializeField] WeaponProjectile projectile;

        public Weapon BuildWeapon(Transform container)
        {
            return new WeaponBuilder()
                .WithName(this.name)
                .WithParent(container)
                .WithBody(body)
                .WithProjectile(projectile)
                .WithLayerToDamage(layerToDamage)
                .WithEmmitAmount(emmitAmount)
                .WithShootRate(shootRate)
                .WithShootAngle(shootAngle)
                .Build();
        }
    }
}
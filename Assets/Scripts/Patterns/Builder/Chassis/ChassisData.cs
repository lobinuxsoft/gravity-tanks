using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Chassis Builder/Chassis Data")]
    public class ChassisData : ScriptableObject
    {
        [SerializeField, Min(1)] float maxHealthMultiplier = 1;
        [SerializeField, Min(1)] float defenseMultiplier = 1;
        [SerializeField] MeshFilter chassisBody;

        public Chassis BuildChassis(Transform container)
        {
            return new ChassisBuilder()
                .WithName(this.name)
                .WithOwner(container)
                .WithBody(chassisBody)
                .WithMaxHealthMultiplier(maxHealthMultiplier)
                .WithDefenseMultiplier(defenseMultiplier)
                .Build();
        }
    }
}
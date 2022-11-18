using UnityEngine;

namespace HNW
{
    public class Chassis : MonoBehaviour
    {
        [SerializeField, Min(1)] float maxHealthMultiplier;
        [SerializeField, Min(1)] float defenseMultiplier;

        public float MaxHealthMultiplier
        {
            get => maxHealthMultiplier;
            set => maxHealthMultiplier = value;
        }

        public float DefenseMultiplier
        {
            get => defenseMultiplier;
            set => defenseMultiplier = value;
        }
    }
}
using UnityEngine;

namespace HNW
{
    public class Chassis : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] int defense;

        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public int Defense
        {
            get => defense;
            set => defense = value;
        }
    }
}
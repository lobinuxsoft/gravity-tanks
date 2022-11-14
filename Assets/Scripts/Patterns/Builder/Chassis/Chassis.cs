using UnityEngine;

namespace HNW
{
    public class Chassis : MonoBehaviour
    {
        [SerializeField] int defense;

        public int Defense
        {
            get => defense;
            set => defense = value;
        }
    }
}
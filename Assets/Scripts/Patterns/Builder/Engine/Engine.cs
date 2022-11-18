using UnityEngine;

namespace HNW
{
    public class Engine : MonoBehaviour
    {
        [SerializeField, Min(1)] float moveForceMultiplier;
        [SerializeField, Min(1)] float turnSpeedMultiplier;

        public float MoveForceMultiplier
        {
            get => moveForceMultiplier;
            set => moveForceMultiplier = value;
        }

        public float TurnSpeedMultiplier
        {
            get => turnSpeedMultiplier;
            set => turnSpeedMultiplier = value;
        }
    }
}
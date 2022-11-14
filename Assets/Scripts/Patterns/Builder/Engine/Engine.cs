using UnityEngine;

namespace HNW
{
    public class Engine : MonoBehaviour
    {
        [SerializeField] int moveForce;
        [SerializeField] int turnSpeed;

        public int MoveForce
        {
            get => moveForce;
            set => moveForce = value;
        }

        public int TurnSpeed
        {
            get => turnSpeed;
            set => turnSpeed = value;
        }
    }
}
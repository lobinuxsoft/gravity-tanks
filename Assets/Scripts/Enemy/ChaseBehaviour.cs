using UnityEngine;

namespace GravityTanks.Enemy.Behaviour
{
    [CreateAssetMenu(menuName = "Enemy/Behaviour/Chase")]
    public class ChaseBehaviour : IABehaviour
    {
        [SerializeField] private LayerMask rayLayerMask;
        [SerializeField] private float moveForce = 10, turnSpeed = 10, heighForce = 5;

        HoverMovement hoverMovement;
        private Transform target;
        private Vector3 targetDir = Vector3.zero;

        public override void DoBehaviour(GameObject owner)
        {
            if (!hoverMovement)
            {
                hoverMovement = owner.AddComponent<HoverMovement>();
                hoverMovement.RayLayerMask = rayLayerMask;
                hoverMovement.HeighForce = heighForce;
                hoverMovement.MoveForce = moveForce;
                hoverMovement.TurnSpeed = turnSpeed;
            }

            if (!target) target = GameObject.FindWithTag("Player").transform;

            Vector3 dir = (target.position - owner.transform.position).normalized;

            Vector3 forward = ProjectDirectionOnPlane(Vector3.forward, Vector3.up);
            Vector3 right = ProjectDirectionOnPlane(Vector3.right, Vector3.up);

            targetDir = dir.x * right + dir.z * forward;

            hoverMovement.Move(new Vector2(targetDir.x, targetDir.z));
        }

        Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
        {
            return (direction - normal * Vector3.Dot(direction, normal)).normalized;
        }
    }
}
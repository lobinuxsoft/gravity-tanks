#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace GravityTanks.Enemy.Behaviour
{
    [CreateAssetMenu(menuName = "Enemy/Behaviour/Chase Behaviour")]
    public class ChaseBehaviour : IABehaviour
    {
        [SerializeField] private float torqueSpeed = 20;
        [SerializeField] private float maxAngularVelocity = 180;

        private Rigidbody body;
        private Transform target;
        private Vector3 targetDir = Vector3.zero;

        public override void DoBehaviour(GameObject owner)
        {
            if(!body) body = owner.GetComponent<Rigidbody>();
            if (!target) target = GameObject.FindWithTag("Player").transform;

            body.angularDrag = maxAngularVelocity * .25f;
            body.maxAngularVelocity = maxAngularVelocity;

            Vector3 dir = (target.position - owner.transform.position).normalized;

            Vector3 forward = ProjectDirectionOnPlane(Vector3.forward, Vector3.up);
            Vector3 right = ProjectDirectionOnPlane(Vector3.right, Vector3.up);

            targetDir = -dir.x * forward + dir.z * right;

            body.AddTorque(torqueSpeed * targetDir, ForceMode.VelocityChange);
        }

#if UNITY_EDITOR
        public override void DrawGizmos(GameObject owner)
        {
            Handles.ArrowHandleCap(0, owner.transform.position, Quaternion.LookRotation(targetDir), 1, EventType.Repaint);
        }
#endif

        Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
        {
            return (direction - normal * Vector3.Dot(direction, normal)).normalized;
        }
    }
}
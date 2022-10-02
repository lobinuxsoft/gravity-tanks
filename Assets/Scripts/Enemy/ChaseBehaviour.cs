using UnityEngine;
using UnityEngine.AI;

namespace GravityTanks.Enemy.Behaviour
{
    [CreateAssetMenu(menuName = "Enemy/Behaviour/Chase")]
    public class ChaseBehaviour : IABehaviour
    {
        [SerializeField] float refreshTargetPositionRate = .25f;
        [SerializeField] private float moveSpeed = 5, turnSpeed = 5, heighForce = 1;

        private NavMeshAgent agent;
        private Rigidbody rb;
        private Transform target;

        private float lastRefresh;
        
        public override void DoBehaviour(GameObject owner)
        {
            if (!target) target = GameObject.FindWithTag("Player").transform;

            if (!agent)
            {
                agent = owner.AddComponent<NavMeshAgent>();
                agent.speed = moveSpeed;
                agent.angularSpeed = turnSpeed;
                agent.baseOffset = heighForce;
            }

            if(!rb)
            {
                rb = owner.GetComponent<Rigidbody>();
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rb.isKinematic = true;
            }

            if(Time.time - lastRefresh < refreshTargetPositionRate)
            {
                lastRefresh = Time.time;
                agent.SetDestination(target.position);
            }
        }
    }
}
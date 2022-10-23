using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace HNW.Enemy.Behaviour
{
    [CreateAssetMenu(menuName = "Enemy/Behaviour/Chase")]
    public class ChaseBehaviour : IABehaviour
    {
        [SerializeField] float timeBetweenAttacks = 1;
        [SerializeField] float attackDistanceThreshold = 1.5f;
        [SerializeField] int damageAmount = 1;
        [SerializeField] float refreshTargetPositionRate = .25f;
        [SerializeField] private float moveSpeed = 5, turnSpeed = 5, heighForce = 1;

        private NavMeshAgent agent;
        private Rigidbody rb;
        private GameObject target;
        private Renderer rend;
        private Damager damager;

        private float lastRefresh;
        private float nextAttackTime;

        public override void InitBehaviour(GameObject owner)
        {
            if (!target)
                target = GameObject.FindWithTag("Player");

            if (!damager)
            {
                damager = owner.AddComponent<Damager>();
                damager.DamageAmount = damageAmount;
            }

            if (!agent)
            {
                agent = owner.AddComponent<NavMeshAgent>();
                agent.speed = moveSpeed;
                agent.angularSpeed = turnSpeed;
                agent.baseOffset = heighForce;
            }

            if (!rb)
            {
                rb = owner.GetComponent<Rigidbody>();
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rb.isKinematic = true;
            }

            if (!rend)
                rend = owner.GetComponent<Renderer>();
        }

        public override async void DoBehaviour()
        {
            if (!target)
            {
                target = GameObject.FindWithTag("Player");
            }

            if (target && target.activeSelf && Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.transform.position - rb.transform.position).sqrMagnitude;

                if(sqrDstToTarget < Mathf.Pow(attackDistanceThreshold, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    await AttackTask();
                }
            }

            if(target && target.activeSelf && agent.isActiveAndEnabled && Time.time - lastRefresh > refreshTargetPositionRate)
            {
                lastRefresh = Time.time;
                agent.SetDestination(target.transform.position);
            }
        }


        async Task AttackTask()
        {
            agent.enabled = false;

            Vector3 oriPos = rb.position;
            Vector3 attackPos = target.transform.position;

            float attackSpeed = 3;
            float percent = 0;

            Color originalColor = rend.material.color;

            rend.material.color = Color.yellow;

            bool hasAppliedDamage = false;

            while (percent <= 1)
            {
                if(percent >= .5f && !hasAppliedDamage)
                {
                    hasAppliedDamage = true;
                    damager.DamageTo(target.gameObject);
                }

                percent += Time.deltaTime * attackSpeed;
                float interpolate = (-Mathf.Pow(percent, 2) + percent) * 4;
                rb.MovePosition(Vector3.Lerp(oriPos, attackPos, interpolate));

                await Task.Yield();
            }

            rend.material.color = originalColor;
            agent.enabled = true;
        }
    }
}
using UnityEngine;
using GravityTanks.Enemy.Behaviour;

namespace GravityTanks.Enemy
{
    public class EnemyIA : MonoBehaviour
    {
        [SerializeField] IABehaviour[] behaviours;

        IABehaviour behaviour;

        private void Awake()
        {
            Random.InitState(this.GetHashCode());

            behaviour = Object.Instantiate(behaviours[Random.Range(0, behaviours.Length)]);
        }

        private void FixedUpdate()
        {
            if(behaviour) behaviour.DoBehaviour(this.gameObject);
        }

        private void OnDestroy() => Destroy(behaviour);
    }
}
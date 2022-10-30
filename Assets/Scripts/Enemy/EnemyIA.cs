using UnityEngine;
using HNW.Enemy.Behaviour;

namespace HNW.Enemy
{
    public class EnemyIA : MonoBehaviour
    {
        [SerializeField] IABehaviour[] behaviours;

        IABehaviour behaviour;

        private void Awake()
        {
            Random.InitState(this.GetHashCode());

            behaviour = Object.Instantiate(behaviours[Random.Range(0, behaviours.Length)]);
            behaviour.InitBehaviour(this.gameObject);
        }

        private void FixedUpdate()
        {
            if(behaviour) behaviour.DoBehaviour();
        }

        private void OnDestroy() => Destroy(behaviour);
    }
}
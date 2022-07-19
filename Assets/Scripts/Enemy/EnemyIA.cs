using UnityEngine;
using GravityTanks.Enemy.Behaviour;

namespace GravityTanks.Enemy
{
    public class EnemyIA : MonoBehaviour
    {
        [SerializeField] IABehaviour[] behaviour;

        int behaviourIndex;

        private void Awake()
        {
            Random.InitState(Mathf.RoundToInt(Time.time));

            behaviourIndex = Random.Range(0, behaviour.Length);
        }

        private void FixedUpdate()
        {
            if(behaviour != null && behaviour.Length > 0) behaviour[behaviourIndex].DoBehaviour(this.gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (behaviour != null && behaviour.Length > 0) behaviour[behaviourIndex].DrawGizmos(this.gameObject);
        }
#endif
    }
}
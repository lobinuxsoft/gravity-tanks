using UnityEngine;

namespace HNW.Enemy.Behaviour
{
    public abstract class IABehaviour : ScriptableObject
    {
        public abstract void InitBehaviour(GameObject owner);
        public abstract void DoBehaviour();
    }
}
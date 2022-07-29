using UnityEngine;

namespace GravityTanks.Enemy.Behaviour
{
    public abstract class IABehaviour : ScriptableObject
    {
        public abstract void DoBehaviour(GameObject owner);
    }
}
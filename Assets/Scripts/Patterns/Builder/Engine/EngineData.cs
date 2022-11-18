using System;
using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Engine Builder/Engine Data")]
    public class EngineData : ScriptableObject
    {
        [SerializeField, Min(1)] float moveForceMultiplier = 1;
        [SerializeField, Min(1)] float turnSpeedMultiplier = 1;
        [SerializeField] MeshFilter engineBody;

        public Engine BuildEngine(Transform owner)
        {
            return new EngineBuilder()
                .WithName(this.name)
                .WithOwner(owner)
                .WithBody(engineBody)
                .WithMoveForceMultiplier(moveForceMultiplier)
                .WithTurnSpeedMultiplier(turnSpeedMultiplier)
                .Build();
        }
    }
}
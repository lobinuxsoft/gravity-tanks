using System;
using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Engine Builder/Engine Data")]
    public class EngineData : ScriptableObject
    {
        [SerializeField] int cost = 1000;
        [SerializeField, Min(.1f)] float moveForceMultiplier = 1;
        [SerializeField, Min(.1f)] float turnSpeedMultiplier = 1;
        [SerializeField] MeshFilter engineBody;

        public int Cost => cost;

        public float MoveForceMultiplier => moveForceMultiplier;
        public float TurnSpeedMultiplier => turnSpeedMultiplier;

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
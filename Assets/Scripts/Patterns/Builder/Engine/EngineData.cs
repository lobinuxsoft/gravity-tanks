using System;
using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Engine Builder/Engine Data")]
    public class EngineData : ScriptableObject
    {
        [SerializeField] int moveForce;
        [SerializeField] int turnSpeed;
        [SerializeField] MeshFilter engineBody;

        public Engine BuildEngine(Transform owner)
        {
            return new EngineBuilder()
                .WithName(this.name)
                .WithOwner(owner)
                .WithBody(engineBody)
                .WithMoveForce(moveForce)
                .WithTurnSpeed(turnSpeed)
                .Build();
        }
    }
}
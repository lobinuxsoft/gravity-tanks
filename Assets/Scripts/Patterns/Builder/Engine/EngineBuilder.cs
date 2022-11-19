using UnityEngine;

namespace HNW
{
    public class EngineBuilder
    {
        string name;
        Transform owner;
        MeshFilter engineBody;
        float moveForceMultiplier;
        float turnSpeedMultiplier;

        public EngineBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public EngineBuilder WithOwner(Transform owner)
        {
            this.owner = owner;
            return this;
        }

        public EngineBuilder WithBody(MeshFilter engineBody)
        {
            this.engineBody = engineBody;
            return this;
        }

        public EngineBuilder WithMoveForceMultiplier(float moveForce)
        {
            this.moveForceMultiplier = moveForce;
            return this;
        }

        public EngineBuilder WithTurnSpeedMultiplier(float turnSpeed)
        {
            this.turnSpeedMultiplier = turnSpeed;
            return this;
        }

        public Engine Build()
        {
            Engine engine = Object.Instantiate(engineBody, owner).gameObject.AddComponent<Engine>();
            engine.name = name;
            engine.MoveForceMultiplier = moveForceMultiplier;
            engine.TurnSpeedMultiplier = turnSpeedMultiplier;

            return engine;
        }
    }
}
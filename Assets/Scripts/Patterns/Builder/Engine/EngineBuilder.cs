using UnityEngine;

namespace HNW
{
    public class EngineBuilder
    {
        string name;
        Transform owner;
        MeshFilter engineBody;
        int moveForce;
        int turnSpeed;

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

        public EngineBuilder WithMoveForce(int moveForce)
        {
            this.moveForce = moveForce;
            return this;
        }

        public EngineBuilder WithTurnSpeed(int turnSpeed)
        {
            this.turnSpeed = turnSpeed;
            return this;
        }

        public Engine Build()
        {
            Engine engine = Object.Instantiate(engineBody, owner).gameObject.AddComponent<Engine>();
            engine.name = name;
            engine.MoveForce = moveForce;
            engine.TurnSpeed = turnSpeed;

            return engine;
        }
    }
}
using UnityEngine;

namespace HNW
{
    public class ChassisBuilder
    {
        string name;
        Transform owner;
        MeshFilter chassisBody;
        int defense;

        public ChassisBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public ChassisBuilder WithOwner(Transform owner)
        {
            this.owner = owner;
            return this;
        }

        public ChassisBuilder WithBody(MeshFilter chassisBody)
        {
            this.chassisBody = chassisBody;
            return this;
        }

        public ChassisBuilder WithDefense(int defense)
        {
            this.defense = defense;
            return this;
        }

        public Chassis Build()
        {
            Chassis chassis = Object.Instantiate(chassisBody, owner).gameObject.AddComponent<Chassis>();
            chassis.name = name;
            chassis.Defense = defense;

            if(owner.TryGetComponent(out MeshCollider mc))
            {
                mc.sharedMesh = chassisBody.sharedMesh;
                mc.convex = true;
            }
            else
            {
                mc = owner.gameObject.AddComponent<MeshCollider>();
                mc.sharedMesh = chassisBody.sharedMesh;
                mc.convex = true;
            }

            return chassis;
        }
    }
}
using UnityEngine;

namespace HNW
{
    public class ChassisBuilder
    {
        string name;
        Transform owner;
        MeshFilter chassisBody;
        float maxHealthMultiplier;
        float defenseMultiplier;

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

        public ChassisBuilder WithMaxHealthMultiplier(float maxHealthMultiplier)
        {
            this.maxHealthMultiplier = maxHealthMultiplier;
            return this;
        }

        public ChassisBuilder WithDefenseMultiplier(float defenseMultiplier)
        {
            this.defenseMultiplier = defenseMultiplier;
            return this;
        }

        public Chassis Build()
        {
            Chassis chassis = Object.Instantiate(chassisBody, owner).gameObject.AddComponent<Chassis>();
            chassis.name = name;
            chassis.MaxHealthMultiplier = maxHealthMultiplier;
            chassis.DefenseMultiplier = defenseMultiplier;

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
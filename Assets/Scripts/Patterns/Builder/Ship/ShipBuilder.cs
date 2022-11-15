using System.Collections.Generic;
using UnityEngine;

namespace HNW
{
    public class ShipBuilder
    {
        string name;
        Transform owner;
        ChassisData chassisData;
        EngineData engineData;
        WeaponData[] weaponsData;

        public ShipBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public ShipBuilder WithOwner(Transform owner)
        {
            this.owner = owner;
            return this;
        }

        public ShipBuilder WithChassis(ChassisData chassisData)
        {
            this.chassisData = chassisData;
            return this;
        }

        public ShipBuilder WithEngine(EngineData engineData)
        {
            this.engineData = engineData;
            return this;
        }

        public ShipBuilder WithWeapons(WeaponData[] weaponsData)
        {
            this.weaponsData = weaponsData;
            return this;
        }

        public Ship Build()
        {
            Ship ship = owner.gameObject.AddComponent<Ship>();

            ship.name = name;

            ship.Chassis = chassisData.BuildChassis(ship.transform);

            ship.Engine = engineData.BuildEngine(ship.transform);

            List<Weapon> weapons = new List<Weapon>();

            for (int i = 0; i < weaponsData.Length; i++)
            {
                weapons.Add(weaponsData[i].BuildWeapon(ship.transform));
            }

            ship.Weapons = weapons.ToArray();

            return ship;
        }
    }
}
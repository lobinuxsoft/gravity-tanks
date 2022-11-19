using CryingOnionTools.ScriptableVariables;
using System.Collections.Generic;
using UnityEngine;

namespace HNW
{
    [CreateAssetMenu(menuName = "Hyper Net Warrior/Ship Builder/Ship Data")]
    public class ShipData : ScriptableVariable<ShipStruct>
    {
        [SerializeField] ChassisShopData chassisShopData;
        [SerializeField] EnginesShopData enginesShopData;
        [SerializeField] WeaponsShopData weaponsShopData;

        public override void EraseData()
        {
            base.EraseData();

            value.maxHp = 10;
            value.attack = 1;
            value.defense = 1;
            value.speed = 15;

            if (chassisShopData)
                value.chassisName = chassisShopData.GetAllChassis()[0].name;
            else
                value.chassisName = "";

            if (enginesShopData)
                value.engineName = enginesShopData.GetAllEngines()[0].name;
            else
                value.engineName = "";

            if (weaponsShopData)
                value.weaponsNames = new string[] { weaponsShopData.GetAllWeapons()[0].name };
            else
                value.weaponsNames = new string[0];
        }

        public Ship BuildShip(Transform owner)
        {
            List<WeaponData> weapons = new List<WeaponData>();

            for (int i = 0; i < value.weaponsNames.Length; i++)
            {
                weapons.Add(weaponsShopData.GetWeaponDataByName(value.weaponsNames[i]));
            }

            return new ShipBuilder()
                .WithName(this.name)
                .WithOwner(owner)
                .WithMaxHp(value.maxHp)
                .WithAttack(value.attack)
                .WithDefense(value.defense)
                .WithSpeed(value.speed)
                .WithChassis(chassisShopData.GetChassisDataByName(value.chassisName))
                .WithEngine(enginesShopData.GetEngineDataByName(value.engineName))
                .WithWeapons(weapons.ToArray())
                .Build();
        }
    }
}

[System.Serializable]
public struct ShipStruct
{
    public int maxHp;
    public int attack;
    public int defense;
    public int speed;

    public string chassisName;
    public string engineName;
    public string[] weaponsNames;
}
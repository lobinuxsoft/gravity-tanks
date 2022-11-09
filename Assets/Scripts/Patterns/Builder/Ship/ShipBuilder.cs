using UnityEngine;

namespace HNW
{
    public class ShipBuilder
    {
        string name;
        Transform owner;
        uint maxHp;
        uint attack;
        uint defense;
        uint speed;
        string chassis;
        string engine;
        string[] weapons;

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

        public ShipBuilder WithMaxHP(uint maxHp)
        {
            this.maxHp = maxHp;
            return this;
        }

        public ShipBuilder WithAttack(uint attack)
        {
            this.attack = attack;
            return this;
        }

        public ShipBuilder WithDefense(uint defense)
        {
            this.defense = defense;
            return this;
        }

        public ShipBuilder WithSpeed(uint speed)
        {
            this.speed = speed;
            return this;
        }

        public ShipBuilder WithChassis(string chassis)
        {
            this.chassis = chassis;
            return this;
        }

        public ShipBuilder WithEngine(string engine)
        {
            this.engine = engine;
            return this;
        }

        public ShipBuilder WithWeapons(string[] weapons)
        {
            this.weapons = weapons;
            return this;
        }
    }
}
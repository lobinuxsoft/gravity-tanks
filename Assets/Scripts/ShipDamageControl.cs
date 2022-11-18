using CryingOnionTools.ScriptableVariables;
using UnityEngine;

namespace HNW
{
    public class ShipDamageControl : Damageable
    {
        [SerializeField] IntVariable curHealth;
        [SerializeField] IntVariable maxHealth;

        Ship ship;

        public Ship Ship
        {
            set
            {
                ship = value;
                MaxHealth =  Mathf.RoundToInt( ship.MaxHP * ship.Chassis.MaxHealthMultiplier);
                Health = MaxHealth;
            }
        }

        public override int Health
        {
            get => curHealth.Value;
            set
            {
                curHealth.Value = value;

                if (curHealth.Value <= 0)
                    onDie?.Invoke(this.gameObject);
            }
        }

        public override int MaxHealth
        {
            get => maxHealth.Value;
            set => maxHealth.Value = value;
        }

        public override void SetDamage(int value)
        {
            if (isActiveAndEnabled)
                StartCoroutine(BlinkEffect());

            int toDamage = Mathf.RoundToInt(value * (100 / (100 + (ship.Defense * ship.Chassis.DefenseMultiplier))));
            Health -= toDamage < 0 ? 0 : toDamage;
        }
    }
}
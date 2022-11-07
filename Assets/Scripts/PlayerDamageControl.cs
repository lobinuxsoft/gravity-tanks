using CryingOnionTools.ScriptableVariables;
using UnityEngine;

namespace HNW
{
    public class PlayerDamageControl : Damageable
    {
        [SerializeField] IntVariable curHealth;
        [SerializeField] IntVariable maxHealth;

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
    }
}
using UnityEngine;

namespace HNW
{
    public class Ship : MonoBehaviour
    {
        Engine engine;
        Chassis chassis;
        Weapon[] weapons;

        public Engine Engine
        {
            get => engine;
            set
            {
                engine = value;

                if(TryGetComponent(out HoverMovement move))
                {
                    move.MoveForce = engine.MoveForce;
                    move.TurnSpeed = engine.TurnSpeed;
                }

                if (TryGetComponent(out ShootControl sc))
                    sc.RotationSpeed = engine.TurnSpeed;
            }
        }

        public Chassis Chassis
        {
            get => chassis;
            set
            {
                chassis = value;

                if(TryGetComponent(out ShipDamageControl phc))
                    phc.Ship = this;
            }
        }

        public Weapon[] Weapons
        {
            get => weapons;
            set
            {
                weapons = value;

                if(TryGetComponent(out ShootControl sc))
                    sc.UpdateWeapons();
            }
        }
    }
}
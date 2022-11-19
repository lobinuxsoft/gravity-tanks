using System;
using UnityEngine;

namespace HNW
{
    public class Ship : MonoBehaviour
    {
        Engine engine;
        Chassis chassis;
        Weapon[] weapons;

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int Speed { get; set; }

        public int MaxHP { get; set; }

        public Engine Engine
        {
            get => engine;
            set
            {
                engine = value;

                if(TryGetComponent(out HoverMovement move))
                {
                    move.MoveForce = Speed * engine.MoveForceMultiplier;
                    move.TurnSpeed = Speed * engine.TurnSpeedMultiplier;
                }

                if (TryGetComponent(out ShootControl sc))
                    sc.RotationSpeed = Speed * engine.TurnSpeedMultiplier;
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
                {
                    sc.UpdateWeapons();
                    sc.onProjectileHit += OnProjectileHit;
                }
            }
        }

        private void OnProjectileHit(GameObject hitObj, float multiplayer)
        {
            Damager.DamageTo(hitObj, Mathf.RoundToInt(Attack * multiplayer));
        }
    }
}
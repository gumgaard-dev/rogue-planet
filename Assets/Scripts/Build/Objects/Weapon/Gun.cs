using Build.Component;
using Capstone.Build.Objects;
using Capstone.Build.Objects.ObjectPool;
using System;
using UnityEngine;

namespace Capstone.Build.Weapon
{
    [RequireComponent(typeof(ProjectileSpawner))]
    public class Gun : PoolUser
    {
        public float ShotInterval;
        public float ShotForceMagnitude;

        private Vector2 ProjectileSpawnPosition => InstantiationPoint.position;
        private Quaternion ProjectileSpawnRotation => InstantiationPoint.rotation;
        private Vector2 ShotDirection => InstantiationPoint.right;

        private Cooldown _shotCooldown;

        private void Start()
        {
            CreatePool(2);

            this._shotCooldown = new Cooldown(ShotInterval);
            _shotCooldown.Activate();
        }

        public void Shoot()
        {
            if (_shotCooldown.IsAvailable())
            {
                _shotCooldown.Activate();

                Projectile p = GetFromPool() as Projectile;

                p.AddImpulseForce(ShotForceMagnitude * ShotDirection);
            }
        }
    }
}

using Build.Component;
using Capstone.Build.Objects;
using System;
using UnityEngine;

namespace Capstone.Build.Weapon
{
    [RequireComponent(typeof(ProjectileSpawner))]
    public class Gun : MonoBehaviour
    {
        public Transform ProjectileSpawnTransform;
        public float ShotInterval;
        public float ShotForceMagnitude;
        public Projectile Projectile;
        public GameObject ProjectileContainer;

        private Vector2 ProjectileSpawnPosition => ProjectileSpawnTransform.position;
        private Quaternion ProjectileSpawnRotation => ProjectileSpawnTransform.rotation;
        private Vector2 ShotDirection => ProjectileSpawnTransform.right;

        private ObjectPool<Projectile> _projectilePool;

        private Cooldown _shotCooldown;

        private void Start()
        {
            if (Projectile != null)
            {
                 this._projectilePool = new ObjectPool<Projectile>(Projectile, 20, ProjectileContainer);
            }
            
            this._shotCooldown = new Cooldown(ShotInterval);
            _shotCooldown.Activate();
        }

        public void Shoot()
        {
            if (_shotCooldown.IsAvailable())
            {
                _shotCooldown.Activate();

                Projectile projectileToShoot = _projectilePool.Get(); // Get a projectile from the pool
                projectileToShoot.transform.SetPositionAndRotation(ProjectileSpawnPosition, ProjectileSpawnRotation);

                projectileToShoot.AddImpulseForce(ShotForceMagnitude * ShotDirection);
            }
        }

        // Call this method to return a projectile to the pool
        public void ReturnProjectile(Projectile projectileToReturn)
        {
            projectileToReturn.StopMoving();
            _projectilePool.ReturnToPool(projectileToReturn);
        }
    }
}

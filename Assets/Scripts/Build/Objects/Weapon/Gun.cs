using Build.Component;
using Capstone.Build.Cam;
using Capstone.Build.Objects;
using Capstone.Build.Objects.ObjectPool;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Capstone.Build.Weapon
{
    [RequireComponent(typeof(ProjectileSpawner))]
    public class Gun : PoolUser
    {
        public float ShotInterval;
        public float ShotForceMagnitude;
        private Vector2 ShotDirection => InstantiationPoint.right;

        private Cooldown _shotCooldown;

        [Header("Visual Effects")]
        public bool ScreenShakeEnabled = false;
        public float ScreenShakeDuration = 0.2f;
        public float ScreenShakeIntensity = 0.4f;

        
        private CameraShakeEvent _cameraShakeOnShoot;

        public bool MuzzleFlashEnabled = false;
        public SpriteRenderer _muzzleFlashRenderer;
        private Light2D _muzzleFlashLight2D;
        public float MuzzleFlashTime = 0.05f;


        private void Start()
        {
            CreatePool(10);

            this._shotCooldown = new Cooldown(ShotInterval);
            _shotCooldown.Activate();

            _cameraShakeOnShoot = new();


            if (MuzzleFlashEnabled)
            {
                if (_muzzleFlashRenderer != null)
                {
                    _muzzleFlashRenderer.enabled = false;

                    // If there's a muzzle flash sprite, also get the MFlash light
                    if (_muzzleFlashRenderer.TryGetComponent(out _muzzleFlashLight2D))
                    {
                        _muzzleFlashLight2D.enabled = false;
                    }
                }
                // Disable muzzle flash flag if there's no muzzle flash renderer
                else
                {
                    MuzzleFlashEnabled = false;
                }
            }

            if (ScreenShakeEnabled)
            {
                _cameraShakeOnShoot.AddListener(Camera.main.GetComponent<CameraShaker>().ShakeCamera);
            }
        }

        public void Shoot()
        {
            if (_shotCooldown.IsAvailable())
            {
                _shotCooldown.Activate();

                Projectile p = InstantiateFromPool() as Projectile;

                p.AddImpulseForce(ShotForceMagnitude * ShotDirection);
                if (ScreenShakeEnabled)
                {
                    _cameraShakeOnShoot?.Invoke(ScreenShakeDuration, ScreenShakeIntensity);
                }

                if (MuzzleFlashEnabled)
                {
                    StartCoroutine(ShowMuzzleFlash());
                }
            }
        }

        IEnumerator ShowMuzzleFlash()
        {
            if (_muzzleFlashRenderer != null)
            {
                float curTime = 0;
                _muzzleFlashRenderer.enabled = true;
                if (_muzzleFlashLight2D != null) { _muzzleFlashLight2D.enabled = true; }
                
                while (curTime < MuzzleFlashTime) 
                {
                    curTime += Time.deltaTime;
                    yield return 0;
                }
                
                _muzzleFlashRenderer.enabled = false;
                if (_muzzleFlashLight2D != null) { _muzzleFlashLight2D.enabled = false; }
            }
        }
    }
}

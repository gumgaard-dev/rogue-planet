using System;
using UnityEngine;

namespace Build.Component
{
    public class Projectile : PoolableObject
    {
        public int AttackPower = 5;

        private Rigidbody2D _rb;

        [Header("If disabled, the projectile will only hit a single target")]
        public bool AllowMultipleHits;
        
        // used by single-hit projectiles to determine damage
        private bool _hasHit;

        public override void Initialize()
        {
            base.Initialize();

            if (!TryGetComponent(out _rb)) {
                Debug.LogWarning("No rigidbody2d attached!");
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("MainCamera"))
            {
                Debug.Log(gameObject.name + "left camera bounds.");
                ReturnToPool();
            }
        }

        private void OnDisable()
        {
            this._rb.velocity = Vector3.zero;
        }

        private void OnEnable()
        {
            _hasHit = false;
        }

        public void AddImpulseForce(Vector2 force)
        {
            if (_rb != null)
            {
                _rb.velocity = Vector3.zero;
                _rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_hasHit || AllowMultipleHits)
            {
                _hasHit = true;
                var targetHealth = collision.gameObject.GetComponent<HealthData>();

                //apply damage to target
                if (targetHealth)
                {
                    targetHealth.Damage(AttackPower);
                }

                ReturnToPool();
            }
        }
    }
}
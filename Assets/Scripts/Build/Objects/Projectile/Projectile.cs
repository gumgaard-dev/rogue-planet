using System;
using UnityEngine;

namespace Build.Component
{
    public class Projectile : PoolableObject
    {
        public int AttackPower = 5;
        public Rigidbody2D RB;
        

        void OnBecameInvisible()
        {
            base.ReturnToPool();
        }

        public void StopMoving()
        {
            this.RB.velocity = Vector3.zero;
        }

        public void AddImpulseForce(Vector2 force)
        {
            if (RB != null)
            {
                RB.AddForce(force, ForceMode2D.Impulse);
            }
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Projectile hit: " + collision.gameObject.name);

            var targetHealth = collision.gameObject.GetComponent<HealthData>();

            //apply damage to target
            if (targetHealth)
            {
                targetHealth.Damage(AttackPower);
            }

            base.ReturnToPool();
        }
    }
}
using System;
using UnityEngine;

namespace Build.Component
{
    public class Projectile : PoolableObject
    {
        public int AttackPower = 5;
        public Rigidbody2D RB;


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Camera"))
            {
                ReturnToPool();
            }
        }

        private void OnDisable()
        {
            this.RB.velocity = Vector3.zero;
        }

        public void AddImpulseForce(Vector2 force)
        {
            if (RB != null)
            {
                RB.velocity = Vector3.zero;
                RB.AddForce(force, ForceMode2D.Impulse);
            }
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            try
            {
                Debug.Log("Projectile hit: " + collision.gameObject.name);

                var targetHealth = collision.gameObject.GetComponent<HealthData>();

                //apply damage to target
                if (targetHealth)
                {
                    targetHealth.Damage(AttackPower);
                }

                ReturnToPool();
            } catch (Exception e) { Debug.LogError(e.Message); }

        }

        private void OnDestroy()
        {
            Debug.Log("Destroyed!");
        }
    }
}
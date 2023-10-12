using Build.Component;
using UnityEngine;

namespace Build.Generic
{
    public class Projectile : MonoBehaviour
    {
        public int AttackPower;
        private void OnTriggerEnter2D(Collider2D col)
        {
            var targetHealth = col.GetComponent<HealthData>();
            
            //apply damage to target
            if (targetHealth != null)
            {
                targetHealth.Damage(AttackPower);  
            }
            
            //destroy the projectile that caused the collision
            Destroy(gameObject);
        }
    }
}
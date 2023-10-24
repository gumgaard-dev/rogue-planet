using UnityEngine;

namespace Build.Component
{
    public class Projectile : MonoBehaviour
    {
        public int AttackPower;
        private void OnTriggerEnter2D(Collider2D col)
        {
            //ignore collisions with non-actor objects
            if (!col.CompareTag("Player") || !col.CompareTag("Enemy")) return;
            
            Debug.Log("Projectile hit: " + col.gameObject.name);
            
            var targetHealth = col.GetComponent<HealthData>();
            
            //apply damage to target
            if (targetHealth)
            {
                targetHealth.Damage(AttackPower);  
            }
            
            //destroy the projectile that caused the collision
            Destroy(gameObject);
        }
    }
}
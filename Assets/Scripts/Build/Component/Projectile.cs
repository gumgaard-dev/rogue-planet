using UnityEngine;

namespace Build.Component
{
    public class Projectile : MonoBehaviour
    {
        public int AttackPower;
        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("Projectile hit: " + col.gameObject.name);
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
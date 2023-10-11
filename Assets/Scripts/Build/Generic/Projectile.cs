using Build.Component;
using UnityEngine;

namespace Build.Generic
{
    public class Projectile : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            var targetHealth = col.GetComponent<HealthData>();

            if (targetHealth != null)
            {
                //apply damage to target
                targetHealth.Damage(gameObject.GetComponentInParent<AttackData>().AttackPower);  
                
                //destroy the projectile that caused the collision
                Destroy(gameObject);
            }
        }
    }
}
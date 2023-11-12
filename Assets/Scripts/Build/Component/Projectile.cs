using UnityEngine;

namespace Build.Component
{
    public class Projectile : MonoBehaviour
    {
        public int AttackPower = 5;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Projectile hit: " + collision.gameObject.name);

            var targetHealth = collision.gameObject.GetComponent<HealthData>();

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
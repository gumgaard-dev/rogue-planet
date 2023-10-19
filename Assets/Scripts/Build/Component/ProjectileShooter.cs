using Build.Component;
using Build.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public const float ProjectileForce = 5f;
    public GameObject projectilePrefab;
    public Transform spawnPoint;

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = this.transform;
            Debug.Log("ProjectileSpawner: No spawn point set.");
        }
        if (projectilePrefab == null)
        {
            Debug.Log("ProjectileSpawner: No projectile prefab set.");
        }
    }

    public void Shoot()
    {        
        var parentAttackPower = gameObject.GetComponentInParent<AttackData>().AttackPower;
        Projectile projectile_body = projectilePrefab.GetComponent<Projectile>();
        projectile_body.AttackPower = parentAttackPower;
        //prevents any potential error with projectiles spawning without a rigidbody
        if (rb != null)
        {
            //sets direction for projectile to travel
            Vector2 forceDirection = projectile.transform.right;
            rb.AddForce(forceDirection * ProjectileForce, ForceMode2D.Impulse);
        }
    }
}
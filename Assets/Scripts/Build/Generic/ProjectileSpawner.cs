using JetBrains.Annotations;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public const float ProjectileForce = 5f;
    public GameObject projectilePrefab;
    public Transform spawnPoint;

    private void Start()
    {
        if (spawnPoint == null)
        {
            Debug.Log("ProjectileSpawner: No spawn point set.");
        }
        if (projectilePrefab == null)
        {
            Debug.Log("ProjectileSpawner: No projectile prefab set.");
        }
    }

    public void Shoot()
    {
        
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        //prevents any potential error with projectiles spawning without a rigidbody
        if (rb != null)
        {
            //sets direction for projectile to travel
            Vector2 forceDirection = projectile.transform.right;
            rb.AddForce(forceDirection * ProjectileForce, ForceMode2D.Impulse);
        }
    }
}
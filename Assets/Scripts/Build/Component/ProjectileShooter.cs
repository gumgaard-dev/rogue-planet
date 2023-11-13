using Build.Component;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public float ProjectileForce = 5f;
    public GameObject projectilePrefab;
    public Transform spawnPoint;

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = this.transform;
            Debug.Log("ProjectileShooter: No spawn point set.");
        }
        if (projectilePrefab == null)
        {
            Debug.Log("ProjectileShooter: No projectile prefab set.");
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
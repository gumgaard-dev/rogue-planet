using Build.Component;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public Projectile projectilePrefab;
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

    public void Shoot(float forceAmount)
    {
        Projectile projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        //prevents any potential error with projectiles spawning without a rigidbody
        if (projectile != null)
        {
            projectile.AddImpulseForce(forceAmount * this.transform.right);
        }
    }
}
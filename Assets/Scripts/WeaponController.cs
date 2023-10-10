using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public const float ProjectileSpeed = 5f;
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;

    private void Update()
    {
        //rotate weapon based on user input
        RotateWeapon();

        //shoot projectiles
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void RotateWeapon()
    {
        float rotation = Input.GetAxis("Horizontal") * -rotationSpeed * Time.deltaTime;
        
        float currentRotation = transform.localRotation.eulerAngles.x;
        if (currentRotation > 180f)
            currentRotation -= 360f;
        
        float newRotation = currentRotation + rotation;
        float limitedRotation = Mathf.Clamp(newRotation, 0f, 180f);

        // Rotate the weapon around the spaceship's position
        Vector3 targetPosition = transform.parent.position;
        transform.RotateAround(targetPosition, Vector3.forward, limitedRotation - transform.eulerAngles.x);
    }

    private void Shoot()
    {
        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        var rb = projectile.GetComponent<Rigidbody2D>();

        //prevents any potential error with projectiles spawning without a rigidbody
        if (null != rb)
        {
            //sets direction for projectile to travel
            rb.velocity = projectile.transform.up * ProjectileSpeed;
        }
    }
}
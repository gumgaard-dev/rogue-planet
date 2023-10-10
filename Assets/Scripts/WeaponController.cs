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
        float newRotation = transform.eulerAngles.z + rotation;

        // limit the rotation to the top half (0 to 180 degrees)
        float limitedRotation = Mathf.Clamp(newRotation, 0f, 180f);
        Vector3 relativePosition = transform.localPosition;
        transform.localPosition = relativePosition;

        // rotate the weapon around the parent (empty GameObject)
        transform.RotateAround(transform.parent.position, Vector3.forward, limitedRotation - transform.eulerAngles.z);
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
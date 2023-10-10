using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public const float ProjectileSpeed = 7f;
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
        var rotation = Input.GetAxis("Horizontal") * -rotationSpeed * Time.deltaTime;
        //get the current rotation in the range [-180, 180]
        var currentRotation = transform.rotation.eulerAngles.x;
        if (currentRotation > 180f)
            currentRotation -= 360f;

        //add the rotation to the current rotation
        var newRotation = currentRotation + rotation;

        //limit the rotation to the top half (0 to 180 degrees)
        var limitedRotation = Mathf.Clamp(newRotation, 0f, 180f);
        transform.RotateAround(transform.parent.position, Vector3.forward, limitedRotation - transform.eulerAngles.x);
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
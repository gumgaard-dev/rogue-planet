using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    
    void Update()
    {
        //rotate weapon based on user input
        RotateWeapon();
        
        //shoot projectiles
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void RotateWeapon()
    {
        float rotation = Input.GetAxis("Horizontal") * -rotationSpeed * Time.deltaTime;

        // Calculate the new rotation angle
        float newRotation = transform.eulerAngles.z - rotation;

        // Limit the rotation to the top half (0 to 180 degrees)
        float limitedRotation = Mathf.Clamp(newRotation, 90f, 270f);

        // Calculate the rotation axis (up vector of the spaceship)
        Vector3 rotationAxis = transform.parent.up;

        // Calculate the position of the weapon relative to the spaceship
        Vector3 relativePosition = transform.localPosition;

        // Rotate the weapon around the spaceship's axis
        //transform.RotateAround(transform.parent.position, rotationAxis, limitedRotation - transform.eulerAngles.z);

        // Restore the relative position after rotation
        transform.localPosition = relativePosition;
        
        //float rotation = Input.GetAxis("Horizontal") * -rotationSpeed * Time.deltaTime;

        // Rotate the weapon around the parent (empty GameObject)
        transform.RotateAround(transform.parent.position, Vector3.forward, limitedRotation - transform.eulerAngles.z);
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation);
    }
}

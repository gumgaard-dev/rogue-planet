using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class player_controller_weapon : MonoBehaviour
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

        // Rotate the weapon around the parent (empty GameObject)
        transform.RotateAround(transform.parent.position, Vector3.forward, rotation);
    }

    void Shoot()
    {
        Debug.Log("Spawn position: " + projectileSpawnPoint.position);
        Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation);
    }
}

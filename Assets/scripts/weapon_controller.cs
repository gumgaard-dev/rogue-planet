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
        Vector3 spaceshipPosition = transform.parent.position;
        
        //calculate the relative position based on user input
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        
        //calculate the new position of the weapon
        float weaponX = spaceshipPosition.x + Mathf.Cos(rotation) * transform.localPosition.x;
        float weaponY = spaceshipPosition.y + Mathf.Sin(rotation) * transform.localPosition.x;
        
        //set the new position of the weapon
        transform.position = new Vector3(weaponX, weaponY, transform.position.z);
        
        //rotate the weapon to face the center of the spaceship
        transform.rotation = Quaternion.LookRotation(spaceshipPosition - transform.position, Vector3.forward);
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileShooter))]
public class Gun : MonoBehaviour
{

    public float shotInterval;
    private ParticleShooter particleShooter;
    private ProjectileShooter projectileShooter;

    private void Start()
    {
        this.projectileShooter = GetComponent<ProjectileShooter>();
        this.particleShooter = GetComponent<ParticleShooter>();
    }
    public void Shoot()
    {
        particleShooter.Shoot();
        projectileShooter.Shoot();
    }
}

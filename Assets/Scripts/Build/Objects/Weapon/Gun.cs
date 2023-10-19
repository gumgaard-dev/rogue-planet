using System.Collections;
using System.Collections.Generic;
using Build.Component;
using UnityEngine;

[RequireComponent(typeof(ProjectileShooter))]
[RequireComponent(typeof(AttackData))]
public class Gun : MonoBehaviour
{
    public float shotInterval;
    private ParticleShooter particleShooter;
    private ProjectileShooter projectileShooter;
    private AttackData _attackData;
    
    private void Start()
    {
        this.projectileShooter = GetComponent<ProjectileShooter>();
        this.particleShooter = GetComponent<ParticleShooter>();
        this._attackData = GetComponent<AttackData>();
    }
    public void Shoot()
    {
        particleShooter.Shoot();
        projectileShooter.Shoot();
    }
}
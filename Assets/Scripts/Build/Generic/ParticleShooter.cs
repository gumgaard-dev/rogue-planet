using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShooter : MonoBehaviour
{
    public OneShotParticles[] shotParticleEffects;
    public Transform shotOrigin;

    public void Shoot()
    {
        if (shotParticleEffects.Length > 0)
        {
            foreach (OneShotParticles particleSystem in shotParticleEffects)
            {
                Instantiate(particleSystem, shotOrigin.position, this.transform.rotation);
            }
        }
    }
}

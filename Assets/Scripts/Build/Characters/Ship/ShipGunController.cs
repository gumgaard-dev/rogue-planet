using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipController : MonoBehaviour
{

    private const float ROTATION_INPUT_MOD = -1f;

    public HingeRotator rotator;
    public ProjectileSpawner gun;
    public float shotInterval = 90f;

    private float rotationInput;
    private bool shootInput;
    private Cooldown shotCooldown;
    public ParticleSystem shotSparks;
    public Transform spawnPoint;
    

    // Start is called before the first frame update
    void Start()
    {
        shotCooldown = new Cooldown(shotInterval);

        if (gun == null)
        {
            DebugLog.ComponentMissing("ShipGun", "Projectile Spawner");
        }

        if (rotator == null)
        {
            DebugLog.ComponentMissing("ShipGun", "Orbit Parent");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PollInput();
        if(rotationInput != 0f)
        {
            // direction is the opposite of input, so that right=clockwise, left=counter
            float rotationDirection = rotationInput * ROTATION_INPUT_MOD;
            rotator.Rotate(rotationDirection);
        }

        if(shootInput && shotCooldown.IsAvailable())
        {
            gun.Shoot();
            Instantiate(shotSparks, spawnPoint.position, spawnPoint.rotation);
            shotCooldown.Activate();
        }
    }

    private void PollInput()
    {
        rotationInput = Input.GetAxis("Horizontal");
        shootInput = Input.GetKey(KeyCode.Space);
    }
}

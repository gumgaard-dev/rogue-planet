using UnityEngine;

[RequireComponent(typeof(Gun))]
public class GunController : MonoBehaviour
{
    private bool shootInput;
    private Cooldown shotCooldown;
    private Gun gun;


    void Start()
    {
        gun = GetComponent<Gun>();
        shotCooldown = new Cooldown(gun.shotInterval);
    }


    void Update()
    {
        PollInput();
        if (shootInput && shotCooldown.IsAvailable())
        {
            gun.Shoot();
            shotCooldown.Activate();
        }
    }


    void PollInput()
    {
        shootInput = Input.GetKey(KeyCode.Space);
    }
}

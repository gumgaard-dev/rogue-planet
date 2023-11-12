using Build.Component;
using UnityEngine;

[RequireComponent(typeof(ProjectileShooter))]
public class Gun : MonoBehaviour
{
    [SerializeField]
    private float _shotInterval;
    
    [SerializeField]
    private ParticleShooter _particleShooter;
    [SerializeField]
    private ProjectileShooter _projectileShooter;
    private Cooldown _shotCooldown;

    private void Start()
    {
        TryGetComponent<ProjectileShooter>(out this._projectileShooter);
        TryGetComponent<ParticleShooter>(out this._particleShooter);

        this._shotCooldown = new Cooldown(_shotInterval);
        _shotCooldown.Activate();
    }
    public void Shoot()
    {
        if (_shotCooldown.IsAvailable())
        {
            _particleShooter.Shoot();
            _projectileShooter.Shoot();
            _shotCooldown.Activate();
        }
    }
}
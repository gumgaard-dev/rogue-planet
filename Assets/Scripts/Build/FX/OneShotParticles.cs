using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class OneShotParticles : MonoBehaviour
{
    private ParticleSystem _particles;

    private void Start()
    {
        _particles = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        Instantiate(_particles, this.transform.position, Quaternion.identity);
        _particles.Play();
    }

    private void Update()
    {
        if (!_particles.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}

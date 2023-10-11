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

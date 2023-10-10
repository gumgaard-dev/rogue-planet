using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleOneShot : MonoBehaviour
{
    private ParticleSystem _particles;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!_particles.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}

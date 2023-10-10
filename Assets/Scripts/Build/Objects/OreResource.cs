using UnityEngine;

public class Resource : MonoBehaviour
{
    public enum ResourceType
    {
        GREEN, RED
    }

    public ResourceType type;
    public ParticleSystem pickupParticles;

    private void Start()
    {
        if (pickupParticles == null)
        {
            Debug.LogError("Please assign a Particle System to the pickupParticles field!");
            return;
        }

        // Set particle color based on resource type
        ParticleSystem.MainModule main = pickupParticles.main;
        switch (type)
        {
            case ResourceType.GREEN:
                main.startColor = Color.green;
                break;
            case ResourceType.RED:
                main.startColor = Color.red;
                break;
        }
    }

    public void PickUp()
    {
        // Spawn the particle effect at the resource's position
        ParticleSystem effect = Instantiate(pickupParticles, transform.position, Quaternion.identity);

        // Get the color of the resource sprite and apply it to the particle system
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        ParticleSystem.MainModule mainModule = effect.main;
        mainModule.startColor = sr.color;

        Destroy(this.gameObject);
    }
}

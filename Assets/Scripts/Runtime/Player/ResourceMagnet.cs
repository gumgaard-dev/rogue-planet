using UnityEngine;

public class ResourceMagnet : MonoBehaviour
{
    public float magnetRadius = 5f;  // The radius within which resources will be attracted
    public float magnetStrength = 2f;  // The speed at which resources will move towards the player
    public LayerMask resourceLayer;  // The layer of the resources

    void Update()
    {
        AttractResources();
    }

    void AttractResources()
    {
        // Detect resources within the magnet's radius
        Collider2D[] resourcesToAttract = Physics2D.OverlapCircleAll(transform.position, magnetRadius, resourceLayer);
        foreach (Collider2D resourceCollider in resourcesToAttract)
        {
            // Move the resource towards the player
            resourceCollider.transform.position = Vector2.MoveTowards(resourceCollider.transform.position, transform.position, magnetStrength * Time.deltaTime);
        }
    }
}
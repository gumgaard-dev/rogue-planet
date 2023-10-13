using UnityEngine;

public class ObjectMagnet : MonoBehaviour
{
    public float magnetRadius = 5f;  // The radius within which resources will be attracted
    public float magnetStrength = 2f;  // The strength of the attraction force
    public LayerMask targetLayer;  // The layer of the resources

    void Update()
    {
        AttractResources();
    }

    void AttractResources()
    {
        // Detect target objects within the magnet's radius
        Collider2D[] objectsToAttract = Physics2D.OverlapCircleAll(transform.position, magnetRadius, targetLayer);
        foreach (Collider2D objCollider in objectsToAttract)
        {
            Rigidbody2D rb = objCollider.GetComponent<Rigidbody2D>();
            if (rb != null)  // Make sure the object has a Rigidbody2D
            {
                // Calculate the object's displacement from the magnet
                Vector2 magnetToObject = (Vector2)transform.position - rb.position;

                // get the individual components
                float distanceToMagnet = magnetToObject.magnitude;
                Vector2 directionToObject = magnetToObject.normalized;

                // Normalize the direction and apply the force based on distance and magnetStrength
                Vector2 force = directionToObject * (magnetStrength / distanceToMagnet);
                rb.AddForce(force);
            }
        }
    }
}

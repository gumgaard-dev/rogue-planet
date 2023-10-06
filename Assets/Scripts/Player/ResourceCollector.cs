using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    private Inventory playerInventory;  // Reference to the player's inventory

    void Start()
    {
        playerInventory = this.transform.parent.GetComponent<Inventory>();
    }

    // This assumes the player has a 2D Collider set to "Trigger"
    void OnTriggerEnter2D(Collider2D other)
    {
        Resource resourceComponent = other.GetComponent<Resource>();
        if (resourceComponent != null)
        {
            playerInventory.addToStorage(resourceComponent.type);
            resourceComponent.PickUp();
        }
    }
}

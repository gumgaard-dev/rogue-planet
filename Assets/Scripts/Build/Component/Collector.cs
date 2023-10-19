using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Inventory))]
public class Collector : MonoBehaviour
{
    public Inventory inventory;

    private void Start()
    {
        if(inventory == null)
        {
            Debug.LogWarning("No inventory attached to ObjectCollector");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered by: " + other.gameObject.name);
        // check if collider is a collectable
        if (other.TryGetComponent<ICollectable>(out var collectable))
        {
            Debug.Log("CollectableObject Detected");

            Collect(collectable);
        }
    }

    private void Collect(ICollectable collectable)
    {
        // add to inventory
        inventory.AddToStorage(collectable.GetItemType());

        // call collect method on collectable
        collectable.Collected();
    }
}

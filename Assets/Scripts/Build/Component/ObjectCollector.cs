using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ObjectCollector : MonoBehaviour
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
        CollectableObject collectableObject = other.GetComponent<CollectableObject>();
        if (collectableObject != null)
        {
            Debug.Log("CollectableObject Detected");
            // ensure collectable has a data component
            ICollectable collectableItemData = collectableObject.GetComponent<ICollectable>();
            if (collectableItemData != null)
            {
                Debug.Log("Collecting Item");
                // get type from data component
                object itemType = collectableItemData.GetItemType();

                // add to inventory
                inventory.AddToStorage(itemType);

                // call collect method on collectable
                collectableObject.OnCollect();
            }

        }
    }
}

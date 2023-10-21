using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemCollectedEvent : UnityEvent<ICollectable> { }

[RequireComponent(typeof(Collider2D))]
public class Collector : MonoBehaviour
{
    public ItemCollectedEvent ItemCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered by: " + other.gameObject.name);
        // check if collider is a collectable
        if (other.TryGetComponent<ICollectable>(out var collectable))
        {
            Debug.Log("CollectableObject Detected");
            ItemCollected.AddListener(collectable.Collected);

            ItemCollected?.Invoke(collectable);
        }
    }
}

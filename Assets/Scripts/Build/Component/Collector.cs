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
        if (other.TryGetComponent<ICollectable>(out var collectable))
        {
            ItemCollected.AddListener(collectable.Collected);

            ItemCollected?.Invoke(collectable);
        }
    }
}

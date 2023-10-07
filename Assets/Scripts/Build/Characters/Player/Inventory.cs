using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Dictionary<Resource.ResourceType, int> storage;

    private void Start()
    {
        this.storage = new();
    }
    public void addToStorage(Resource.ResourceType type)
    {
        if (!storage.ContainsKey(type))
        {
            storage[type] = 0;
        }
        storage[type] += 1;
    }

    public void removeFromStorage(Resource.ResourceType type)
    {
        if (!storage.ContainsKey(type))
        {
            return; // Nothing to remove
        }
        storage[type] -= 1;
        if (storage[type] <= 0)
        {
            storage.Remove(type); // If there are no more resources of this type, remove the key
        }
    }
}

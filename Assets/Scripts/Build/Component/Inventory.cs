using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class InventoryChangedEvent : UnityEvent<Dictionary<object, int>> { }

[System.Serializable]
public class Inventory : MonoBehaviour
{
    public static List<OreCollectable> AllOreCollectibles;
    public Dictionary<object, int> storage = new();
    public InventoryChangedEvent InventoryChanged;

    [Header("Optional Capacity Setting. Set Max to -1 for infinite.")]
    public int MaxCapacity;
    public int CurCapacity => storage.Values.Sum();
    public bool IsFull => MaxCapacity >= 0 && CurCapacity == MaxCapacity;

    private void Start()
    {
        foreach (OreCollectable o in ItemDB.GetOreList())
        {
            storage[o.GetItemType()] = 3;
        }

        InvokeInventoryChanged();
    }

    protected virtual void UpdateStorage(object itemType, int amount)
    { 
        storage[itemType] += amount;

        InvokeInventoryChanged();
    }

    protected virtual void UpdateStorage(ICollectable item, int amount)
    {
        UpdateStorage(item.GetItemType(), amount);
    }

    public virtual void AddToStorage(ICollectable item)
    {
        UpdateStorage(item, 1);
    }

    public virtual void AddToStorage(object itemType)
    {
        UpdateStorage(itemType, 1);
    }

    public virtual void RemoveFromStorage(ICollectable item)
    {
        UpdateStorage(item, -1);
    }

    public virtual void RemoveFromStorage(object itemType)
    {
        UpdateStorage(itemType, -1);
    }

    public void AddToStorage(ICollectable item, int amount)
    {
        UpdateStorage(item, amount);
    }

    public void AddToStorage(object itemType, int amount)
    {
        UpdateStorage(itemType, amount);
    }

    public virtual void RemoveFromStorage(ICollectable item, int amount)
    {
        UpdateStorage(item, -1 * amount);
    }

    public virtual void RemoveFromStorage(object itemType, int amount)
    {
        UpdateStorage(itemType, -1 * amount);
    }
    private void InvokeInventoryChanged()
    {
        if (storage != null)
        {
            InventoryChanged?.Invoke(this.storage);
        }
    }
}

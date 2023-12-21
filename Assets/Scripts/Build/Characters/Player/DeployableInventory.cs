using Capstone.Build.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DeployableInventoryChangedEvent : UnityEvent<Dictionary<object, int>> { }

public class DeployableInventory : MonoBehaviour
{
    private Transform _playerTransform;
    private Dictionary<string, int> _storageAmount = new();
    
    public List<GameObject> itemPrefabs;

    private Dictionary<string, GameObject> _itemDictionary = new();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in itemPrefabs)
        {
            _itemDictionary[item.tag] = item;
            UpdateStorage(item.tag, 1000);
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = player.transform;
    }

    private void UpdateStorage(string objectTag, int amount)
    {
        // don't update storage if we don't have an entry for that object
        if (!_itemDictionary.ContainsKey(objectTag))
        {
            return;
        }


        if (!_storageAmount.ContainsKey(objectTag))
        {
            _storageAmount[objectTag] = 0;
        }

        _storageAmount[objectTag] += amount;

        if (_storageAmount[objectTag] <= 0)
        {
            _storageAmount.Remove(objectTag);
        }
    }
    
    // use this overload to add new objects to storage
    protected virtual void UpdateStorage(GameObject item, int amount)
    {

        string objectTag = item.tag;

        if (!_itemDictionary.ContainsKey(objectTag))
        {
            GameObject clone = Instantiate(item, this.transform);

            this._itemDictionary[objectTag] = clone;
            clone.SetActive(false);
        }

        UpdateStorage(objectTag, amount);
    }

    private void AddToStorage(string objectTag)
    {
        UpdateStorage(objectTag, 1);
    }

    public void AddToStorage(GameObject item)
    {
        UpdateStorage(item, 1);
    }

    public void PlaceDeployable(string objectTag)
    {

        Debug.Log(objectTag);
        GameObject item = GetFromStorage(objectTag);
        foreach (string key in _itemDictionary.Keys)
        {
            Debug.Log(key);
            Debug.Log(_storageAmount[key]);
        }

        if (item != null)
        {
            Debug.Log("Got item");
            Debug.Log(_storageAmount[objectTag]);
            Instantiate(item, _playerTransform.position, Quaternion.identity);
        }
    }

    private GameObject GetFromStorage(string objectTag)
    {
        _storageAmount.TryGetValue(objectTag, out int numberInStorage);
        
        if (numberInStorage <= 0)
        {
            return null;
        }


        _itemDictionary.TryGetValue(objectTag, out GameObject item);

        if (item == null)
        {
            return null;
        }

        UpdateStorage(objectTag, -1);

        return item;
    }
}

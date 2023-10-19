using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class InventoryDisplay : MonoBehaviour
{
    public Inventory inventory; // Reference to the Inventory script
    public GameObject inventoryUI; // GameObject that will hold all the UI elements
    public Text itemTemplate; // A text template for items to display

    private void Start()
    {
        // Subscribe to the onInventoryChanged event
        inventory.onInventoryChanged.AddListener(UpdateUI);
    }

    private void OnDestroy()
    {
        // Unsubscribe when this GameObject is destroyed to prevent memory leaks
        inventory.onInventoryChanged.RemoveListener(UpdateUI);
    }

    public void UpdateUI()
    {
        // Iterate through the storage dictionary and create UI elements for each 
        string inventoryText = "";
        foreach (var entry in inventory.storage)
        {
            inventoryText += $"{entry.Key}: {entry.Value}\n";
        }

        itemTemplate.text = inventoryText;
    }
}


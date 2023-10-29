using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class InventoryDisplay : MonoBehaviour
{
    public GameObject inventoryUI; // GameObject that will hold all the UI elements
    public Text itemTemplate; // A text template for items to display

    public void UpdateUI(Dictionary<object, int> storage)
    {
        // Iterate through the storage dictionary and create UI elements for each 
        string inventoryText = "";
        foreach (var entry in storage)
        {
            object itemName = entry.Key;
            int itemCount = entry.Value;
            inventoryText += $"{itemName}: {itemCount}\n";
        }

        itemTemplate.text = inventoryText;
    }
}


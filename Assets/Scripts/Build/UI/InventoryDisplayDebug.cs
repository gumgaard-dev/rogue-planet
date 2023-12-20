using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public Text itemTemplate; // A text template for items to display

    public void UpdateUI(Dictionary<object, int> storage)
    {

        Debug.Log("Updating Inventory UI");
        // Iterate through the storage dictionary and create UI elements for each 
        string inventoryText = "";
        foreach (var entry in storage)
        {
            object itemName = entry.Key;
            int itemCount = entry.Value;
            inventoryText += $"{itemName}: {itemCount}\n";
        }

        Debug.Log(inventoryText);

        itemTemplate.text = inventoryText;
    }
}


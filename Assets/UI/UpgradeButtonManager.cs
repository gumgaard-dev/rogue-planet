using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UpgradeMenuController))]
public class UpgradeDataManager : MonoBehaviour
{
    public List<BuyItemUpgradeData> BuyItemUpgradeData = new();
    public List<ItemUpgradeData> ItemUpgradeData = new();
    public List<ShipHealthUpgradeData> ShipUpgradeData = new();
    public List<SuitUpgradeData> SuitUpgradeData = new();

    private void Start()
    {
        foreach (var u in BuyItemUpgradeData)
        {
            UpgradeMenuController.AddButtonFor(u);
        }

        foreach (var u in ItemUpgradeData)
        {
            UpgradeMenuController.AddButtonFor(u);
        }

        foreach (var u in ShipUpgradeData)
        {
            UpgradeMenuController.AddButtonFor(u);
        }

        foreach (var u in SuitUpgradeData) 
        {
            UpgradeMenuController.AddButtonFor(u);
        }
    }
}

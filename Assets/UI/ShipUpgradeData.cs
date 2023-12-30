using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Upgrade/ShipUpgrade")]

public class ShipUpgradeData : UpgradeData
{
    public ShipUpgradeData()
    {
        base.UpgradeCategory = UpgradeCategory.SHIP;
    }

    public override void UpgradeEffect()
    {
        
    }
}

using Build.Component;
using Capstone.Build.Characters.Ship;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Upgrade/ShipHealthUpgrade")]

public class ShipHealthUpgradeData : UpgradeData
{
    public ShipHealthUpgradeData()
    {
        base.UpgradeCategory = UpgradeCategory.SHIP;
    }

    public override void ApplyUpgradeEffect()
    {
        FindObjectOfType<Ship>().GetComponent<HealthData>().IncreaseMaxHealthBy(10);
    }
}

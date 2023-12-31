using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Upgrade/ItemUpgrade")]
public class ItemUpgradeData : UpgradeData
{
    public ItemUpgradeData()
    {
        base.UpgradeCategory = UpgradeCategory.ITEM;
    }

    public override void ApplyUpgradeEffect()
    {
    }
}

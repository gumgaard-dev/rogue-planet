using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Upgrade/SuitUpgrade")]

public class SuitUpgradeData : UpgradeData
{
    public SuitUpgradeData()
    {
        base.UpgradeCategory = UpgradeCategory.SUIT;
    }

    public override void UpgradeEffect()
    {

    }
}

using Capstone.Build.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Upgrade/BuyItemUpgrade")]
public class BuyItemUpgradeData : UpgradeData
{
    public GameObject item;

    public BuyItemUpgradeData()
    {
        base.UpgradeCategory = UpgradeCategory.ITEM;
    }

    public override void UpgradeEffect()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddToDeployableInventory(item);
    }
}

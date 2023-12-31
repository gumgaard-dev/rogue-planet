using Capstone.Build.Characters.Player;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 

[CreateAssetMenu(menuName = "Custom/Upgrade/BuyItemUpgrade")]
public class BuyItemUpgradeData : UpgradeData
{
    public GameObject item;

    public BuyItemUpgradeData()
    {
        base.UpgradeCategory = UpgradeCategory.ITEM;
    }

    public override void ApplyUpgradeEffect()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddToDeployableInventory(item);
    }
}

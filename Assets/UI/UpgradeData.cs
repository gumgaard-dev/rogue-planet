using Capstone.Build.Characters.Ship;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public enum UpgradeCategory
{
    SUIT,
    SHIP,
    ITEM
}
public abstract class UpgradeData : ScriptableObject
{
    public string LabelText;
    public string Tooltip;
    public Sprite Icon;
    public OreCollectable.ResourceType CostType;
    public UpgradeCategory UpgradeCategory;
    public int CostAmount;

    public Action PurchaseUpgrade;

    public UpgradeData()
    {
        PurchaseUpgrade = () =>
        {
            FindFirstObjectByType<Ship>().GetComponent<Inventory>().RemoveFromStorage(CostType, CostAmount);
        };

        PurchaseUpgrade += UpgradeEffect;
    }

    public abstract void UpgradeEffect();
}
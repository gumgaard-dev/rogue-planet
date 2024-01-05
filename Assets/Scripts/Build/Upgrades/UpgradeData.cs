using Capstone.Build.Characters.Ship;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public void PurchaseUpgrade()
    {
        FindFirstObjectByType<Ship>().GetComponent<Inventory>().RemoveFromStorage(CostType, CostAmount);

        ApplyUpgradeEffect();
    }

    // use this method to invoke apply upgrade-specific effects
    public abstract void ApplyUpgradeEffect();
}
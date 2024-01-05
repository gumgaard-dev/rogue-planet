using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class UpgradeButton : VisualElement
{
    [SerializeField] public UpgradeData UpgradeData;

    private const string TEMPLATE_PATH = "UI/UpgradeButton.uxml";

    public OreCollectable.ResourceType CostType => UpgradeData.CostType;
    public int CostAmount => UpgradeData.CostAmount;

    private VisualElement _upgradeButtonBase;
    private Button _upgradeButtonElement;
    private Label _label => _upgradeButtonBase.Q<Label>(className: "UpgradeLabel");
    private VisualElement _upgradeIcon => _upgradeButtonBase.Q<VisualElement>(className: "UpgradeIcon");
    private Label _costAmountLabel => _upgradeButtonBase.Q<Label>(className: "CostAmount");
    private VisualElement _costTypeIcon => _upgradeButtonBase.Q(className: "CostTypeIcon");

    public UpgradeButton()
    {
        VisualTreeAsset upgradeButtonAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/UpgradeButton.uxml");
        this._upgradeButtonBase = upgradeButtonAsset.CloneTree();
        this._upgradeButtonElement = _upgradeButtonBase.Query<Button>().ToList().FirstOrDefault();

        this.Add(_upgradeButtonBase); // adding as child
    }

    public void SetUpgradeData(UpgradeData newUpgradeData)
    {

        if (newUpgradeData == null)
        {
            Debug.LogWarning("attempted to set null UpgradeData on UpgradeButton");
            return;
        }

        this.UpgradeData = newUpgradeData;

        _upgradeButtonElement.clickable.clicked += UpgradeData.PurchaseUpgrade;
        SetUpgradeLabel(newUpgradeData.LabelText);

        if (newUpgradeData.Icon != null) { SetUpgradeIcon(newUpgradeData.Icon); }

        SetCostTypeIcon(ItemDB.GetSpriteForOreType(newUpgradeData.CostType));
        SetCostAmount(newUpgradeData.CostAmount);
    }

    private void SetUpgradeIcon(Sprite icon)
    {
        if (icon != null)
        {
            this._upgradeIcon.style.backgroundImage = new StyleBackground(icon);
        }
    }

    public void SetCostTypeIcon(Sprite icon)
    {
        if (icon != null)
        {
            this._costTypeIcon.style.backgroundImage = new StyleBackground(icon);
        }
    }

    public void SetCostAmount(int amount)
    {
        this._costAmountLabel.text = amount.ToString();
    }

    public void SetUpgradeLabel(string labelText)
    {
        if (labelText != null) { this._label.text = labelText; }
    }

    public void AddClickAction(Action a)
    {
        this._upgradeButtonElement.clickable.clicked += a;
    }
    #region UXML

    [Preserve]
    public new class UxmlFactory : UxmlFactory<UpgradeButton, UxmlTraits> { }

    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
        }
    }
    #endregion
}

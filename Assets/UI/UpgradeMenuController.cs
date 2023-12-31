using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeMenuController : MonoBehaviour
{
    public static UpgradeMenuController Instance { get; private set; }

    public UIDocument doc;
    public VisualElement root;
    public VisualElement _upgradeMainPanel;
    public List<UpgradeButton> UpgradeButtons = new();
    public Dictionary<object, Label> oreDisplays;
    private Dictionary<UpgradeCategory, VisualElement> _upgradeCategoryButtonContainers = new();
    private TabPanel _tabPanel;

    private Label _greenOreLabel;
    private Label _redOreLabel;

    public static bool MenuOpen => Instance != null && Instance._upgradeMainPanel != null && Instance._upgradeMainPanel.style.display != DisplayStyle.None;

    void Awake()
    {

        // Check if an instance already exists and if it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _tabPanel = GetComponentInChildren<TabPanel>();
        _tabPanel.TabChanged.AddListener(FocusFirst);
        root = doc.rootVisualElement;

        _upgradeCategoryButtonContainers[UpgradeCategory.ITEM] = root.Query(name: "ItemsTabView").ToList().First();
        _upgradeCategoryButtonContainers[UpgradeCategory.SHIP] = root.Query(name: "ShipTabView").ToList().First();
        _upgradeCategoryButtonContainers[UpgradeCategory.SUIT] = root.Query(name: "SuitTabView").ToList().First();

        _upgradeMainPanel = root.Query<VisualElement>(name: "UpgradeMenuPanelBase");

        _greenOreLabel = root.Query<Label>(name: "GreenOreLabel");
        _redOreLabel = root.Query<Label>(name: "RedOreLabel");

        if (MenuOpen)
        {
            HideMenu();
        }
    }

    private void HideMenu()
    {
        _upgradeMainPanel.style.display = DisplayStyle.None;
    }

    private void ShowMenu()
    {
        _upgradeMainPanel.style.display = DisplayStyle.Flex;
        UpgradeMenuController.RecheckButtonsEnabledState();
        _tabPanel.SelectDefaultTab();
        FocusFirst();
    }

    private static void FocusFirst()
    {
        var curTab = Instance._tabPanel.CurrentTabView;

        var first = curTab.Query(className: "UpgradeButton").ToList().FirstOrDefault();

        if (first != null) { first.Focus(); }
    }

    public static void OpenUpgradeMenu()
    {
        Instance.ShowMenu();
    }

    public static void CloseUpgradeMenu()
    {
        Instance.HideMenu();
    }

    public void OnResourceInventoryChanged(Dictionary<object, int> storage)
    {
        if (_greenOreLabel != null)
        {
            _greenOreLabel.text = storage[OreCollectable.ResourceType.GREEN].ToString();
        }

        if (_redOreLabel != null)
        {
            _redOreLabel.text = storage[OreCollectable.ResourceType.RED].ToString();
        }

        RecheckButtonsEnabledState();
    }

    public static void CursorMoved(Vector2 cursorMovementDirection)
    {

    }

    public static void ConfirmPressed()
    {

    }

    public static bool BackPressed()
    {

        // logic will be updated if more complex menus are in place
        if (MenuOpen)
        {
            return true;
        }

        return false;
    }

    public static void AddButtonFor(UpgradeData upgrade)
    {
        UpgradeButton ub = new();

        ub.SetUpgradeData(upgrade);

        ub.AddClickAction(RecheckButtonsEnabledState);

        bool tabFound = Instance._upgradeCategoryButtonContainers.TryGetValue(upgrade.UpgradeCategory, out var targetTabView);

        if (!tabFound)
        {
            Debug.LogWarning("Couldn't find upgrade tab for upgrade " + upgrade.LabelText + ", where type=" + upgrade.UpgradeCategory.ToString());
            return;
        }

        var nextOpenRow = targetTabView.Query(className: "UpgradeMenuRow").ToList().LastOrDefault();

        if (nextOpenRow.childCount == 4)
        {
            nextOpenRow = new();
            nextOpenRow.AddToClassList("UpgradeMenuRow");
            targetTabView.Add(nextOpenRow);
        }

        nextOpenRow.Add(ub);

        Instance.UpgradeButtons.Add(ub);
    }

    public static void RecheckButtonsEnabledState()
    {
        foreach (UpgradeButton ub in Instance.UpgradeButtons)
        {
            var CostType = ub.UpgradeData.CostType;
            int curResourceCount = GetCurrentResourceCount(CostType);

            ub.SetEnabled(curResourceCount >= ub.UpgradeData.CostAmount);
        }
    }

    public static int GetCurrentResourceCount(OreCollectable.ResourceType resourceType)
    {
        switch (resourceType)
        {
            case OreCollectable.ResourceType.GREEN:
                return int.Parse(Instance._greenOreLabel.text);
            case OreCollectable.ResourceType.RED:
                return int.Parse(Instance._redOreLabel.text);
            default:
                return 0;
        }
    }
}
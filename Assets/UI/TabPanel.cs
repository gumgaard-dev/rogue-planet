using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

//Inherits from class `MonoBehaviour`. This makes it attachable to a game object as a component.
public class TabPanel : MonoBehaviour
{
    private VisualElement root;
    private List<VisualElement> TabButtons;
    private List<VisualElement> TabViews;

    public UnityEvent TabChanged;
    public VisualElement CurrentTabView => TabViews[currentTabIndex];

    // stored as tabButton: tabView
    private Dictionary<VisualElement, VisualElement> TabDictionary = new();
    int currentTabIndex;

    public UIInputActions UIInputActions;

    public const string TAB_BUTTON_CLASS = "TabButton";
    public const string ACTIVE_TAB_BUTTON_CLASS = "activeTabButton";
    public const string TAB_VIEW_CLASS = "TabView";
    public const string ACTIVE_TAB_VIEW_CLASS = "activeTabView";
    public const string INACTIVE_TAB_VIEW_CLASS = "inactiveTabView";

    public Color ActiveTabBackgroundColor;
    private void Start()
    {
        if (TryGetComponent(out UIDocument menu))
        {
            root = menu.rootVisualElement;

            TabButtons = root.Query<VisualElement>(className: TAB_BUTTON_CLASS).ToList();
            TabViews = root.Query<VisualElement>(className: TAB_VIEW_CLASS).ToList();
            
            for (int i = 0; i < TabButtons.Count && i < TabViews.Count; i++)
            {
                TabDictionary[TabButtons[i]] = TabViews[i];
                TabButtons[i].focusable = false;
            }
        }
    }

    private void OnEnable()
    {
        if (UIInputActions == null)
        {
            UIInputActions = new();
        }
        UIInputActions.Enable();
        UIInputActions.UI.SwitchTabLeft.performed += _ => SelectTabToLeft();
        UIInputActions.UI.SwitchTabRight.performed += _ => SelectTabToRight();
    }

    private void OnDisable()
    {
        UIInputActions.UI.SwitchTabLeft.performed -= _ => SelectTabToLeft();
        UIInputActions.UI.SwitchTabRight.performed -= _ => SelectTabToRight();
        UIInputActions.Disable();
    }

    private void SelectTabToRight()
    {
        currentTabIndex += 1;
        if (currentTabIndex >= TabButtons.Count)
        {
            currentTabIndex = 0;
        }

        VisualElement rightTabButton = TabButtons[currentTabIndex];
        SelectTab(rightTabButton);

    }

    private void SelectTabToLeft()
    {
        currentTabIndex -= 1;
        if (currentTabIndex < 0)
        {
            currentTabIndex = TabButtons.Count - 1;
        }

        VisualElement leftTabButton = TabButtons[currentTabIndex];
        SelectTab(leftTabButton);
    }

    public void SelectDefaultTab()
    {
        currentTabIndex = 0;
        SelectTab(TabButtons[0]);
    }

    private void SelectTab(VisualElement tabButton)
    {
        HideAllTabs();

        tabButton.AddToClassList(ACTIVE_TAB_BUTTON_CLASS);

        var curTabView = TabDictionary[tabButton];

        SetTabViewActive(curTabView);

        TabChanged?.Invoke();
    }
        
    private void HideAllTabs()
    {
        foreach(var tabPair in TabDictionary)
        {
            VisualElement tabButton = tabPair.Key;
            VisualElement tabView = tabPair.Value;

            tabButton.RemoveFromClassList(ACTIVE_TAB_BUTTON_CLASS);
            tabView.RemoveFromClassList(ACTIVE_TAB_VIEW_CLASS);
            tabView.AddToClassList(INACTIVE_TAB_VIEW_CLASS);
        }
    }

    private void SetTabViewActive(VisualElement tabView)
    {
        if (tabView == null) { return; }

        tabView.RemoveFromClassList(INACTIVE_TAB_VIEW_CLASS);
        tabView.AddToClassList(ACTIVE_TAB_VIEW_CLASS);
    }
}
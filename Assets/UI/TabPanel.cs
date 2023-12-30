using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Inherits from class `MonoBehaviour`. This makes it attachable to a game object as a component.
public class TabPanel : MonoBehaviour
{
    private VisualElement root;
    private List<VisualElement> TabButtons;
    private List<VisualElement> TabViews;
    private Dictionary<VisualElement, VisualElement> TabDictionary = new();

    private const string TAB_BUTTON_CLASS = "TabButton";
    private const string ACTIVE_TAB_BUTTON_CLASS = "activeTabButton";
    private const string TAB_VIEW_CLASS = "TabView";
    private const string ACTIVE_TAB_VIEW_CLASS = "activeTabView";
    private const string INACTIVE_TAB_VIEW_CLASS = "inactiveTabView";

    public Color ActiveTabBackgroundColor;
    private void OnEnable()
    {
        if (TryGetComponent(out UIDocument menu))
        {
            root = menu.rootVisualElement;

            TabButtons = root.Query<VisualElement>(className: TAB_BUTTON_CLASS).ToList();
            TabViews = root.Query<VisualElement>(className: TAB_VIEW_CLASS).ToList();

            if (TabButtons.Count > 0 && TabViews.Count > 0)
            {
                for (int i = 0; i < TabButtons.Count && i < TabViews.Count; i++)
                {
                    TabDictionary[TabButtons[i]] = TabViews[i];
                }

                foreach (var tabButton in TabButtons)
                {
                    tabButton.RegisterCallback<ClickEvent>(OnTabButtonClick);
                    tabButton.RegisterCallback<FocusEvent>(OnTabButtonFocus);
                }

                TabButtons[0].Focus();
            }
        }
    }

    private void OnTabButtonFocus(FocusEvent e)
    {
        SelectTab(e.currentTarget as VisualElement);
    }

    private void OnTabButtonClick(ClickEvent e)
    {
        SelectTab(e.currentTarget as VisualElement);
    }

    private void SelectTab(VisualElement tabButton)
    {
        HideAllTabs();

        tabButton.AddToClassList(ACTIVE_TAB_BUTTON_CLASS);

        var curTabView = TabDictionary[tabButton];

        curTabView.RemoveFromClassList(INACTIVE_TAB_VIEW_CLASS);
        curTabView.AddToClassList(ACTIVE_TAB_VIEW_CLASS);
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

    private void SetTabViewActiveClass(VisualElement tabView)
    {
        if (tabView == null) { return; }

        tabView.RemoveFromClassList(INACTIVE_TAB_VIEW_CLASS);
        tabView.AddToClassList(ACTIVE_TAB_VIEW_CLASS);
    }
}
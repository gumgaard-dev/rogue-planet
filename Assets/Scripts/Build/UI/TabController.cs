using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using Capstone.Build.UI;

public class TabController : MonoBehaviour
{
    public GameObject TabButtonContainer;
    private Dictionary<TabBtn, GameObject> tabDictionary = new();
    // Start is called before the first frame update
    void Start() 
    {
        TabBtn[] tabButtons = { };
        ScrollRect[] tabViews = { };

        if (TabButtonContainer != null) {
            tabButtons = TabButtonContainer.GetComponentsInChildren<TabBtn>();
        }

        for (int i = 0; i < tabButtons.Length; i ++)
        {
            tabDictionary.Add(tabButtons[i], tabButtons[i].TabView);
        }

        foreach (TabBtn b in tabDictionary.Keys)
        {
            b.GetComponent<Button>().onClick.AddListener(() => OnTabClick(b));
        }

        if (tabDictionary.Count > 0 )
        {
            SetSelectedTabButton(tabDictionary.Keys.ToArray()[0]);
        }

    }

    private void SetSelectedTabButton(TabBtn tabButton)
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(tabButton.gameObject, new BaseEventData(eventSystem));
        OnTabClick(tabButton);
    }

    private void OnTabClick(TabBtn tabButton)
    {
        tabDictionary.TryGetValue(tabButton, out GameObject tabView);

        if (tabView != null)
        {
            SetActiveTabView(tabView);
        }
    }

    private void SetActiveTabView(GameObject tab)
    {
        foreach(GameObject g in  tabDictionary.Values)
        {
            g.SetActive(false);
        }

        tab.SetActive(true);
    }
}

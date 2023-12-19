using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class TabController : MonoBehaviour
{
    public GameObject TabButtonContainer;
    public GameObject TabViewContainer;
    private Dictionary<Button, GameObject> tabDictionary = new();
    // Start is called before the first frame update
    void Start() 
    {
        Button[] tabButtons = { };
        ScrollRect[] tabViews = { };

        if (TabButtonContainer != null) {
            tabButtons = TabButtonContainer.GetComponentsInChildren<Button>();
        }

        if (TabViewContainer != null)
        {
            tabViews = TabViewContainer.GetComponentsInChildren<ScrollRect>();
        }

        for (int i = 0; i < tabButtons.Length; i ++)
        {
            tabDictionary.Add(tabButtons[i], tabViews[i].gameObject);
        }

        foreach (Button b in tabDictionary.Keys)
        {
            b.onClick.AddListener(() => OnTabClick(b));
        }

        if (tabDictionary.Count > 0 )
        {
            SetSelectedTabButton(tabDictionary.Keys.ToArray()[0]);
        }

    }

    private void SetSelectedTabButton(Button b)
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(b.gameObject, new BaseEventData(eventSystem));
        OnTabClick(b);
    }

    private void OnTabClick(Button tabButton)
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

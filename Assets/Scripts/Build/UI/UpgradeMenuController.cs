using Unity.VisualScripting;
using UnityEngine;

public class UpgradeMenuController : MonoBehaviour
{
    public static UpgradeMenuController Instance { get; private set; }

    public GameObject _upgradeMenuCanvas;
    public static bool MenuOpen => Instance != null && Instance._upgradeMenuCanvas != null && Instance._upgradeMenuCanvas.activeSelf;

    private void Awake()
    {
        // Check if an instance already exists and if it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this instance to be the singleton instance and optionally make it persistent
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    // Start is called before the first frame update
    void Start()
    {

        // Optionally, hide the menu on start
        if (MenuOpen)
        {
            _upgradeMenuCanvas.SetActive(false);
        }
    }

    public static void OpenUpgradeMenu()
    {
        Instance._upgradeMenuCanvas.SetActive(true);
    }

    public static void CloseUpgradeMenu()
    {
        Debug.Log("Closing");
        Debug.Log(Instance._upgradeMenuCanvas);
        Instance._upgradeMenuCanvas.SetActive(false);
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
}

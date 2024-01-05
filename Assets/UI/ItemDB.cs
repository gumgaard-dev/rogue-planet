using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    public static ItemDB Instance { get; private set; }
    [SerializeField] private List<OreCollectable> _oreCollectables;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public static List<OreCollectable> GetOreList()
    {
        return Instance._oreCollectables;
    }

    public static Sprite GetSpriteForOreType(OreCollectable.ResourceType oreType)
    {
        
        var match = Instance._oreCollectables.Where(o => o.resourceType == oreType).First();

        if (match == null)
        {
            Debug.LogWarning("Attempted to get sprite for oreType: " + oreType.ToString() + ". Returned null.");
        }

        return match.GetSprite();
    }
}

using UnityEngine;

[System.Serializable]
public class OreCollectable : MonoBehaviour, ICollectable
{
    public enum ResourceType
    {
        GREEN, RED
    }

    public ResourceType resourceType;

    public string ItemName;

    public object GetItemType()
    {
        return resourceType;
    }

    public void Collected(ICollectable collectable)
    {
        if((object)collectable == this)
        {
            Destroy(this.gameObject);
        }
    }

    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public string GetName()
    {
        return this.ItemName;
    }
}

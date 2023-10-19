using UnityEngine;

public class OreCollectable : MonoBehaviour, ICollectable
{
    public enum ResourceType
    {
        GREEN, RED
    }

    public ResourceType resourceType;

    public object GetItemType()
    {
        return resourceType;
    }

    public void Collected()
    {
        Destroy(this.gameObject);
    }
}

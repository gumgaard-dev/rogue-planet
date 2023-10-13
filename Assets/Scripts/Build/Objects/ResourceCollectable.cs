using UnityEngine;

public class ResourceCollectable : MonoBehaviour, ICollectable
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
}

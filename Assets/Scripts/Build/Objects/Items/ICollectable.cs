// Should be implemented by the part of a collectable item that contains data about the item
// To implement, define an enum containing possible types of that collectable
// E.g, collectable resource implements an enum ResourceType { GREEN, RED, etc}
using UnityEngine;

public interface ICollectable
{
    // the behavior that should be executed on collection
    public void Collected(ICollectable collectable);
    public object GetItemType();

    public Sprite GetSprite();

    public string GetName();
}

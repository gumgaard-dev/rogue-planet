// Should be implemented by the part of a collectable item that contains data about the item
// To implement, define an enum containing possible types of that collectable
// E.g, collectable resource implements an enum ResourceType { GREEN, RED, etc}
public interface ICollectable
{
    object GetItemType();
}

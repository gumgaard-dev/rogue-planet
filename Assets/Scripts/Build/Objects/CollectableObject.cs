using UnityEngine;

[RequireComponent(typeof(ICollectable))]
public class CollectableObject : MonoBehaviour
{
    public void OnCollect()
    {
        Destroy(gameObject);
    }
}

using Capstone.Build.Objects.ObjectPool;
using UnityEngine;

public class PoolUser : MonoBehaviour
{

    public PoolableObject PoolablePrototype;
    public string PoolKey;
    public Transform InstantiationPoint;

    public void CreatePool(int poolSize)
    {
        if (PoolKey == null)
        {
            PoolKey = PoolablePrototype.gameObject.name;
        }
        PoolManager.Instance.CreatePoolIfNotExists(PoolKey, PoolablePrototype, poolSize);
    }

    public PoolableObject GetFromPool()
    {
        if (PoolKey != null)
        {
            PoolableObject p = PoolManager.Instance.GetFromPool<PoolableObject>(PoolKey);
            p.transform.SetPositionAndRotation(InstantiationPoint.position, InstantiationPoint.rotation);
            return p;
        }
        else { return null; }
    }

    public void ReturnToPool(PoolableObject toReturn)
    {
        if (PoolKey != null)
        {
            PoolManager.Instance.ReturnToPool(PoolKey, toReturn);
        }
    }
}

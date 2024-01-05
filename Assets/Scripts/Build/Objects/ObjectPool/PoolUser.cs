using Capstone.Build.Objects.ObjectPool;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PoolUser : MonoBehaviour
{

    public PoolableObject PoolablePrototype;
    public string PoolKey;
    public Transform InstantiationPoint;
    public List<Vector2> returnPoints;
    public void CreatePool(int poolSize)
    {
        if (PoolKey == null)
        {
            PoolKey = PoolablePrototype.gameObject.name;
        }
        PoolManager.Instance.CreatePoolIfNotExists(PoolKey, PoolablePrototype, poolSize);
    }

    public PoolableObject InstantiateFromPool()
    {
        return InstantiateFromPoolAt(InstantiationPoint.position);
    }

    public PoolableObject InstantiateFromPoolAt(Vector2 position)
    {
        PoolableObject p = GetFromPool();

        if (p != null)
        {
            p.ReturnToPoolCriteriaMet.AddListener(ReturnToPool);
            p.transform.SetPositionAndRotation(position, InstantiationPoint.rotation);
        }
        else
        {
            Debug.LogError("Could not get object from " + PoolKey + "pool.");
        }

        return p;
    }

    public PoolableObject GetFromPool()
    {
        if (PoolKey != null)
        {
            return PoolManager.Instance.GetFromPool<PoolableObject>(PoolKey);
        }
        else { return null; }
    }

    public void ReturnToPool(PoolableObject toReturn)
    {
        if (PoolKey != null)
        {
            toReturn.ReturnToPoolCriteriaMet.RemoveAllListeners();
            Debug.Log("returning");
            returnPoints.Add(toReturn.transform.position);
            PoolManager.Instance.ReturnToPool(PoolKey, toReturn);
        }
    }

    private void OnDrawGizmos()
    {
        if (returnPoints.Count > 0)
        {
            foreach (Vector2 point in returnPoints)
            {
               
                Gizmos.color = UnityEngine.Color.red;
                Gizmos.DrawSphere(new(point.x, point.y), 0.2f);
            }
        }
    }
}

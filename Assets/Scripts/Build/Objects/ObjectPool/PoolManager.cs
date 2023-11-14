using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Build.Objects.ObjectPool
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager _instance;
        public static PoolManager Instance => _instance;

        public float IdleTimeUntilCullingExcessObjects = 10;
        public bool _excessCulled;
        private float _timeSinceLastActive;

        private Dictionary<string, object> pools = new Dictionary<string, object>();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void FixedUpdate()
        {
            if (IdleTimeUntilCullingExcessObjects >= _timeSinceLastActive)
            {
                if (!_excessCulled)
                {
                    CullExcessObjects();
                    _excessCulled = true;
                }
            } else 
            {
                if (_excessCulled) { _excessCulled = false; }
                _timeSinceLastActive += Time.fixedDeltaTime;
            }
        }

        private void CullExcessObjects()
        {
            foreach (object pool in pools.Values)
            {
                ((ObjectPool<PoolableObject>)pool).DestroyExcessObjects();
            }
        }

        public void CreatePool<T>(string poolKey, T prefab, int initialCount) where T : PoolableObject
        {
            ObjectPool<T> newPool = new ObjectPool<T>(prefab, initialCount);
            pools[poolKey] = newPool;
        }

        public T GetObject<T>(string poolKey) where T : PoolableObject
        {
            if (pools.TryGetValue(poolKey, out object pool))
            {
                return ((ObjectPool<T>)pool).Get();
            }
            else
            {
                Debug.LogError("No pool found with key: " + poolKey);
                return null;
            }
        }

        public void ReturnObject<T>(string poolKey, T objectToReturn) where T : PoolableObject
        {
            if (pools.TryGetValue(poolKey, out object pool))
            {
                ((ObjectPool<T>)pool).ReturnToPool(objectToReturn);
            }
            else
            {
                Debug.LogError("No pool found with key: " + poolKey);
            }
        }
    }

}

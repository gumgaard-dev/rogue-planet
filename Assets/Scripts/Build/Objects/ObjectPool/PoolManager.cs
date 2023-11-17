using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Build.Objects.ObjectPool
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager _instance;
        public static PoolManager Instance => _instance;

        public Transform MainPoolContainer;

        private Dictionary<string, object> _pools = new();

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

        public void CreatePoolIfNotExists<T>(string poolKey, T prefab, int initialCount) where T : PoolableObject
        {
            if (!ContainsPool(poolKey))
            {
                GameObject newPoolContainer = new GameObject(poolKey + "Container");
                newPoolContainer.transform.parent = MainPoolContainer.transform;
                ObjectPool<T> newPool = new(prefab, initialCount, newPoolContainer);
                _pools[poolKey] = newPool;
            }
        }
        
        public bool ContainsPool(string poolKey)
        {
            return _pools.ContainsKey(poolKey);
        }

        public T GetFromPool<T>(string poolKey) where T : PoolableObject
        {
            if (_pools.TryGetValue(poolKey, out object pool))
            {
                return ((ObjectPool<T>)pool).Get();
            }
            else
            {
                Debug.LogError("No pool found with key: " + poolKey);
                return null;
            }
        }

        public void ReturnToPool<T>(string poolKey, T objectToReturn) where T : PoolableObject
        {
            if (_pools.TryGetValue(poolKey, out object pool))
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

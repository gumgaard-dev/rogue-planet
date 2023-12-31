using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Capstone.Build.Objects
{

    public class ObjectPool<T> where T : PoolableObject
    {
        [System.Serializable] public class ObjectAddedToPoolEvent : UnityEvent<T> { }
        [System.Serializable] public delegate void ReturnToPoolAction(T returnedObject);
        private readonly Queue<T> _objectQueue = new Queue<T>();
        private readonly T _poolablePrefab;

        public int DefaultAmount;

        public GameObject ObjectContainer;

        public ObjectAddedToPoolEvent ObjectAddedToPool; 

        public ObjectPool(T prefab)
        {
            this._poolablePrefab = prefab;
            this.ObjectContainer = Object.Instantiate(new GameObject("PoolContainer"));
            this.DefaultAmount = 10;
        }

        public ObjectPool (T prefab, GameObject container)
        {
            this._poolablePrefab = prefab;
            this.ObjectContainer = container;
            this.DefaultAmount = 10;
        }
        public ObjectPool(T prefab, int initialCount)
        {
            this._poolablePrefab = prefab;
            this.DefaultAmount = initialCount;
            ObjectContainer = Object.Instantiate(new GameObject(prefab.GetType() + "PoolContainer"));
            AddObjects(DefaultAmount);
        }


        public ObjectPool(T prefab, int initialCount, GameObject container)
        {
            this._poolablePrefab = prefab;
            this.DefaultAmount = initialCount;
            ObjectContainer = container;
            AddObjects(initialCount);
        }

        private void AddObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T newObject = Object.Instantiate(_poolablePrefab, ObjectContainer.transform);
                newObject.Initialize();
                newObject.gameObject.SetActive(false);
                _objectQueue.Enqueue(newObject);
            }
        }

        public T Get()
        {
            if (_objectQueue.Count == 0)
                AddObjects(1); // Optionally add more objects if the pool is empty

            T pObj = _objectQueue.Dequeue();
            pObj.gameObject.SetActive(true);
            pObj.OnGetFromPool();
            return pObj;
        }

        public void ReturnToPool(T pObj)
        {
            pObj.gameObject.SetActive(false);
            pObj.OnReturnToPool();
            _objectQueue.Enqueue(pObj);
        }

        public void ReturnToPool(PoolableObject toReturn)
        {
            ReturnToPool(toReturn as T);
        }
    }
}

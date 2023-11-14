using Build.Component;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        }

        public ObjectPool (T prefab, GameObject container)
        {
            this._poolablePrefab = prefab;
            this.ObjectContainer = container;
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
            ObjectContainer = container;
            AddObjects(initialCount);
        }

        public void DestroyExcessObjects()
        {
            while (_objectQueue.Count > DefaultAmount)
            {
                Object.Destroy( _objectQueue.Dequeue() );
            }
        }

        private void AddObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T newObject = Object.Instantiate(_poolablePrefab, ObjectContainer.transform);
                newObject.gameObject.SetActive(false);

                newObject.OnReturnToPool += ReturnToPool;
                _objectQueue.Enqueue(newObject);
                ObjectAddedToPool?.Invoke(newObject);
            }
        }

        public T Get()
        {
            if (_objectQueue.Count == 0)
                AddObjects(1); // Optionally add more objects if the pool is empty

            T obj = _objectQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnToPool(T toReturn)
        {
            // keep pool at default size if possible
            if ( _objectQueue.Count >= DefaultAmount)
            {
                Object.Destroy(toReturn.gameObject);
            }
            else
            {
                toReturn.gameObject.SetActive(false);
                _objectQueue.Enqueue(toReturn);
            }
        }

        public void ReturnToPool(PoolableObject toReturn)
        {
            ReturnToPool(toReturn as T);
        }
    }
}

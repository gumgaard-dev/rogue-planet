using Build.Component;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class PoolableObject : MonoBehaviour 
{
    public event Action<PoolableObject> OnReturnToPool;

    public void ReturnToPool()
    {
        OnReturnToPool?.Invoke(this);
    }
}
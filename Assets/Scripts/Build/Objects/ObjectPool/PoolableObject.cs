using Build.Component;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class PoolableObject : MonoBehaviour 
{
    public event Action<PoolableObject> OnReturnToPool;

    public virtual void ReturnToPool()
    {
        OnReturnToPool?.Invoke(this);
    }

}
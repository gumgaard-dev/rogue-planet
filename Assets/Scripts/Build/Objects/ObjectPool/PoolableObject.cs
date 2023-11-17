using Build.Component;
using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class ReturnToPoolEvent : UnityEvent<PoolableObject> { }
public abstract class PoolableObject : MonoBehaviour 
{
    public ReturnToPoolEvent OnReturnToPool;

    public virtual void ReturnToPool()
    {
        Debug.Log("returningToPool");
        OnReturnToPool?.Invoke(this);
    }

}
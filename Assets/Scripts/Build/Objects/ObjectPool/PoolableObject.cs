using Build.Component;
using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class PoolObjectEvent : UnityEvent<PoolableObject> { }
public abstract class PoolableObject : MonoBehaviour 
{
    // subscriptions handled by poolUser. Don't override or add listeners.
    // Use the OnReturnToPool and OnGetFromPool methods instead, to ensure object is active.
    public PoolObjectEvent ReturnToPoolCriteriaMet;

    // called when pool clones object, replaces Start() and Awake() methods
    public virtual void Initialize() { }

    
    public virtual void OnReturnToPool()
    {
        Debug.Log("Left Pool");
    }

    public virtual void OnGetFromPool()
    {
        Debug.Log("leaving pool");
    }

}
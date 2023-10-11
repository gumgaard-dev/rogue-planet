using Build.Component;
using UnityEngine;

[RequireComponent(typeof(HealthData))]
public class Ship : MonoBehaviour
{
    private HealthData _healthData;

    private void Start()
    {
        this._healthData = GetComponent<HealthData>();
    }
}

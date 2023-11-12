using Build.World.WorldTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class GlobalLightController : MonoBehaviour, ITimeBasedBehavior, IDayNightBehavior
{
    private Light2D GlobalLight;

    public float DaytimeIntensity = 1.0f; // Intensity during the day
    public float NighttimeIntensity = 0.2f; // Intensity at night
    private float _lastIntensity;
    private float _currentTargetIntensity;

    public float TransitionLength;
    private float _currentTransitionTime;


    public void Awake()
    {
        if (!TryGetComponent(out GlobalLight))
        {
            Debug.Log("No light component attached.");
        }
    }

    public void OnClockIntialized(Clock clock)
    {
        if (clock.IsDay)
        {
            OnDayStart();
        } else
        {
            OnNightStart();
        }
    }


    public void OnTimeChanged(float timeChange)
    {
        if (GlobalLight != null && GlobalLight.intensity != _currentTargetIntensity)
        {
            UpdateLightIntensity(timeChange);
        }
    }

    private void UpdateLightIntensity(float timeChange)
    {
        _currentTransitionTime += timeChange;

        if (_currentTransitionTime <= TransitionLength)
        {
            float currentTransitionTimeNormalized = _currentTransitionTime / TransitionLength;
            GlobalLight.intensity = Mathf.Lerp(_lastIntensity, _currentTargetIntensity, currentTransitionTimeNormalized);
        }
        else
        {
            GlobalLight.intensity = _currentTargetIntensity;
        }
    }


    public void OnDayStart()
    {
        StartTransition(DaytimeIntensity);
    }


    public void OnNightStart()
    {
        StartTransition(NighttimeIntensity);
    }


    private void StartTransition(float targetIntensity)
    {
        _lastIntensity = GlobalLight.intensity;
        _currentTargetIntensity = targetIntensity;
        _currentTransitionTime = 0.0f;
    }
}

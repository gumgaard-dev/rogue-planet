using Build.World.WorldTime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class DayNightController : MonoBehaviour, ITimeBasedBehavior, IDayNightBehavior
{
    private Light2D GlobalLight;

    public float DaytimeIntensity = 1.0f; // Intensity during the day
    public float NighttimeIntensity = 0.2f; // Intensity at night
    private float _lastIntensity;
    private float _currentTargetIntensity;

    public float TransitionLength;
    private float _currentTransitionTime;

    public SpriteRenderer DaySkyBackground;
    public SpriteRenderer NightSkyBackground;
    public bool IsDay;

    private Color _currentNightBackgroundAlpha;


    public void Awake()
    {
        if (!TryGetComponent(out GlobalLight))
        {
            Debug.Log("No light component attached.");
        }
    }

    public void OnClockIntialized(Clock clock)
    {
        _currentNightBackgroundAlpha = new(1f, 1f, 1f, 0f);
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
            _currentTransitionTime += timeChange;
            UpdateLightIntensity();
            UpdateBackground();
        }
    }

    private void UpdateBackground()
    {

        float currentTransitionTimeNormalized = _currentTransitionTime <= TransitionLength ? _currentTransitionTime / TransitionLength : 1;


        // sunrise if day (fade nightsky from 1 to 0), sunset if night (0 to 1)
        _currentNightBackgroundAlpha.a = IsDay ? 1 - currentTransitionTimeNormalized : currentTransitionTimeNormalized;

        NightSkyBackground.color = _currentNightBackgroundAlpha;
    }

    private void UpdateLightIntensity()
    {
        
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
        IsDay = true;
        StartTransition(DaytimeIntensity);
    }


    public void OnNightStart()
    {
        IsDay = false;
        StartTransition(NighttimeIntensity);
    }


    private void StartTransition(float targetIntensity)
    {
        _lastIntensity = GlobalLight.intensity;
        _currentTargetIntensity = targetIntensity;
        _currentTransitionTime = 0.0f;
    }
}

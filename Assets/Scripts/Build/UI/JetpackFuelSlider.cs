using System;
using UnityEngine;
using UnityEngine.UI;

public class JetpackFuelSliderController : MonoBehaviour
{
    public Slider Slider;

    [Header("Low Fuel")]
    public int LowFuelPercentage;
    public Color LowFuelColor;

    [Header("Mid Fuel")]
    public Color MidFuelColor;

    [Header("High Fuel")]
    public int HighFuelPercentage;
    public Color HighFuelColor;

    private Image _fillImage;

    private void Start()
    {
        if (Slider == null)
        {
            Slider = GetComponentInChildren<Slider>();
            if (Slider == null)
            {
                Debug.Log("JetpackFuelSlider: no slider component found, please assign one in the inspector.");
            }
        } else
        {
            Slider.gameObject.SetActive(false);
            Slider.value = 100;
        }

        Slider.fillRect.TryGetComponent(out _fillImage);
    }
    public void UpdateFuelSlider(int maxFuel, int curFuel)
    {
        if (Slider != null)
        {
            // Calculating the remaining fuel percentage, and updating the slider's value
            float newSliderValue = ((float)curFuel / (float)maxFuel) * 100;
            Slider.value = newSliderValue;

            DetermineVisibility();

            DetermineFillColor();
        }
    }

    private void DetermineFillColor()
    {
        if (_fillImage)
        {
            if (Slider.value < LowFuelPercentage) { _fillImage.color = LowFuelColor; }
            else if (Slider.value > HighFuelPercentage) { _fillImage.color = HighFuelColor; }
            else { _fillImage.color = MidFuelColor; }
        }
        
    }

    private void DetermineVisibility()
    {
        // Activating and deactivating the slider based on whether FuelLevel is full
        if (!Slider.IsActive() && Slider.value < Slider.maxValue)
        {
            Slider.gameObject.SetActive(true);
        }
        else if (Slider.IsActive() && Slider.value >= Slider.maxValue) 
        {
            Slider.gameObject.SetActive(false);
        }
    }
}

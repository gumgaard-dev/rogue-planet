using UnityEngine;
using UnityEngine.UI;

public class JetpackFuelSliderController : MonoBehaviour
{
    public Slider Slider;

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
    }
    public void UpdateFuelSlider(int maxFuel, int curFuel)
    {
        if (Slider != null)
        {
            // Calculating the remaining fuel percentage, and updating the slider's value
            float newSliderValue = ((float)curFuel / (float)maxFuel) * 100;

            // Activating and deactivating the slider based on whether FuelLevel is full
            if (newSliderValue < 100)
            {
                Slider.gameObject.SetActive(true);
            }
            else
            {
                Slider.gameObject.SetActive(false);
            }

            Slider.value = newSliderValue;
        }
    }
}

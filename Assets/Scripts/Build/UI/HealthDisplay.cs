using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Slider HealthSlider;
    private RectTransform _sliderRectTransform;
    private Image _sliderFillImage;

    public TextMeshPro MaxHealthText;
    public TextMeshPro CurHealthText;
    

    private void Awake()
    {
        if (HealthSlider == null)
        {
            Debug.LogWarning("Healthbar slider not set in inspector.");
        }
        else
        {

            if (!HealthSlider.fillRect.TryGetComponent(out _sliderFillImage))
            {
                Debug.LogWarning("Could not get slider fill image from HealthBar");
            }
            if (!HealthSlider.TryGetComponent(out _sliderRectTransform))
            {
                Debug.LogWarning("Could not get slider rect transform and will not be able to resize health bar.");
            }
        }
    }
    public void UpdateMaxHealth(int maxHealth)
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = maxHealth;
            _sliderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth);
        }
    }

    public void UpdateCurHealth(int curHealth)
    {
        HealthSlider.value = curHealth;
    }
}

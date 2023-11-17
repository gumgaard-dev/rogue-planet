using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Slider HealthBar;
    public TextMeshPro MaxHealthText;
    public TextMeshPro CurHealthText;
    private Image _sliderFillRect;

    public void UpdateMaxHealth(int maxHealth)
    {
        HealthBar.maxValue = maxHealth;
        HealthBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth);
    }

    public void UpdateCurHealth(int curHealth)
    {
        HealthBar.value = curHealth;
    }
}

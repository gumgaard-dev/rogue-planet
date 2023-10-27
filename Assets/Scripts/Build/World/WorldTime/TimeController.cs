using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private float timeMultiplier;

    [SerializeField] 
    private float startHour;

    [SerializeField]
    private TextMeshProUGUI timeText;

    private DateTime _currentTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void updateTimeOfDay()
    {
        _currentTime = _currentTime.AddSeconds(Time.deltaTime + timeMultiplier);

        if (timeText != null)
        {
            timeText.text = _currentTime.ToString("HH:mm");
        }
    }
}

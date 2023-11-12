using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------References--------------------------------------------------------//
// sun: <a href="https://www.flaticon.com/free-icons/sun" title="sun icons">Sun icons created by Freepik - Flaticon</a>     //
// moon: <a href="https://www.flaticon.com/free-icons/moon" title="moon icons">Moon icons created by Freepik - Flaticon</a> //
//--------------------------------------------------------------------------------------------------------------------------//

namespace Build.World.WorldTime
{
    public class ClockSliderController : MonoBehaviour, IDayNightBehavior, ITimeBasedBehavior
    {
        private Slider _timeSlider;
        private Image _sliderFill;
        public Clock Clock;

        public Image DayIcon;
        public Image NightIcon;

        public Color DayFillColor = Color.yellow;
        public Color NightFillColor = Color.blue;

        public void Awake()
        {
            this._timeSlider = GetComponentInChildren<Slider>();
            _timeSlider.fillRect.TryGetComponent(out _sliderFill);
            if (this.Clock == null)
            {
                Debug.LogWarning("No clock set in the inspector");
            }
        }

        public void OnClockIntialized(Clock clock)
        {
            if (clock.IsDay)
            {
                OnDayStart();
            }
            else
            {
                OnNightStart();
            }
        }

        public void OnTimeChanged(float timeChange)
        {
            if (this._timeSlider != null)
            {
                _timeSlider.value -= timeChange;
            }
        }

        public void OnDayStart()
        {
            _timeSlider.maxValue = Clock.DayDuration;
            _timeSlider.value = Clock.DayDuration;
            SetSliderColor(DayFillColor);
            DayIcon.gameObject.SetActive(true);
            NightIcon.gameObject.SetActive(false);
        }

        public void OnNightStart()
        {
            _timeSlider.maxValue = Clock.NightDuration;
            _timeSlider.value = Clock.NightDuration;
            SetSliderColor(NightFillColor);
            DayIcon.gameObject.SetActive(false);
            NightIcon.gameObject.SetActive(true);
        }

        private void SetSliderColor(Color color)
        {
            if (_sliderFill != null)
            {
                _sliderFill.color = color;
            }
        }
    }
}
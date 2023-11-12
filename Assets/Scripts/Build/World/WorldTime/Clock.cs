using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Build.World.WorldTime
{
    [System.Serializable] public class TimeChangedEvent : UnityEvent<float> { }
    [System.Serializable] public class ClockInitializedEvent : UnityEvent<Clock> { }
    public class Clock : MonoBehaviour
    {
        public float DayDuration = 10f; // Duration of the daytime in seconds
        public float NightDuration = 10f; // Duration of the nighttime in seconds
        private float _cycleDuration;

        private float _currentTimeInCycle = 0f;
        private float _timeStep;
        public float TimeOfDay;
        public bool IsDay;
        private bool _wasDay;

        // Cached values for time of day adjustment
        private float _dayTimeIncrement;
        private float _nightTimeDecrement;

        public TimeChangedEvent TimeOfDayChanged;
        public ClockInitializedEvent ClockInitialized;
        public UnityEvent DayStart;
        public UnityEvent NightStart;

        private void Awake()
        {
            _cycleDuration = DayDuration + NightDuration;

            _timeStep = Time.fixedDeltaTime;

            IsDay = true;
        }

        private void Start()
        {
            ClockInitialized?.Invoke(this);
        }


        private void FixedUpdate()
        {
            if (_currentTimeInCycle >= _cycleDuration)
                _currentTimeInCycle = 0;

            _currentTimeInCycle += _timeStep;

            UpdateDayNightState();

            // Update slider and other day/night related changes
            TimeOfDayChanged?.Invoke(_timeStep);
        }

        private void UpdateDayNightState()
        {
            // Determine if it is day or night
            IsDay = _currentTimeInCycle < DayDuration;

            if (!_wasDay && IsDay)
            {
                DayStart?.Invoke();
            }
            else if (_wasDay && !IsDay)
            {
                NightStart?.Invoke();
            }

            _wasDay = IsDay;
        }
    }
}
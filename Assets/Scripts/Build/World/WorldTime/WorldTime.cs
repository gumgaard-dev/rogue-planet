using UnityEngine;

namespace Build.World.WorldTime
{
    public class WorldTime : MonoBehaviour
    {
        public float cycleDuration = 10f; // duration of a full day-night cycle in seconds
        public Light directionalLight;

        public float daytimeIntensity = 1.0f; // intensity during the day
        public float nighttimeIntensity = 0.2f; // intensity at night
        public float middayIntensity = 0.6f;
        
        public float timer;
        public float timeOfDay;
        public bool isDay;

        private float _intensity;

        private void Update()
        {
            // calculate current time of day based on the timer
            timeOfDay = Mathf.PingPong(timer, cycleDuration) / cycleDuration;

            // update intensity based on time of day
            // dawn to high noon
            if (isDay && (timeOfDay <= 0.5))
            {
                _intensity = Mathf.Lerp(middayIntensity, daytimeIntensity, timeOfDay);
                directionalLight.intensity = _intensity;
            } 
            // high noon to dusk
            else if (isDay && (0.5 < timeOfDay))
            {
                _intensity = Mathf.Lerp(daytimeIntensity, middayIntensity, timeOfDay);
                directionalLight.intensity = _intensity;
            } 
            // dusk to midnight
            else if (!isDay && (timeOfDay <= 0.5))
            {
                _intensity = Mathf.Lerp(middayIntensity, nighttimeIntensity, timeOfDay);
                directionalLight.intensity = _intensity;
            } 
            // midnight to dawn
            else if (!isDay && (0.5 < timeOfDay))
            {
                _intensity = Mathf.Lerp(nighttimeIntensity, middayIntensity, timeOfDay);
                directionalLight.intensity = _intensity;
            }

            // updates time of day bool
            if (timeOfDay <= 0.01) isDay = true;
            if (timeOfDay >= 0.99) isDay = false;
            
            // update timer
            timer += Time.deltaTime;
        }
    }
}
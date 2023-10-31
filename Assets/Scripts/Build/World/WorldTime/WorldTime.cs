using UnityEngine;

namespace Build.World.WorldTime
{
    public class WorldTime : MonoBehaviour
    {
        public float cycleDuration = 10f; // duration of a full day-night cycle in seconds
        public Light directionalLight;

        public float daytimeIntensity = 1.0f; // intensity during the day
        public float nighttimeIntensity = 0.2f; // intensity at night
        
        private float _timer;

        private void Update()
        {
            // calculate current time of day based on the timer
            float timeOfDay = Mathf.PingPong(_timer, cycleDuration) / cycleDuration;
            
            // calculate intensity based on time of day
            float intensity = Mathf.Lerp(daytimeIntensity, nighttimeIntensity, timeOfDay);
            directionalLight.intensity = intensity;

            // update timer
            _timer += Time.deltaTime;
        }
    }
}
using UnityEngine;

namespace Build.World.WorldTime
{
    public class DayNightCycle : MonoBehaviour
    {
        public float cycleDuration = 60f; // Duration of a full day-night cycle in seconds
        public Light directionalLight; // Reference to your directional light
        public Material daySkybox; // Material for the day skybox
        public Material nightSkybox; // Material for the night skybox
        public AnimationCurve lightIntensityCurve;
        
        private float _timer = 0f;
        private bool _isDay = true;

        private void Start()
        {
            // Initialize with the day skybox and full light intensity
            RenderSettings.skybox = daySkybox;
            directionalLight.intensity = 1.0f;
        }

        private void Update()
        {
            // Calculate the current time of day based on the timer
            float timeOfDay = Mathf.PingPong(_timer, cycleDuration) / cycleDuration;
            //directionalLight.intensity = Mathf.Lerp(0.1f, 1.0f, timeOfDay);
            directionalLight.intensity = lightIntensityCurve.Evaluate(timeOfDay);

            // Change lighting and skybox based on time of day
            if (timeOfDay >= 0.5f && _isDay)
            {
                // Transition from day to night
                RenderSettings.skybox = nightSkybox;
                _isDay = false;
            }
            else if (timeOfDay < 0.5f && !_isDay)
            {
                // Transition from night to day
                RenderSettings.skybox = daySkybox;
                _isDay = true;
            }

            // Adjust directional light intensity based on time of day
            directionalLight.intensity = Mathf.Lerp(0.2f, 1.0f, timeOfDay);

            // Update the timer
            _timer += Time.deltaTime;
        }
    }
}
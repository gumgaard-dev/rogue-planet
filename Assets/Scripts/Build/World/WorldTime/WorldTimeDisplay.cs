using UnityEngine;
using UnityEngine.UI;

namespace Build.World.WorldTime
{
    public class WorldTimeDisplay : MonoBehaviour
    {
        public Slider timeOfDaySlider;
        public WorldTime worldTime;

        public Image imgSun;
        public Image imgMoon;

        private void Start()
        {
            imgSun.gameObject.SetActive(true);
            imgMoon.gameObject.SetActive(false);
            timeOfDaySlider.value = 0.5f; 
        }

        // Update is called once per frame
        private void Update()
        {
            // get the timeOfDay value from WorldTime script
            var timeOfDay = worldTime.timeOfDay;
            
            // calculate slider value based on time of day (decreasing as night approaches)
            var sliderValue = 1f - timeOfDay;
            
            // updates slider value
            timeOfDaySlider.value = sliderValue;
            
            // update image
            if (worldTime.isDay)
            {
                imgSun.gameObject.SetActive(true);
                imgMoon.gameObject.SetActive(false);
            }
            else
            {
                imgSun.gameObject.SetActive(false);
                imgMoon.gameObject.SetActive(true);
            }
        }
    }
}
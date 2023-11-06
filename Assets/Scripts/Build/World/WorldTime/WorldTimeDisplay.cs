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

        // Update is called once per frame
        void Update()
        {
            // check that WorldTime script is assigned
            if (worldTime != null)
            {
                // get the timeOfDay value from WorldTime script
                float timeOfDay = worldTime.timeOfDay;
                
                // calculate slider value based on time of day (decreasing as night approaches)
                float sliderValue = 1f - timeOfDay;
                
                // updates slider value
                timeOfDaySlider.value = sliderValue;
                
                // update image
                if (sliderValue <= 0.5f)
                {
                    imgSun.gameObject.SetActive(false);
                    imgMoon.gameObject.SetActive(true);
                }
                else
                {
                    imgSun.gameObject.SetActive(true);
                    imgMoon.gameObject.SetActive(false);
                }
            }
        }
    }
}

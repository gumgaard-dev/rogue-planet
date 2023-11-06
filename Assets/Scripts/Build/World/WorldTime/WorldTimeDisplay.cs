using UnityEngine;
using UnityEngine.UI;

namespace Build.World.WorldTime
{
    public class WorldTimeDisplay : MonoBehaviour
    {
        public Slider timeOfDaySlider;
        public WorldTime worldTime;

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
            }
        }
    }
}

using Capstone.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Capstone.Build.Characters.Player.PlayerStates;
using UnityEngine.UI;

namespace Capstone.Build.Characters.Player
{

    public class Jetpack : MonoBehaviour
    {

        [SerializeField]
        public Slider FuelSlider;
        private GameSettings _settings;

        public int FuelLevel;
        public int MaxFuel;
        public int RechargeRate;
        public int FuelConsumptionRate;
        public int RechargeDelay;
        public int RechargeTimer;

        private Player _player;

        // Start is called before the first frame update
        void Start()
        {

            if (!transform.parent.gameObject.TryGetComponent(out _player))
            {
                Debug.LogWarning("Jetpack is not a child of an object with Player component.");
            }

            _settings = Resources.Load<GameSettings>(_player.SETTINGS_PATH);
            if (_settings == null)
            {
                Debug.LogWarning("Jetpack could not load GameSettings at " + _player.SETTINGS_PATH);
            }

            // Fixed update is called 50 times a second and we will -1 fuel each time. So this gives 5 seconds of fuel

            MaxFuel = _settings.DefaultJetpackMaxFuel;

            FuelLevel = MaxFuel;
            // Recharge rate will be applied to fuel every FixedUpdate if jetpack is not in use
            RechargeRate = _settings.DefaultJetpackRechargeRate;

            FuelConsumptionRate = _settings.DefaultJetpackConsumptionRate;
            // RechargeTimer will be incremented each FixedUpdate when jetpack not being used, and reset to zero when jetpack is used
            // When RechargeTimer reaches RechargeDelay, fuel begins to recharge 
            RechargeTimer = 0;
            RechargeDelay = _settings.DefaultJetpackRechargeDelay;

            UpdateFuelSlider();

        }


        // FixedUpdate is called 50 times a second by default
        void FixedUpdate()
        {
            // If the player is not in jetpack state we can start to recharge
            if (FuelLevel < MaxFuel)
            {
                if (_player.StateType != PlayerStateType.Jetpack && _player.StateType != PlayerStateType.Fall)
                {

                    // Checking the timer
                    if (RechargeTimer < RechargeDelay)
                    {
                        RechargeTimer += 1;
                    }
                    else if (FuelLevel < MaxFuel)
                    {
                        // This makes sure we don't accidentally go over max fuel
                        if (FuelLevel + RechargeRate <= MaxFuel)
                        {
                            FuelLevel += RechargeRate;
                        }
                        else
                        {
                            FuelLevel = MaxFuel;
                        }
                    }
                }
                
                // We only need to call this function when fuel is not max. Fuel gets set to max in this if block, so this will get called one last time when the fuel reaches max.
                UpdateFuelSlider();

            }
            
        }

        void UpdateFuelSlider()
        {
            if (FuelLevel < MaxFuel) { }
            // Calculating the remaining fuel percentage, and updating the slider's value
            FuelSlider.value = ((float)FuelLevel / (float)MaxFuel) * 100;

            // Activating and deactivating the slider based on whether FuelLevel is full
            if (FuelSlider.value < 100) 
            {
                FuelSlider.gameObject.SetActive(true);
            }
            else
            {
                FuelSlider.gameObject.SetActive(false);
            }
        }
    }


}
using UnityEngine;
using Capstone.Build.Characters.Player.PlayerStates;
using UnityEngine.Events;

namespace Capstone.Build.Characters.Player
{
    [System.Serializable] public class FuelUpdatedEvent : UnityEvent<int, int> { }
    public class Jetpack : MonoBehaviour
    {
        public FuelUpdatedEvent FuelUpdated;
        private GameSettings _settings;

        public int _curFuelLevel;
        public int MaxFuel;
        public int RechargeRate;
        public int FuelConsumptionRate;
        public int RechargeDelay;
        private int _rechargeTimer;

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
            else
            {
                // Fixed update is called 50 times a second and we will -1 fuel each time. So this gives 5 seconds of fuel
                MaxFuel = _settings.DefaultJetpackMaxFuel;
                _curFuelLevel = MaxFuel;
                
                // Recharge rate will be applied to fuel every FixedUpdate if jetpack is not in use
                RechargeRate = _settings.DefaultJetpackRechargeRate;
                FuelConsumptionRate = _settings.DefaultJetpackConsumptionRate;
                
                // RechargeTimer will be incremented each FixedUpdate when jetpack not being used, and reset to zero when jetpack is used
                // When RechargeTimer reaches RechargeDelay, fuel begins to recharge 
                _rechargeTimer = 0;
                RechargeDelay = _settings.DefaultJetpackRechargeDelay;
            }

        }


        // FixedUpdate is called 50 times a second by default
        void FixedUpdate()
        {
            // If the player is not in jetpack state we can start to recharge
            if (_curFuelLevel < MaxFuel)
            {
                if (_player.StateType != PlayerStateType.Jetpack && _player.StateType != PlayerStateType.Fall)
                {

                    // Checking the timer
                    if (_rechargeTimer < RechargeDelay)
                    {
                        _rechargeTimer += 1;
                    }
                    else if (_curFuelLevel < MaxFuel)
                    {
                        // This makes sure we don't accidentally go over max fuel
                        if (_curFuelLevel + RechargeRate <= MaxFuel)
                        {
                            _curFuelLevel += RechargeRate;
                        }
                        else
                        {
                            _curFuelLevel = MaxFuel;
                        }
                    }
                }

                FuelUpdated?.Invoke(MaxFuel, _curFuelLevel);
            }      
        }

        public void InitializeTimer()
        {
            this._rechargeTimer = 0;
        }

        public void ConsumeFuel()
        {
            this._curFuelLevel -= FuelConsumptionRate;
            FuelUpdated?.Invoke(MaxFuel, _curFuelLevel);
        }
        
        public bool HasFuel()
        {
            return this._curFuelLevel > 0;
        }
    }
}
using UnityEngine;
using Capstone.Build.Characters.Player.PlayerStates;
using UnityEngine.Events;
using System;
using Capstone.Input;

namespace Capstone.Build.Characters.Player
{
    [System.Serializable] public class FuelUpdatedEvent : UnityEvent<int, int> { }
    public class Jetpack : MonoBehaviour
    {
        public FuelUpdatedEvent FuelUpdated;
        public bool UseGameSettings;
        private GameSettings _settings;

        public int MaxFuel;
        public int RechargeRate;
        public int FuelConsumptionRate;

        [Header("Thrust")]
        public float NormalThrust;
        public float LiftoffThrust;

        [Header("How long after landing before charging starts.")]
        public int RechargeDelay;

        [Header("How long to apply liftoff thrust after leaving ground")]
        public int LiftoffLength;

        [Header("Don't change these (for visualization only)")]
        [SerializeField] private int _timeSinceLanding = 0;
        [SerializeField] private int _timeSinceLiftoff = 0;
        [SerializeField] private int _curFuelLevel = 0;



        private Player _player;
        private bool _canRecharge;

        private ParticleSystem[] _thrustParticleSystems;

        // Start is called before the first frame update
        void Start()
        {
            if (!transform.parent.gameObject.TryGetComponent(out _player))
            {
                Debug.LogWarning("Jetpack is not a child of an object with Player component.");
            }


            _thrustParticleSystems = GetComponentsInChildren<ParticleSystem>();
            if (_thrustParticleSystems.Length == 0 ) { Debug.LogWarning("Jetpack has no particle systems as children"); }

            
            if (UseGameSettings && _settings == null)
            {
                _settings = Resources.Load<GameSettings>(_player.SETTINGS_PATH);
                if (_settings == null)
                {
                    Debug.LogWarning("Jetpack UseGameSettings set to true, but could not load GameSettings at " + _player.SETTINGS_PATH);
                } else
                {
                    // Fixed update is called 50 times a second and we will -1 fuel each time. So this gives 5 seconds of fuel
                    MaxFuel = _settings.DefaultJetpackMaxFuel;

                    // Recharge rate will be applied to fuel every FixedUpdate if jetpack is not in use
                    RechargeRate = _settings.DefaultJetpackRechargeRate;
                    FuelConsumptionRate = _settings.DefaultJetpackConsumptionRate;

                    // RechargeTimer will be incremented each FixedUpdate when jetpack not being used, and reset to zero when jetpack is used
                    // When RechargeTimer reaches RechargeDelay, fuel begins to recharge 
                    RechargeDelay = _settings.DefaultJetpackRechargeDelay;

                    NormalThrust = _settings.JetpackSpeed;
                    LiftoffThrust = _settings.JetpackBoostSpeed;
                }
            } 

            _curFuelLevel = MaxFuel;
            _canRecharge = true;

        }


        // FixedUpdate is called 50 times a second by default
        void FixedUpdate()
        {
            if (!_canRecharge) { return; }

            if (_curFuelLevel == MaxFuel) { _canRecharge = false; return; }

            if (_curFuelLevel > MaxFuel) { _canRecharge = false; _curFuelLevel = MaxFuel; return; }
            
            if (_timeSinceLanding < RechargeDelay) { _timeSinceLanding += 1; return; }

            RechargeFuel();
        }

        public void ActivateThrustParticles()
        {
            if (_curFuelLevel > 0)
            {
                foreach (ParticleSystem particleSystem in _thrustParticleSystems)
                {
                    if (!particleSystem.isPlaying) { particleSystem.Play(); }
                }
            }
        }

        public void DeactivateThrustParticles()
        {
            foreach (ParticleSystem particleSystem in _thrustParticleSystems)
            {
                if (particleSystem.isPlaying) { particleSystem.Stop(); }
            }
        }

        public void ConsumeFuel()
        {
            this._curFuelLevel -= FuelConsumptionRate;
            FuelUpdated?.Invoke(MaxFuel, _curFuelLevel);
            

        }

        public void RechargeFuel()
        {
            _curFuelLevel += RechargeRate;
            FuelUpdated?.Invoke(MaxFuel, _curFuelLevel);
        }

        public bool HasFuel()
        {
            return this._curFuelLevel > 0;
        }

        public float CalculateThrust()
        {
            if (this._timeSinceLiftoff < this.LiftoffLength)
            {
                _timeSinceLiftoff += 1;
                return LiftoffThrust;
            }
            else if (this._curFuelLevel > 0)
            {
                return NormalThrust;
            }
            else
            {
                return 0;
            }

        }

        public void OnPlayerGrounded()
        {
            this._canRecharge = true;
            this._timeSinceLanding = 0;
            this._timeSinceLiftoff = 0;
        }

        public void OnPlayerNotGrounded()
        {
            this._canRecharge = false;
        }
    }
}
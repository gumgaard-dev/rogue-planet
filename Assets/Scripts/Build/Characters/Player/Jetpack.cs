using Capstone.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Capstone.Build.Characters.Player.PlayerStates;

namespace Capstone.Build.Characters.Player
{

    public class Jetpack : MonoBehaviour
    {

        private int _maxFuel;

        public int Fuel;
        public int RechargeRate;
        public int RechargeDelay;
        public int RechargeTimer;

        private Player _player;

        // Start is called before the first frame update
        void Start()
        {
            // Fixed update is called 50 times a second and we will -1 fuel each time. So this gives 5 seconds of fuel
            _maxFuel = 250;

            Fuel = _maxFuel;
            // Recharge rate will be applied to fuel every FixedUpdate if jetpack is not in use
            RechargeRate = 1;
            // RechargeTimer will be incremented each FixedUpdate when jetpack not being used, and reset to zero when jetpack is used
            // When RechargeTimer reaches RechargeDelay, fuel begins to recharge 
            RechargeTimer = 0;
            RechargeDelay = 50;

            if (!transform.parent.gameObject.TryGetComponent(out _player))
            {
                Debug.LogWarning("Jetpack is not a child of an object with Player component.");
            }


        }

        // FixedUpdate is called 50 times a second by default
        void FixedUpdate()
        {
            if (_player.StateType != PlayerStateType.Jetpack)
            {
                if (RechargeTimer < RechargeDelay)
                {
                    RechargeTimer += 1;
                }
                else if (Fuel < _maxFuel)
                {
                    Fuel += RechargeRate;
                }
            }
        }
    }


}
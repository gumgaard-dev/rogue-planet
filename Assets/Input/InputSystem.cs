using UnityEngine;
using UnityEngine.InputSystem;
using Capstone.Build.Characters.Player;
using System;
using System.Collections.Generic;

namespace Capstone.Input
{
    public class InputSystem : MonoBehaviour
    {

        private Player _player;

        private readonly Dictionary<string, InputAction> _playerActions = new();
        private readonly Dictionary<string, InputAction> _shipActions = new();

        private Vector2 _previousDirectionalInput;
        private bool _previousJumpInput;
        private bool _previousShootInput;

        public void AwakeManaged()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();

            var PIA = new PlayerInputActions();
            var shipActions = PIA.Ship;
            var playerActions = PIA.Player;

            _playerActions.Add("Move", playerActions.Move);
            _playerActions.Add("Jump", playerActions.Jump);
            _playerActions.Add("Aim", playerActions.Aim);
            _playerActions.Add("EnterShip", playerActions.EnterShip);


            _shipActions.Add("ExitShip", shipActions.ExitShip);
            _shipActions.Add("Shoot", shipActions.Shoot);
            _shipActions.Add("Aim", shipActions.Aim);
            _shipActions.Add("PrecisionAim", shipActions.PrecisionAim);
        }

        public void UpdateManaged()
        {
            if (_player.StateType == Build.Characters.Player.PlayerStates.PlayerStateType.InShip)
            {
                PollShootCombatInput();
                PollCombatAimInput();
                PollExitShipInput();
                PollPrecisionAimInput();
            }
            else
            {
                PollMoveInput();
                PollJumpInput();
                PollMiningAimInput();
                PollEnterShipInput();
            }

        }

        private void PollEnterShipInput()
        {
            _player.State.SetEnterShipInput(_playerActions["EnterShip"].ReadValue<float>() > 0);
        }

        private void PollExitShipInput()
        {
            _player.State.SetExitShipInput(_shipActions["ExitShip"].ReadValue<float>() > 0);
        }

        private void PollCombatAimInput()
        {
            _player.State.SetShipAimInput(_shipActions["Aim"].ReadValue<float>());
        }

        private void PollMiningAimInput()
        {
            _player.State.SetAimInput(_playerActions["Aim"].ReadValue<Vector2>());
        }

        private void PollMoveInput() 
        {
            Vector2 currentDirectionalInput = _playerActions["Move"].ReadValue<Vector2>();

            // D-Pad does not detect a new button press if finger is slid from one direction to another without letting go
            // Checking against the previous move input will fix this issue

            // Checking if d-pad was either released or pressed right previously, and is now pressed left
            if (currentDirectionalInput.x > 0 && _previousDirectionalInput.x <= 0)
            {
                _player.State.SetHorizontalInput(1);
            }
            // Checks if d-pad was either released or pressed left previously, and is now pressed left
            else if (currentDirectionalInput.x < 0 && _previousDirectionalInput.x >= 0)
            {
                _player.State.SetHorizontalInput(-1);
            }
            // Checks if the d-pad was previously pressed left or right and is now released (or pressed up or down)
            else if (currentDirectionalInput.x == 0 && _previousDirectionalInput.x != 0)
            {
                _player.State.SetHorizontalInput(0);
            }

            // Doing the same checks but for y axis
            if (currentDirectionalInput.y > 0 && _previousDirectionalInput.y <= 0)
            {
                _player.State.SetVerticalInput(1);
            }
            else if (currentDirectionalInput.y < 0 && _previousDirectionalInput.y >= 0)
            {
                _player.State.SetVerticalInput(-1);
            }
            else if (currentDirectionalInput.y == 0 && _previousDirectionalInput.y != 0)
            {
                _player.State.SetVerticalInput(0);
            }


            _previousDirectionalInput = currentDirectionalInput;

            // Follow this same process for y value if we want that functionality (maybe for climbing ladders)
        }

        private void PollJumpInput() 
        {
            bool currentJumpInput = (_playerActions["Jump"].ReadValue<float>() > 0);

            if (currentJumpInput != _previousJumpInput)
            {
                _player.State.SetJumpInput(currentJumpInput);
            }

            _previousJumpInput = currentJumpInput;
        }
        private void PollShootCombatInput()
        {
            bool currentShootInput = _shipActions["Shoot"].ReadValue<float>() > 0;

            if (currentShootInput != _previousShootInput)
            {
                _player.State.SetShootInput(currentShootInput);
            }
            
            _previousShootInput = currentShootInput;
        }

        private void PollPrecisionAimInput()
        {
            _player.State.SetPrecisionAimInput(_shipActions["PrecisionAim"].ReadValue<float>() > 0);
        }

        void OnEnable()
        {
            foreach (InputAction i in _playerActions.Values)
            {
                i.Enable();
            }

            foreach (InputAction i in _shipActions.Values)
            {
                i.Enable();
            }
        }

        void OnDisable()
        {
            foreach (InputAction i in _playerActions.Values)
            {
                i.Disable();
            }

            foreach (InputAction i in _shipActions.Values)
            {
                i.Disable();
            }
        }
    }

}

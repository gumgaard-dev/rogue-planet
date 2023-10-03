using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Capstone
{
    public class InputSystem : MonoBehaviour
    {

        private Player _player;

        private PlayerInputActions _playerInputActions;

        private InputAction _moveAction;
        private InputAction _jumpAction;

        private Vector2 _previousMoveInput;
        private float _previousJumpInput;

        public void AwakeManaged()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();

            _playerInputActions = new PlayerInputActions();

            _moveAction = _playerInputActions.Player.Move;
            _jumpAction = _playerInputActions.Player.Jump;
        }

        public void UpdateManaged()
        {
            PollMoveInput();
            PollJumpInput();
        }

        private void PollMoveInput() 
        {
            Vector2 currentMoveInput = _moveAction.ReadValue<Vector2>();

            // D-Pad does not detect a new button press if finger is slid from one direction to another without letting go
            // Checking against the previous move input will fix this issue

            // Checking if d-pad was either released or pressed right previously, and is now pressed left
            if (currentMoveInput.x > 0 && _previousMoveInput.x <= 0)
            {
                _player.State.SetHorizontalInput(1);
            }
            // Checks if d-pad was either released or pressed left previously, and is now pressed left
            else if (currentMoveInput.x < 0 && _previousMoveInput.x >= 0)
            {
                _player.State.SetHorizontalInput(-1);
            }
            // Checks if the d-pad was previously pressed left or right and is now released (or pressed up or down)
            else if (currentMoveInput.x == 0 && _previousMoveInput.x != 0)
            {
                _player.State.SetHorizontalInput(0);
            }

            _previousMoveInput = currentMoveInput;

            // Follow this same process for y value if we want that functionality (maybe for climbing ladders)
        }

        private void PollJumpInput() 
        {
            float currentJumpInput = _jumpAction.ReadValue<float>();

            if (currentJumpInput != _previousJumpInput)
            {
                _player.State.SetJumpInput(currentJumpInput);
            }

            _previousJumpInput = currentJumpInput;
        }

        void OnEnable()
        {
            _moveAction.Enable();
            _jumpAction.Enable();
        }

        void OnDisable()
        {
            _moveAction.Disable();
            _jumpAction.Disable();
        }
    }

}

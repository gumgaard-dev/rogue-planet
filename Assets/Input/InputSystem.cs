using UnityEngine;
using UnityEngine.InputSystem;
using Capstone.Build.Characters.Player;

namespace Capstone.Input
{
    public class InputSystem : MonoBehaviour
    {

        private Player _player;

        private PlayerInputActions _playerInputActions;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _shootAction;

        private Vector2 _previousDirectionalInput;
        private bool _previousJumpInput;
        private bool _previousShootInput;

        public void AwakeManaged()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();

            _playerInputActions = new PlayerInputActions();

            _moveAction = _playerInputActions.Player.Directional;
            _jumpAction = _playerInputActions.Player.Jump;
            _shootAction = _playerInputActions.Player.Shoot;
        }

        public void UpdateManaged()
        {
            PollDirectionalInput();
            PollJumpInput();
            PollShootInput();
        }

        private void PollDirectionalInput() 
        {
            Vector2 currentDirectionalInput = _moveAction.ReadValue<Vector2>();

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
            bool currentJumpInput = (_jumpAction.ReadValue<float>() > 0);

            if (currentJumpInput != _previousJumpInput)
            {
                _player.State.SetJumpInput(currentJumpInput);
            }

            _previousJumpInput = currentJumpInput;
        }
        private void PollShootInput()
        {
            bool currentShootInput = _shootAction.ReadValue<float>() > 0;

            if (currentShootInput != _previousShootInput)
            {
                _player.State.SetShootInput(currentShootInput);
            }
            
            _previousShootInput = currentShootInput;
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

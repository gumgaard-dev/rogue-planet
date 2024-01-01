using UnityEngine;
using UnityEngine.InputSystem;
using Capstone.Build.Characters.Player;
using Capstone.Build.Characters.Player.PlayerStates;

namespace Capstone.Input
{
    public class InputSystem : MonoBehaviour
    {

        private Player _player;
        private GameInputActions _inputActions;
        private GameInputActions.PlayerActions _playerActions;
        private GameInputActions.ShipActions _shipActions;

        private Vector2 _previousDirectionalInput;

        public void AwakeManaged()
        {
            _player = FindObjectOfType<Player>();
            _inputActions = new();
            _playerActions = _inputActions.Player;
            _shipActions = _inputActions.Ship;
        }

        void OnEnable()
        {
            _inputActions.Enable();
        }

        void OnDisable()
        {
            _inputActions.Disable();
        }

        public void UpdateManaged()
        {
            switch (_player.StateType)
            {
                case PlayerStateType.InShip:
                    PollShipInput();
                    break;

                default:
                    PollPlayerInput();
                    break;
            }
        }

        private void PollShipInput()
        {
            InputInfo.OpenUpgradeMenu = _shipActions.OpenUpgradeMenu.triggered;
            InputInfo.ExitShip = _shipActions.ExitShip.triggered;
            InputInfo.ShipAim = _shipActions.Aim.ReadValue<float>();
            InputInfo.ShootHeld = InputHeld(_shipActions.Shoot);
            InputInfo.ShootPressed = _shipActions.Shoot.triggered;
            InputInfo.PrecisionAim = InputHeld(_shipActions.PrecisionAim);
        }

        private void PollPlayerInput()
        {
            InputInfo.PlaceDeployable = _playerActions.PlaceDeployable.triggered;
            InputInfo.EnterShip = _playerActions.EnterShip.triggered;
            InputInfo.PlayerAim = _playerActions.Aim.ReadValue<Vector2>();
            InputInfo.FireLaser = InputHeld(_playerActions.FireLaser);
            InputInfo.JumpHeld = InputHeld(_playerActions.Jump);
            InputInfo.JumpPressed = _playerActions.Jump.triggered;
            PollMoveInput();
        }

        private void PollMoveInput() 
        {
            Vector2 currentDirectionalInput = _playerActions.Move.ReadValue<Vector2>();

            // D-Pad does not detect a new button press if finger is slid from one direction to another without letting go
            // Checking against the previous move input will fix this issue

            // Checking if d-pad was either released or pressed right previously, and is now pressed left
            if (currentDirectionalInput.x > 0 && _previousDirectionalInput.x <= 0)
            {
                InputInfo.Move.x = 1;
            }
            // Checks if d-pad was either released or pressed left previously, and is now pressed left
            else if (currentDirectionalInput.x < 0 && _previousDirectionalInput.x >= 0)
            {
                InputInfo.Move.x = -1;
            }
            // Checks if the d-pad was previously pressed left or right and is now released (or pressed up or down)
            else if (currentDirectionalInput.x == 0 && _previousDirectionalInput.x != 0)
            {
                InputInfo.Move.x = 0;
            }

            // Doing the same checks but for y axis
            if (currentDirectionalInput.y > 0 && _previousDirectionalInput.y <= 0)
            {
                InputInfo.Move.y = 1;
            }
            else if (currentDirectionalInput.y < 0 && _previousDirectionalInput.y >= 0)
            {
                InputInfo.Move.y = -1;
            }
            else if (currentDirectionalInput.y == 0 && _previousDirectionalInput.y != 0)
            {
                InputInfo.Move.y = 0;
            }


            _previousDirectionalInput = currentDirectionalInput;

            // Follow this same process for y value if we want that functionality (maybe for climbing ladders)
        }

        private static bool InputHeld(InputAction action)
        {
            return action.ReadValue<float>() > 0;
        }

        private static bool ButtonPressed(InputAction action)
        {
            return action.triggered;
        }
    }
}

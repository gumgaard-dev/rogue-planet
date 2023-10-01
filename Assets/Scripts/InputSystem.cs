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

            if (currentMoveInput.x > 0)
            {
                _player.State.SetHorizontalInput(1);
            }
            else if (currentMoveInput.x < 0)
            {
                _player.State.SetHorizontalInput(-1);
            }
            else
            {
                _player.State.SetHorizontalInput(0);
            }
        }

        private void PollJumpInput() 
        {
            float currentJumpInput = _jumpAction.ReadValue<float>();

            _player.State.SetJumpInput(currentJumpInput);
        }

        void OnEnable()
        {
            
        }

        void OnDisable()
        {
            
        }
    }

}

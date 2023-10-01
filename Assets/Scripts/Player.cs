using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Capstone
{
    public class Player : MonoBehaviour
    {

        public PlayerBaseState State { get; private set; }
        public PlayerStateType StateType;

        private Dictionary<PlayerStateType, PlayerBaseState> _playerStates;

        public void AwakeManaged()
        {
            
        }

        public void StartManaged() 
        {
            _playerStates = new Dictionary<PlayerStateType, PlayerBaseState>
            {
                [PlayerStateType.Move] = new PlayerMoveState(this),
            };

            SetState(PlayerStateType.Move);
        }

        public void UpdateManaged()
        {

        }

        public void FixedUpdateManaged()
        {

        }

        public void SetState(PlayerStateType stateType)
        {
            StateType = stateType;
            State = _playerStates[stateType];

            State.Init();
        }
    }
}

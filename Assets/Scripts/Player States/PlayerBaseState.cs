using Capstonme;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Capstone
{
    public abstract class PlayerBaseState
    {

        protected Player Player;

        protected InputInfo InputInfo;

        public PlayerBaseState(Player player) 
        {
            Player = player;

            InputInfo = player.GetComponent<InputInfo>();

        }

        public virtual void Init() { }

        public virtual void UpdateManaged() { }

        public virtual void FixedUpdateManaged() { }

        public virtual void SetHorizontalInput(float inputValue) 
        {
            InputInfo.Move.x = inputValue;
        }

        public virtual void SetVerticalInput(float inputValue)
        {
            InputInfo.Move.y = inputValue;
        }

        public virtual void SetJumpInput(float inputValue)
        {
            InputInfo.Jump = inputValue;
        }

    }
}

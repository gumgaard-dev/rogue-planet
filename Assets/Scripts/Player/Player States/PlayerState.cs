using Capstonme;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Capstone
{
    public abstract class PlayerState
    {
        protected GameSettings Settings;
        protected Player Player;

        protected InputInfo InputInfo;
        protected TriggerInfo TriggerInfo;

        private LayerMask _surfaceLayerMask;
        private LayerMask _climbableLayerMask;

        protected float VelocityXDamped;

        public PlayerState(GameSettings settings, Player player) 
        {
            Settings = settings;
            Player = player;

            InputInfo = player.GetComponent<InputInfo>();
            TriggerInfo = player.GetComponent<TriggerInfo>();

            _surfaceLayerMask = LayerMask.GetMask("Surface");
            _climbableLayerMask = LayerMask.GetMask("Climbable");

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

        public void UpdateTriggers() 
        { 
            TriggerInfo.ResetTriggers();

            // If there is no collision this will be null, because it was reset above
            UpdateGroundTriggers();
        }

        public void UpdateGroundTriggers()
        {
            // This function checks if the box we specify overlaps with any colliders, and returns the first one it finds.
            TriggerInfo.Ground = Physics2D.OverlapBox(
                TriggerInfo.GroundBounds.center, TriggerInfo.GroundBounds.size, 0f, _surfaceLayerMask
            );
        }

        public void ResetVelocityXDamping()
        {
            VelocityXDamped = 0;
        }

    }
}

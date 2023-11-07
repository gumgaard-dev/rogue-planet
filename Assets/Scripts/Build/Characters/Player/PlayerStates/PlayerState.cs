using Capstone.Input;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    public abstract class PlayerState
    {
        protected GameSettings Settings;
        protected Player Player;

        protected InputInfo InputInfo;
        protected TriggerInfo TriggerInfo;

        private LayerMask _surfaceLayerMask;

        protected float VelocityXDamped;
        protected float VelocityYDamped;

        public PlayerState(GameSettings settings, Player player) 
        {
            Settings = settings;
            Player = player;

            InputInfo = player.GetComponent<InputInfo>();
            TriggerInfo = player.GetComponent<TriggerInfo>();

            _surfaceLayerMask = LayerMask.GetMask("Terrain");

        }

        public virtual void Enter() { }

        public virtual void Exit() { }

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

        public virtual void SetJumpInput(bool inputValue)
        {
            InputInfo.Jump = inputValue;
        }

        public virtual void SetShootInput(bool inputValue)
        {
            InputInfo.Shoot = inputValue;
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

        internal void ResetVelocityYDamping()
        {
            VelocityYDamped = 0;
        }

        internal void SetAimInput(Vector2 value)
        {
            Debug.Log(value.ToString());
            InputInfo.Aim = value;
        }
    }
}

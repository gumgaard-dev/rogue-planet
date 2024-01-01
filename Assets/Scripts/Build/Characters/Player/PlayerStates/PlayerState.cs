using Capstone.Input;
using System;
using UnityEngine;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    public abstract class PlayerState
    {
        protected GameSettings Settings;
        protected Player Player;

        protected TriggerInfo TriggerInfo;

        private LayerMask _surfaceLayerMask;

        protected float VelocityXDamped;
        protected float VelocityYDamped;

        public PlayerState(GameSettings settings, Player player) 
        {
            Settings = settings;
            Player = player;

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

        public virtual void SetEnterShipInput (bool inputValue)
        {
            InputInfo.EnterShip = inputValue;
        }

        public virtual void SetExitShipInput(bool inputValue)
        {
            InputInfo.ExitShip = inputValue;
        }

        public virtual void SetJumpInput(bool inputValue)
        {
            InputInfo.JumpHeld = inputValue;
        }

        public virtual void SetFireLaserInput(bool inputValue)
        {
            InputInfo.FireLaser = inputValue;
        }

        public virtual void SetShootInput(bool inputValue)
        {
            InputInfo.ShootHeld = inputValue;
        }

        public virtual void SetShipAimInput(float inputValue)
        {
            InputInfo.ShipAim = inputValue;
        }

        public virtual void SetPrecisionAimInput(bool inputValue)
        {
            InputInfo.PrecisionAim = inputValue;
        }

        public virtual void SetOpenUpgradeMenuInput(bool inputValue)
        {
            InputInfo.OpenUpgradeMenu = inputValue;
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
            InputInfo.PlayerAim = value;
        }

        internal void SetPlaceDeployableInput(bool triggered)
        {
            InputInfo.PlaceDeployable = triggered;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(GameSettings settings, Player player) : base(settings, player) { }

        public override void Enter()
        {
            Player.SetAnimation("Idle");
            Player.SetGravityScale(Settings.DefaultGravityScale);
        }

        public override void UpdateManaged()
        {

            UpdateTriggers();

            if (InputInfo.Directional.y > 0 && Player.IsNearShip)
            {
                Player.SetState(PlayerStateType.InShip);
            }
            // Checking for down input, and that player is on the ground
            else if (InputInfo.Directional.y < 0 && TriggerInfo.Ground)
            {
                Player.SetState(PlayerStateType.Duck);
            }
            else
            {
                if (TriggerInfo.Ground)
                {
                    Player.SetGravityScale(Settings.DefaultGravityScale);
                }
                else if (Player.Velocity.y <= -Settings.MinFallSpeed)
                {
                    Player.SetGravityScale(Settings.FallingGravityScale);
                }

                Player.UpdateFacing();
                Player.UpdateAnimation();
            }

        }

        public override void FixedUpdateManaged()
        {
            Vector2 newVelocity = Player.Velocity;

            // Smoothly changes the player's velocity
            // target velocity is input.x * run speed, becuase info.x is either -1, 0, or 1 based on input
            newVelocity.x = Mathf.SmoothDamp(
                Player.Velocity.x,
                InputInfo.Directional.x * Settings.RunSpeed,
                ref VelocityXDamped,
                TriggerInfo.Ground ? Settings.GroundSpeedSmoothTime : Settings.AirSpeedSmoothTime
            );

            Player.SetVelocity( newVelocity );

            // This should set the state to idle only when the player is still.
            //if (newVelocity.x == 0)
            //{
            //    Player.SetState(PlayerStateType.Idle);
            //}
        }

        public override void SetJumpInput(bool inputValue)
        {
            base.SetJumpInput(inputValue);

            if (inputValue)
            {
                // perform jump only when on ground
                if (TriggerInfo.Ground)
                {
                    Player.SetVelocity(Player.Velocity.x, Settings.JumpSpeed);
                }
            }
            else
            {
                if (Player.Velocity.y > 0)
                {
                    Player.SetGravityScale(Settings.FallingGravityScale);
                }            }
        }
    }
}
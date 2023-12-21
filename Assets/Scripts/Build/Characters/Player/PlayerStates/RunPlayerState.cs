using System;
using UnityEngine;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    public class RunPlayerState : PlayerState
    {
        public RunPlayerState(GameSettings settings, Player player) : base(settings, player) { }

        public override void Enter()
        {
            Player.SetGravityScale(Settings.DefaultGravityScale);
        }

        public override void UpdateManaged()
        {

            // Note: could set animation for running here after checking for minRunSpeed, if we want the animation not to start right away

            UpdateTriggers();

            if (InputInfo.Move.y > 0 && Player.IsNearShip) { Player.SetState(PlayerStateType.InShip); }

            else if (InputInfo.Jump && Player.Jetpack.HasFuel()) { Player.SetState(PlayerStateType.Jetpack); }

            else if (!TriggerInfo.Ground) { Player.SetState(PlayerStateType.Fall); }
            
            else { Player.UpdateFacing(); }

            if (InputInfo.PlaceDeployable)
            {
                Player.PlaceDeployable();
            }

        }


        // This function sets the player's velocity and checks for idle condition
        public override void FixedUpdateManaged()
        {

            Vector2 newVelocity = Player.Velocity;

            // Smoothly changes the player's velocity
            // target velocity is input.x * run speed, becuase info.x is either -1, 0, or 1 based on input
            newVelocity.x = Mathf.SmoothDamp(
                Player.Velocity.x,
                InputInfo.Move.x * Settings.RunSpeed,
                ref VelocityXDamped,
                TriggerInfo.Ground ? Settings.GroundSpeedSmoothTime : Settings.AirSpeedSmoothTime
            );

            Player.SetVelocity(newVelocity);

            if (InputInfo.Move.x == 0 && Player.Velocity == Vector2.zero)
            {
                Player.SetState(PlayerStateType.Idle);
            }
            else if (!TriggerInfo.Ground)
            {
                Player.SetState(PlayerStateType.Fall);
            }

        }

        public override void SetJumpInput(bool inputValue)
        {
            base.SetJumpInput(inputValue);
        }
    }
}
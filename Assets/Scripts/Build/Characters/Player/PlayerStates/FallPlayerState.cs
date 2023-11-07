using UnityEngine;

namespace Capstone.Build.Characters.Player.PlayerStates
{

    public class FallPlayerState : PlayerState
    {
        public FallPlayerState(GameSettings settings, Player player) : base(settings, player) { }

        public override void Enter()
        {
            Player.SetGravityScale(Settings.FallingGravityScale);
        }

        public override void Exit()
        {
            Player.SetGravityScale(Settings.DefaultGravityScale);
        }

        public override void UpdateManaged()
        {

            UpdateTriggers();

            if (TriggerInfo.Ground)
            {
                Player.SetState(PlayerStateType.Idle);
            }
            else if (InputInfo.Jump && Player.Jetpack.HasFuel())
            {
                Player.SetState(PlayerStateType.Jetpack);
            }
            else
            {
                Player.UpdateFacing();
            }

        }

        public override void FixedUpdateManaged()
        {

            Vector2 newVelocity = Player.Velocity;

            // Smoothly changes the player's velocity
            // target velocity is input.x * run speed, becuase info.x is either -1, 0, or 1 based on input
            newVelocity.x = Mathf.SmoothDamp(
                Player.Velocity.x,
                InputInfo.Move.x * Settings.RunSpeed,
                ref VelocityXDamped,
                Settings.AirSpeedSmoothTime
            );

            newVelocity.y = Player.Velocity.y;

            Player.SetVelocity(newVelocity);
        }
    }
}
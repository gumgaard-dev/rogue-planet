using Capstone.Input;
using UnityEngine;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    public class DuckPlayerState : PlayerState
    {
        public DuckPlayerState(GameSettings settings, Player player) : base(settings, player)
        {
        }

        public override void Enter()
        {
            Player.SetGravityScale(Settings.DefaultGravityScale);
        }

        public override void UpdateManaged()
        {
            UpdateTriggers();

            if (!TriggerInfo.Ground)
            {
                Player.SetState(PlayerStateType.Run);
            }
        }

        public override void FixedUpdateManaged()
        {
            Vector2 newVelocity = Player.Velocity;

            // Smoothly changes the player's velocity
            // target velocity is input.x * run speed, becuase info.x is either -1, 0, or 1 based on input
            newVelocity.x = Mathf.SmoothDamp(
                Player.Velocity.x,
                0,
                ref VelocityXDamped,
                Settings.GroundSpeedSmoothTime
            );

            Player.SetVelocity(newVelocity);
        }

        public override void SetVerticalInput(float inputValue)
        {
            base.SetVerticalInput(inputValue);

            if (InputInfo.Move.y >= 0)
            {
                // TODO Switch this to idle
                Player.SetState(PlayerStateType.Run);
            }
        }
    }
}
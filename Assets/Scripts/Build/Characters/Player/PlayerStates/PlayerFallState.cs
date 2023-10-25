using UnityEngine;

namespace Capstone.Build.Characters.Player.PlayerStates
{

    public class PlayerFallState : PlayerState
    {
        public PlayerFallState(GameSettings settings, Player player) : base(settings, player) { }

        public override void Enter()
        {
            Player.SetAnimation("Fall");
            // Can maybe get rid of gravity scales unless we want custom gravity
            Player.SetGravityScale(Settings.DefaultGravityScale);
        }

        public override void UpdateManaged()
        {

            UpdateTriggers();

            if (TriggerInfo.Ground)
            {
                
                if (InputInfo.Directional.x == 0 && Player.Velocity == Vector2.zero)
                {

                    Player.SetState(PlayerStateType.Idle);
                }
                else
                {
                    Player.SetState(PlayerStateType.Run);
                }
            }
            else if (InputInfo.Jump)
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
                InputInfo.Directional.x * Settings.RunSpeed,
                ref VelocityXDamped,
                TriggerInfo.Ground ? Settings.GroundSpeedSmoothTime : Settings.AirSpeedSmoothTime
            );

        }

    }
}
using UnityEngine;

namespace Capstone.Build.Characters.Player.PlayerStates
{

    public class PlayerJetpackState : PlayerState
    {
        public PlayerJetpackState(GameSettings settings, Player player) : base(settings, player) { }

        public override void UpdateManaged()
        {
            UpdateTriggers();
            Player.UpdateFacing();
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

            if (InputInfo.Jump)
            {
                //Player.SetVelocity(Player.Velocity.x, Settings.JetpackSpeed);
                // Handling jetpack movement
                int yModifier = InputInfo.Jump ? 1 : 0;

                newVelocity.y = Mathf.SmoothDamp(
                    Player.Velocity.y,
                    yModifier * Settings.JetpackSpeed,
                    ref VelocityYDamped,
                    Settings.AirSpeedSmoothTime
                );
            }

            Player.SetVelocity(newVelocity);


            if (InputInfo.Directional.x == 0 && Player.Velocity == Vector2.zero)
            {
                Player.SetState(PlayerStateType.Idle);
            } else if (Player.Velocity.y < Settings.MinFallSpeed)
            {
                Player.SetState(PlayerStateType.Fall);
            }

        }

    }

}

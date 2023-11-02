using UnityEngine;

namespace Capstone.Build.Characters.Player.PlayerStates
{

    public class PlayerJetpackState : PlayerState
    {

        private Jetpack _jetpack;

        public PlayerJetpackState(GameSettings settings, Player player, Jetpack jetpack) : base(settings, player) 
        { 
            _jetpack = jetpack;
        }

        override public void Enter()
        {
            _jetpack.RechargeTimer = 0;
        }

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

            if (InputInfo.Jump && _jetpack.FuelLevel > 0)
            {
                // This has to be done from the state or the time difference between this function call and Jetpack FixedUpdate call will cause total jetpack time to be off
                _jetpack.FuelLevel -= 1;

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
            } else if (!InputInfo.Jump)
            {
                Player.SetState(PlayerStateType.Fall);
            }

        }

    }

}

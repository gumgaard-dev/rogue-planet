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
            _jetpack.InitializeTimer();
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
                Settings.AirSpeedSmoothTime
            );

            if (InputInfo.Jump && _jetpack.HasFuel())
            {
                // This has to be done from the state or the time difference between this function call and Jetpack FixedUpdate call will cause total jetpack time to be off
                _jetpack.ConsumeFuel();


                float thrust = _jetpack.CalculateThrust();

                int yModifier = InputInfo.Jump ? 1 : 0;

                newVelocity.y = Mathf.SmoothDamp(
                    Player.Velocity.y,
                    yModifier * thrust,
                    ref VelocityYDamped,
                    Settings.AirSpeedSmoothTime
                );

                newVelocity.y = _jetpack.CalculateThrust();
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

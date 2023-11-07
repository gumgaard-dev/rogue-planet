using UnityEngine;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    public class JetpackPlayerState : PlayerState
    {

        public JetpackPlayerState(GameSettings settings, Player player) : base(settings, player) { }

        override public void Enter()
        {
            Player.Jetpack.ActivateThrustParticles();
        }

        override public void Exit()
        {
            Player.Jetpack.DeactivateThrustParticles();
        }

        public override void UpdateManaged()
        {
            UpdateTriggers();
            Player.UpdateFacing();
        }

        public override void FixedUpdateManaged()
        {
            // exit state on jump release
            if(!InputInfo.Jump || !Player.Jetpack.HasFuel())
            {
                Player.SetState(PlayerStateType.Fall);
                return;
            }


            Vector2 newVelocity = Player.Velocity;

            // Smoothly changes the player's velocity
            // target velocity is input.x * run speed, becuase info.x is either -1, 0, or 1 based on input
            newVelocity.x = Mathf.SmoothDamp(
                Player.Velocity.x,
                InputInfo.Move.x * Settings.RunSpeed,
                ref VelocityXDamped,
                Settings.AirSpeedSmoothTime
            );

            // This has to be done from the state or the time difference between this function call and Jetpack FixedUpdate call will cause total jetpack time to be off
            Player.Jetpack.ConsumeFuel();


            float thrust = Player.Jetpack.CalculateThrust();

            int yModifier = InputInfo.Jump ? 1 : 0;

            newVelocity.y = Mathf.SmoothDamp(
                Player.Velocity.y,
                yModifier * thrust,
                ref VelocityYDamped,
                Settings.AirSpeedSmoothTime
            );

            newVelocity.y = Player.Jetpack.CalculateThrust();

            Player.SetVelocity(newVelocity);
        }

    }

}

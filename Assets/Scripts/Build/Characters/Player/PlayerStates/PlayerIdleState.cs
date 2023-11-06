﻿namespace Capstone.Build.Characters.Player.PlayerStates
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(GameSettings settings, Player player) : base(settings, player) { }

        public override void Enter()
        {
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
            else if (InputInfo.Directional.x != 0)
            {
                Player.SetState(PlayerStateType.Run);
            }
            else
            {
                Player.UpdateFacing();
            }

        }

        public override void SetJumpInput(bool inputValue)
        {
            base.SetJumpInput(inputValue);

            if (inputValue)
            {
                //Player.SetVelocity(Player.Velocity.x, Settings.JetpackSpeed);
                Player.SetState(PlayerStateType.Jetpack);
            }

        }

    }
}

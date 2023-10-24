using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(GameSettings settings, Player player) : base(settings, player) { }

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
            else if ((InputInfo.Directional.x != 0 || InputInfo.Jump) && TriggerInfo.Ground)
            {
                Player.SetState(PlayerStateType.Move);
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
                }
            }
        }

    }
}

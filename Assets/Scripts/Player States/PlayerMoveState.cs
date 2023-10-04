using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(GameSettings settings, Player player) : base(settings, player) { }

        public override void Init()
        {
            Player.SetAnimation("Idle");
            Player.SetGravityScale(Settings.DefaultGravityScale);
        }

        public override void UpdateManaged()
        {
            Player.UpdateFacing();
            Player.UpdateAnimation();
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
                Settings.GroundSpeedSmoothTime
            );

            Player.SetVelocity( newVelocity );

            // This should set the state to idle only when the player is still.
            //if (newVelocity.x == 0)
            //{
            //    Player.SetState(PlayerStateType.Idle);
            //}
        }
    }
}
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
            newVelocity.x = Mathf.SmoothDamp(
                Player.Velocity.x,
                InputInfo.Move.x * Settings.RunSpeed,
                ref VelocityXDamped,
                Settings.GroundSpeedSmoothTime
            );

            Player.SetVelocity( newVelocity );
        }
    }
}
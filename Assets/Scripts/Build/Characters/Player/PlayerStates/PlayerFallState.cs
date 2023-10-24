using UnityEngine;

namespace Capstone.Build.Characters.Player.PlayerStates
{

    public class PlayerFallState : PlayerState
    {
        public PlayerFallState(GameSettings settings, Player player) : base(settings, player) { }

        public override void Enter()
        {
            Player.SetAnimation("Fall");
            Player.SetGravityScale(Settings.FallingGravityScale);
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
            else
            {
                Player.UpdateFacing();
            }
            //else if (InputInfo.Jump)
            //{
            //    // Jetpack
            //}

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



        // Putting this here for reference for jetpack state code
        //public override void SetJumpInput(bool inputValue)
        //{
        //    base.SetJumpInput(inputValue);

        //    if (inputValue)
        //    {
        //        // perform jump only when on ground
        //        if (TriggerInfo.Ground)
        //        {
        //            Player.SetVelocity(Player.Velocity.x, Settings.JumpSpeed);
        //        }
        //    }
        //    else
        //    {
        //        if (Player.Velocity.y > 0)
        //        {
        //            Player.SetGravityScale(Settings.FallingGravityScale);
        //        }
        //    }
        //}

    }
}
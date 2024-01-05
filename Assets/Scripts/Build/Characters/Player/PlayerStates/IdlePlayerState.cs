using Capstone.Input;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    public class IdlePlayerState : PlayerState
    {
        public IdlePlayerState(GameSettings settings, Player player) : base(settings, player) { }

        public override void Enter()
        {
            Player.SetGravityScale(Settings.DefaultGravityScale);
        }

        public override void UpdateManaged()
        {

            UpdateTriggers();

            if (InputInfo.EnterShip && Player.IsNearShip) { Player.SetState(PlayerStateType.InShip); }
                        
            else if (!TriggerInfo.Ground) { Player.SetState(PlayerStateType.Fall); }

            else if (InputInfo.JumpHeld && Player.Jetpack.HasFuel()) { Player.SetState(PlayerStateType.Jetpack); }
            
            else if (InputInfo.Move.x != 0) {  Player.SetState(PlayerStateType.Run); }
            
            else { Player.UpdateFacing(); }

            if (InputInfo.PlaceDeployable)
            {
                Player.PlaceDeployable();
            }
        }

        public override void SetJumpInput(bool inputValue)
        {
            base.SetJumpInput(inputValue);
        }

    }
}

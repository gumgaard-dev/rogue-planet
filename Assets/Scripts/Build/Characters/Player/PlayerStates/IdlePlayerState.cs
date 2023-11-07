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

            if (InputInfo.Move.y > 0 && Player.IsNearShip) { Player.SetState(PlayerStateType.InShip); }

            else if (InputInfo.Jump && Player.Jetpack.HasFuel()) { Player.SetState(PlayerStateType.Jetpack); }
            
            else if (InputInfo.Move.x != 0) {  Player.SetState(PlayerStateType.Run); }
            
            else { Player.UpdateFacing(); }

        }

        public override void SetJumpInput(bool inputValue)
        {
            base.SetJumpInput(inputValue);
        }

    }
}

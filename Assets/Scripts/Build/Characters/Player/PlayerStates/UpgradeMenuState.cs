using Capstone.Build.Characters.Player;
using Capstone.Build;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    public class UpgradeMenuState : PlayerState
    {

        public UpgradeMenuState(GameSettings settings, Player player) : base(settings, player) { }

        public override void Enter()
        {
            UpgradeMenuController.OpenUpgradeMenu();
            GameManager.PauseGame();
        }

        public override void Exit()
        {
            UpgradeMenuController.CloseUpgradeMenu();
            GameManager.ResumeGame();
        }

        public override void UpdateManaged()
        {
            if (InputInfo.Back)
            {
                bool shouldExitState = UpgradeMenuController.BackPressed();

                if (shouldExitState)
                {
                    Player.SetState(PlayerStateType.InShip);
                    return;
                }
            }

            if (InputInfo.MoveCursor != Vector2.zero)
            {
                UpgradeMenuController.CursorMoved(InputInfo.MoveCursor);
            }

            if (InputInfo.Confirm) 
            {
                UpgradeMenuController.ConfirmPressed();
            }
        }

    }
}

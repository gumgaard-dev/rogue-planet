using Capstone.Build.Characters.Player;
using Capstone.Build;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    public class UpgradeMenuState : PlayerState
    {
        public UIInputActions UIInputActions = new();
        public UIInputActions.UIActions UIActions;

        public UpgradeMenuState(GameSettings settings, Player player) : base(settings, player)
        {
            UIActions = this.UIInputActions.UI;
        }
        public override void Enter()
        {
            UpgradeMenuController.OpenUpgradeMenu();
            GameManager.PauseGame();
            UIActions.Enable();
        }

        public override void Exit()
        {
            UIActions.Disable();

            UpgradeMenuController.CloseUpgradeMenu();
            GameManager.ResumeGame();
        }

        public override void UpdateManaged()
        {
            if (UIActions.Cancel.triggered)
            {
                // executes cancel action, returns true if menu closes
                // if the player is in a sub-menu, UpgradeMenuController returns false and navigates to parent menu
                bool shouldExitState = UpgradeMenuController.BackPressed();

                if (shouldExitState)
                {
                    Player.SetState(PlayerStateType.InShip);
                    return;
                }
            }
        }
    }
}

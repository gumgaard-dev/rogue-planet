using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    
    public class InShipState : PlayerState
    {
        private Ship.Ship _ship;
        public InShipState(GameSettings settings, Player player) : base(settings, player) {}

        // this is to solve a bug that arises from using the same button to enter and exit ship
        private readonly float _timeUntilPlayerCanExitShip = 1f;
        private float _timeSinceEnteringState;

        public override void Enter()
        {
            _timeSinceEnteringState = 0;
            // notify listeners that the player has entered the ship
            Player.EnterShip?.Invoke();

            // cache ship ref
            this._ship = Player.Ship;

            // deactivate visuals and physics of player until they exit the ship
            if (Player.gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = false;
            }

            if (Player.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
            {
                Player.transform.position = _ship.transform.position;
                rb.bodyType = RigidbodyType2D.Static;
            }

            // set player's position to ship position
            Player.SetPosition(this._ship.transform.position);
        }

        public override void Exit()
        {
            if (Player.StateType == PlayerStateType.UpgradeMenu) { return; }


            // notify listeners that the player has exited the ship
            Player.ExitShip?.Invoke();

            // reactivate player visuals and physics
            if (Player.gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = true;
            }

            if (Player.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.WakeUp();
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        public override void UpdateManaged()
        {
            if (InputInfo.OpenUpgradeMenu)
            {
                Player.SetState(PlayerStateType.UpgradeMenu);
            }
        }

        public override void FixedUpdateManaged()
        {
            // pass aiming input to ship
            // if keyboard input, pass that
            // else pass controller input
            if (InputInfo.AimShip != 0)
            {
                this._ship.HandleRotationInput(InputInfo.AimShip, InputInfo.PrecisionAim);
            }
            
            this._ship.HandleShootInput(InputInfo.Shoot);

            if(InputInfo.ExitShip)
            {
                if (_timeSinceEnteringState >= _timeUntilPlayerCanExitShip)
                {
                    Player.SetState(PlayerStateType.Run);
                }
                
            } else
            {
                _timeSinceEnteringState += Time.fixedDeltaTime;
            }
        }
    }
}
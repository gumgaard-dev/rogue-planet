using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Capstone
{
    
    public class InShipState : PlayerState
    {
        private Ship _ship;
        public InShipState(GameSettings settings, Player player) : base(settings, player) {}

        public override void Enter()
        {
            // cache ship ref
            this._ship = Player.Ship;

            // deactivate visuals and physics of player until they exit the ship
            if (Player.gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = false;
            }

            if (Player.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.Sleep();
            }

            // set player's position to ship position
            Player.SetPosition(this._ship.transform.position);
        }

        public override void Exit()
        {

            // reactivate player visuals and physics
            if (Player.gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = true;
            }

            if (Player.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.WakeUp();
            }
        }

        public override void FixedUpdateManaged()
        {

            // pass input to ship 
            this._ship.HandleRotationInput(InputInfo.Directional.x);
            this._ship.HandleShootInput(InputInfo.Jump);

            if(InputInfo.Directional.y < 0)
            {
                Player.SetState(PlayerStateType.Move);
            }
        }
    }
}
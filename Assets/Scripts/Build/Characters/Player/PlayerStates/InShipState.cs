using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Capstone.Build.Characters.Player.PlayerStates
{
    
    public class InShipState : PlayerState
    {

        public InShipState(GameSettings settings, Player player) : base(settings, player) {}

        public override void Enter()
        {
            if (Player.gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = false;
            }

            if (Player.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.Sleep();
            }
        }

        public override void Exit()
        {
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
            if (InputInfo.Directional.x != 0)
            {
                Debug.Log("Rotating!");
            }
            Player.ship.HandleRotationInput(InputInfo.Directional.x);
            Player.ship.HandleShootInput(InputInfo.Jump);

            if(InputInfo.Directional.y < 0)
            {
                Player.SetState(PlayerStateType.Move);
            }
        }
    }
}
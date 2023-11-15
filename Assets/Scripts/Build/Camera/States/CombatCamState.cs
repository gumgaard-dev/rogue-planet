using Capstone.Build.Characters.Ship;
using UnityEngine;

namespace Capstone.Build.Cam
{
    public class CombatCamState : BaseCamState
    {
        public CombatCamState(Ship ship, Camera camera, float cameraSize) : base(camera, cameraSize)
        {
            this.target = ship.gameObject;
            
        }
        public override void MoveCamera()
        {
            UpdateZoom();

            if (this.target != null)
            {
                Cam.transform.position = Vector3.SmoothDamp(Cam.transform.position, _camTargetPosition, ref _camMoveVelocity, MoveSmoothTime);
            }

        }
    }
}

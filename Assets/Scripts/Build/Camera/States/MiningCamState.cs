using Capstone.Build.Characters.Player;
using System;
using UnityEngine;

namespace Capstone.Build.Cam
{
    [RequireComponent(typeof(Camera))]
    public class MiningCamState : BaseCamState
    {

        // Amount to offset in x-axis based on player's facing direction
        public float LookaheadAmount = 1;

        // for caching calculated and retrieved values
        private Vector3 _playerCurrentPosition;
        private float _playerFacingDirection;
        

        public MiningCamState(Player player, Camera camera, float cameraSize) : base(camera, cameraSize)
        {
            target = player.gameObject;
        }
        public override void MoveCamera()
        {
            if (_camCurrentSize != StateCamSize)
            {
                Cam.orthographicSize = Mathf.SmoothDamp(Cam.orthographicSize, StateCamSize, ref _camZoomVelocity, ZoomSmoothTime);
                CameraSizeChanged?.Invoke(Cam.orthographicSize);
            }

            if (_camTargetPosition != _camCurrentPosition)
            {
                Cam.transform.position = Vector3.SmoothDamp(_camCurrentPosition, _camTargetPosition, ref _camMoveVelocity, MoveSmoothTime);
            }
        }

        // Update is called once per frame
        public override void UpdateCachedVariables()
        {
            CamOffset.x *= target.transform.localScale.x;

            base.UpdateCachedVariables();

            if (target == null)
            {
                return;
            }

            

        }
    }
}


using Capstone.Build.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Capstone.Build.Cam
{
    [System.Serializable] public class CameraSizeChangedEvent : UnityEvent<float> { }
    public abstract class BaseCamState
    {
        public Camera Cam;

        public GameObject target;

        
        public float StateCamSize; // inverse correlation with zoom
        public Vector2 CamOffset;
        public float ZoomSmoothTime;
        public float MoveSmoothTime;

        protected float _camCurrentSize;
        protected Vector3 _camCurrentPosition;
        protected Vector3 _camTargetPosition;
        protected float _camZoomVelocity;
        protected Vector3 _camMoveVelocity;

        public CameraSizeChangedEvent CameraSizeChanged;

        public abstract void MoveCamera();

        public virtual void UpdateZoom()
        {
            if (_camCurrentSize != StateCamSize)
            {
                Cam.orthographicSize = Mathf.SmoothDamp(Cam.orthographicSize, StateCamSize, ref _camZoomVelocity, ZoomSmoothTime);
                CameraSizeChanged?.Invoke(Cam.orthographicSize);
            }
        }

        public virtual void UpdateCachedVariables()
        {
            _camCurrentSize = Cam.orthographicSize;
            _camCurrentPosition = Cam.transform.position;
            _camTargetPosition = new Vector3(target.transform.position.x + CamOffset.x, target.transform.position.y + CamOffset.y);
        }

        public BaseCamState(Camera camera, float cameraSize)
        {
            Cam = camera;
            StateCamSize = cameraSize;
        }
    }
}


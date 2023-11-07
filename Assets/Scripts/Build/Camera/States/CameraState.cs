using Capstone.Build.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CameraState
{
    public Camera Camera;

    // inverse correlation with zoom;
    public float CameraSize;
    private float _zoomVelocity;
    public float ZoomSmoothTime;
    public float MoveSmoothTime;
    public virtual void MoveCamera()
    {
        Camera.orthographicSize = Mathf.SmoothDamp(Camera.orthographicSize, CameraSize, ref _zoomVelocity, ZoomSmoothTime);
    }
    public virtual void UpdateCachedVariables() { }

    public CameraState(Camera camera, float cameraSize)
    {
        Camera = camera;
        CameraSize = cameraSize;
    }
}
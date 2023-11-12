using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCameraState : CameraState
{
    public Ship Ship;
    private Vector3 _velocity;

    public float VerticalOffset;

    public CombatCameraState(Ship ship, Camera camera, float cameraSize) : base(camera, cameraSize)
    {
        this.Ship = ship;
    }
    public override void MoveCamera()
    {
        if (this.Ship != null) 
        {
            base.MoveCamera();
            Vector3 targetPostion = new Vector3(Ship.transform.position.x, Ship.transform.position.y + VerticalOffset);
            Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, targetPostion, ref _velocity, MoveSmoothTime);
        }

    }
}
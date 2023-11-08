using Capstone.Build.Characters.Player;
using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MiningCameraState : CameraState
{
    
    public Player Player;

    // Amount to offset in x-axis based on player's facing direction
    public float LookaheadAmount = 1;

    // for caching calculated and retrieved values
    private Vector3 _targetCurrentPosition;
    private Vector3 _cameraCurrentPosition;
    private float _playerFacingDirection;
    private Vector3 _lookaheadVector;
    private Vector3 _velocity;

    public MiningCameraState(Player player, Camera camera, float cameraSize) : base(camera, cameraSize)
    {
        Player = player;
    }
    public override void MoveCamera()
    {
        base.MoveCamera();
        Camera.transform.position = Vector3.SmoothDamp(_cameraCurrentPosition, _targetCurrentPosition + _lookaheadVector, ref _velocity, MoveSmoothTime);
    }

    // Update is called once per frame
    public override void UpdateCachedVariables()
    {
        if (Player == null)
        {
            return;
        }

        bool directionHasChanged = UpdatePlayerFacingDirection();

        if (directionHasChanged)
        {
            UpdateLookaheadVector();
        }

        UpdatePositions();
    }

    private void UpdateLookaheadVector()
    {
        _lookaheadVector = new Vector3(LookaheadAmount * _playerFacingDirection, 0, 0);
    }

    private void UpdatePositions()
    {
        _targetCurrentPosition = Player.transform.position;
        _cameraCurrentPosition = Camera.transform.position;
    }

    // updates cached direction variable, returns true if direction has changed
    public bool UpdatePlayerFacingDirection()
    {
       if (this._playerFacingDirection != Player.Facing)
        {
            _playerFacingDirection = Player.Facing;
            return true;
        } else
        {
            return false;
        }
    }


}

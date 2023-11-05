using Capstone.Build.Characters.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Follow : MonoBehaviour
{

    [SerializeField] private Player _target;

    [Header("How long to reach the target's position")]
    [SerializeField] private float _smoothTime;

    [Header("Max distance between target and center of camera")]
    [SerializeField] private float _maxOffsetX = 0f;
    [SerializeField] private float _maxOffsetY = 0f;

    [Header("Amount to offset in x-axis based on player's facing direction")]
    [SerializeField] private float _lookaheadAmount;

    // for caching calculated and retrieved values
    private Vector3 _targetCurrentPosition;
    private Vector3 _cameraCurrentPosition;
    private float _playerFacingDirection;
    private Vector3 _lookaheadVector;
    private Vector3 _velocity;

    void Awake()
    {
        if (_target == null)
        {
            Debug.Log("Camera->Follow: no target transform set in the inspector.");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        bool directionHasChanged = UpdatePlayerFacingDirection();

        if (directionHasChanged)
        {
            Debug.Log("Direction has changed");
            Debug.Log(_playerFacingDirection);
            UpdateLookaheadVector();
        }

        UpdatePositions();

        MoveCamera();

/*        if (!TargetIsWithinBounds() || directionHasChanged) { MoveCamera(); }*/

        
    }

    private void UpdateLookaheadVector()
    {
        _lookaheadVector = new Vector3(_lookaheadAmount * _playerFacingDirection, 0, 0);
    }

    private void MoveCamera()
    {
        transform.position = Vector3.SmoothDamp(_cameraCurrentPosition, _targetCurrentPosition + _lookaheadVector, ref _velocity, _smoothTime);
    }

    private void UpdatePositions()
    {
        _targetCurrentPosition = _target.transform.position;
        _cameraCurrentPosition = this.transform.position;
    }

    private bool TargetIsWithinBounds()
    {
        Vector3 distanceToTarget = _targetCurrentPosition - _cameraCurrentPosition;

        bool withinXBounds = Math.Abs(distanceToTarget.y) < _maxOffsetY;
        bool withinYBounds = Math.Abs(distanceToTarget.x) < _maxOffsetX + _lookaheadAmount;

        return withinXBounds && withinYBounds; 
    }

    // updates cached direction variable, returns true if direction has changed
    public bool UpdatePlayerFacingDirection()
    {
       if (this._playerFacingDirection != _target.Facing)
        {
            _playerFacingDirection = _target.Facing;
            return true;
        } else
        {
            return false;
        }
    }
}

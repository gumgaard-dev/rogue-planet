using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    private Vector2 _currentCamPosition, _lastCamPosition, _camDistanceSinceLastUpdate;
    private bool _currentlyOnCamera;
    public float ParallaxSpeed;

    private void LateUpdate()
    {
        if(_currentlyOnCamera)
        {
            CalculateCameraMovement();
            Vector3 parallaxTranslation = new Vector3(_camDistanceSinceLastUpdate.x, _camDistanceSinceLastUpdate.y, 0) * ParallaxSpeed;
            this.transform.Translate(parallaxTranslation, Space.World);
        }
    }

    private void CalculateCameraMovement()
    {
        UpdateCurrentCamPosition();
        
        if (_lastCamPosition == _currentCamPosition)
        {
            _camDistanceSinceLastUpdate = Vector2.zero;
        }
        else
        {
            _camDistanceSinceLastUpdate = _currentCamPosition - _lastCamPosition;
        }

        UpdateLastCamPosition();
    }

    private void UpdateCurrentCamPosition()
    {
        _currentCamPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
    }

    private void UpdateLastCamPosition()
    {
        _lastCamPosition = _currentCamPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UpdateCurrentCamPosition();
        UpdateLastCamPosition();
        _currentlyOnCamera = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _currentlyOnCamera = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _camDistanceSinceLastUpdate = Vector3.zero;
        _currentlyOnCamera = false;
    }
}

using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent (typeof(BoxCollider2D))]
public class ParallaxCameraBox : MonoBehaviour
{
    private Camera _camera;
    private BoxCollider2D _boxCollider;
    private float _sizeX, _sizeY, _xyRatio;
    
    void Awake()
    {
        if (!TryGetComponent(out _camera))
        {
            Debug.Log("CameraCalculateBox: no camera detected, please attach this script to a camera object");
        }

        if(!TryGetComponent(out _boxCollider))
        {
            Debug.Log("CameraCalculateBox: no box collider detected, please attach one in the inspector");

        }
    }
    
    void Start()
    {
        _xyRatio = (float)Screen.width / Screen.height;
        _sizeY = _camera.orthographicSize * 2;
        _sizeX = _sizeY * _xyRatio;
        _boxCollider.size = new Vector2(_sizeX, _sizeY);
    }
}

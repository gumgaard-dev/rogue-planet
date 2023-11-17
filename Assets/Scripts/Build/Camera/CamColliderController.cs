using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent (typeof(BoxCollider2D))]
public class CamColliderController : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private float _sizeX, _sizeY, _xyRatio;
    
    void Awake()
    {
        if(!TryGetComponent(out _boxCollider))
        {
            Debug.Log("CameraCalculateBox: no box collider detected, please attach one in the inspector");

        }
    }

    public void UpdateColliderBounds(float newSize)
    {
        if (_boxCollider != null)
        {
            _xyRatio = (float)Screen.width / Screen.height;
            _sizeY = newSize * 2;
            _sizeX = _sizeY * _xyRatio;
            _boxCollider.size = new Vector2(_sizeX, _sizeY);
        }
    }
}

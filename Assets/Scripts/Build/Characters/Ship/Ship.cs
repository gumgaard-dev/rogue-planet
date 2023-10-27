using Build.Component;
using UnityEngine;

[RequireComponent(typeof(HealthData))]
public class Ship : MonoBehaviour
{
    private HealthData _healthData;
    public Gun Gun;

    private const float ROTATION_INPUT_MOD = -1f;

    public HingeRotator Arm;
    private float _rotationInput;

    public void Start()
    {
        this._healthData = GetComponent<HealthData>();
    }

    // should be controlled by a player only, and should not look for input directly
    public void HandleRotationInput(float direction)
    {
        if (direction != 0f)
        {
            Arm.Rotate(direction * ROTATION_INPUT_MOD);
        }
    }

    public void HandleShootInput(bool input)
    {
        if (Gun != null && input)
        {
            Gun.Shoot();
        }
    }
}

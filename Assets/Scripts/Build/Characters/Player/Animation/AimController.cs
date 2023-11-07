using UnityEngine;
using Capstone.Input; // Import namespace to reference InputSystem
using UnityEngine.InputSystem;
using Capstone.Build.Characters.Player;

public class PlayerAimController : MonoBehaviour
{
    private Player _player;
    public Transform bone; // Assign the bone you want to rotate in the inspector
    public float rotationSpeed = 5f; // Adjust rotation speed as needed
    private InputInfo _inputInfo;
    private Vector2 aimDirection;
    public bool IsAiming;
    public float AimXDirection { get; private set; }

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        // Get the InputInfo component from the player game object
        _inputInfo = GetComponentInParent<InputInfo>();

        if (_inputInfo == null)
        {
            Debug.LogError("InputInfo component not found on parent game object.");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Retrieve the aim direction from the input system
        aimDirection = _inputInfo.Aim;
        Debug.Log(aimDirection.ToString());
        AimXDirection = aimDirection.normalized.x;
        if (aimDirection.sqrMagnitude > 0.01f)
        {
            IsAiming = true;               
            RotateBoneTowardsDirection(new Vector2(aimDirection.x, aimDirection.y));
        }
        else 
        { 
            IsAiming = false;
            RotateBoneTowardsDirection(Vector2.down); // put arm down if not aiming
        }
    }

    void RotateBoneTowardsDirection(Vector2 direction)
    {
        // Use the player's Facing property to determine the direction the player is facing
        bool isFacingLeft = _player.Facing < 0;

        // Calculate the angle from the aim direction
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // If the player is facing left, we need to mirror the rotation around the y-axis.
        if (isFacingLeft)
        {
            // This effectively rotates the bone correctly when the player sprite is flipped
            angle = (180f - angle) * -1;
        }

        // Create a target rotation based on the calculated angle
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Apply the rotation to the bone
        bone.rotation = Quaternion.Slerp(bone.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
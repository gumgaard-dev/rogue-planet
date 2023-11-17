using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HingeRotator : MonoBehaviour
{
    public float torqueToApply = 20f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Rotate(float rotationDirection)
    {
        rb.AddTorque(torqueToApply * rotationDirection);
    }

    public void RotateTo(Vector2 point)
    {
        Vector3 rotationVector = point - rb.position;
        float zRotation = Mathf.Atan2(rotationVector.y, rotationVector.x) * Mathf.Rad2Deg;

        this.rb.rotation = zRotation;
    }
}
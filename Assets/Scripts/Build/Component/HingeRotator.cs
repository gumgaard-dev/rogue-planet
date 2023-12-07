using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HingeRotator : MonoBehaviour
{
    public float NormalTorque = 20f;
    public float PrecisionTorque = 5f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Rotate(float rotationDirection)
    {
        rb.AddTorque(NormalTorque * rotationDirection);
    }

    public void Rotate(float rotationDirection, bool precisionMode)
    {
        if (precisionMode)
        {
            rb.AddTorque(PrecisionTorque * rotationDirection);
        }
        else
        {
            rb.AddTorque(NormalTorque * rotationDirection);
        }

    }

    public void RotateTo(Vector2 point)
    {
        Vector3 rotationVector = point - rb.position;
        float zRotation = Mathf.Atan2(rotationVector.y, rotationVector.x) * Mathf.Rad2Deg;

        this.rb.rotation = zRotation;
    }
}
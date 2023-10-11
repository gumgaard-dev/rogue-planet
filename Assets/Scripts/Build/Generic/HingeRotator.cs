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
}
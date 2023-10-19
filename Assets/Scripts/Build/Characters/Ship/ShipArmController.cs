using UnityEngine;

public class ShipArmController : MonoBehaviour
{

    private const float ROTATION_INPUT_MOD = -1f;

    public HingeRotator rotator;
    private float rotationInput;
    

    // Start is called before the first frame update
    void Start()
    {
        if (rotator == null)
        {
            Debug.Log("ShipArmController: no rotator hinge set in inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PollInput();
        if(rotationInput != 0f)
        {
            // direction is the opposite of input, so that right=clockwise, left=counter
            float rotationDirection = rotationInput * ROTATION_INPUT_MOD;
            rotator.Rotate(rotationDirection);
        }

    }

    private void PollInput()
    {
        rotationInput = Input.GetAxis("Horizontal");
    }
}

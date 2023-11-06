using UnityEngine;
using UnityEngine.U2D.IK;

public class DynamicFootPlacement : MonoBehaviour
{
    public IKManager2D ikManager;
    public Solver2D leftFootSolver;
    public Solver2D rightFootSolver;
    public Transform leftFootIKTarget;
    public Transform rightFootIKTarget;
    public LayerMask groundLayer;
    public float stepThreshold = 0.1f; // How far the player has to move before a step is taken
    public float stepRaycastDistance = 1f;
    public float stepSpeed = 5f;

    private Vector2 lastPlayerPosition;

    void Start()
    {
        lastPlayerPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector2 currentPlayerPosition = transform.position;
        float movementSinceLastStep = (lastPlayerPosition - currentPlayerPosition).magnitude;

        // If the player has moved enough, try placing the feet
        if (movementSinceLastStep > stepThreshold)
        {
            TryPlaceFoot(leftFootSolver, leftFootIKTarget);
            TryPlaceFoot(rightFootSolver, rightFootIKTarget);
            lastPlayerPosition = currentPlayerPosition; // Update the last position
        }

        ikManager.UpdateManager(); // Force IK manager to update solvers
    }

    void TryPlaceFoot(Solver2D solver, Transform footIKTarget)
    {
        // Cast a ray down to check for ground
        RaycastHit2D hit = Physics2D.Raycast(footIKTarget.position, -Vector2.up, stepRaycastDistance, groundLayer);
        if (hit.collider != null)
        {
            Vector2 footPosition = hit.point;
            // Optionally, you might want to add an offset here so the foot isn't exactly on the ground
            footIKTarget.position = Vector2.MoveTowards(footIKTarget.position, footPosition, stepSpeed * Time.fixedDeltaTime);
        }
    }
}
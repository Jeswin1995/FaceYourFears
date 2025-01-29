using UnityEngine;

public class WallMover : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the wall moves
    private Vector3 direction; // Direction the wall moves
    private float distanceToMove = 0.0f; // Distance to move for this step
    private float movedDistance = 0.0f; // Distance the wall has moved
    private bool isMoving = false; // Whether the wall is moving
    private Vector3 initialPosition; // Initial position of the wall for reference
    private float lapDistance = 3.0f; // Total distance this wall should move in a lap

    void Start()
    {
        initialPosition = transform.position; // Save initial position for reference
    }

    void Update()
    {
        if (isMoving && movedDistance < lapDistance)
        {
            // Calculate the step size for movement
            float moveStep = speed * Time.deltaTime;

            // Move the wall towards the target position
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + direction * lapDistance, moveStep);
            movedDistance += moveStep;

            if (movedDistance >= lapDistance)
            {
                isMoving = false;
                movedDistance = 0.0f; // Reset moved distance for next movement
            }
        }
    }

    // Set the movement direction
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    // Start moving the wall a certain distance
    public void StartMoving(float distance)
    {
        lapDistance = distance; // Set the movement distance for this lap
        movedDistance = 0.0f;
        isMoving = true;
        initialPosition = transform.position; // Update the initial position for the next lap
    }

    // Stop moving the wall
    public void StopMoving()
    {
        isMoving = false;
    }

    // Public getter for isMoving
    public bool IsMoving()
    {
        return isMoving;
    }

    // Increase speed for movement
    public void IncreaseSpeed(float multiplier)
    {
        speed *= multiplier; // Increase speed by the multiplier
    }

    public Vector3 GetDirection()
    {
        return direction;
    }
}

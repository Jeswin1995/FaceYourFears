using UnityEngine;

public class WallMover : MonoBehaviour
{
    public float speed = 1.0f; // Speed at which the wall moves
    private Vector3 direction; // Direction in which the wall moves
    private float distanceToMove = 0.0f; // The distance the wall should move
    private float movedDistance = 0.0f; // Distance the wall has moved so far
    private bool isMoving = false; // Whether the wall is moving
    private Vector3 initialPosition; // Initial position of the wall for Lerp

    void Start()
    {
        initialPosition = transform.position; // Save the initial position at the start
    }

    void Update()
    {
        if (isMoving && movedDistance < distanceToMove)
        {
            // Calculate the target position based on the direction and distance
            Vector3 targetPosition = initialPosition + direction * distanceToMove;

            // Smooth movement using Lerp
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            movedDistance += speed * Time.deltaTime;

            // If the target distance is reached, stop the movement
            if (movedDistance >= distanceToMove)
            {
                isMoving = false;
                movedDistance = 0.0f; // Reset moved distance for next move
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
        distanceToMove = distance; // Set the target distance
        movedDistance = 0.0f; // Reset moved distance to start fresh
        isMoving = true; // Start moving
    }

    // Stop the movement of the wall
    public void StopMoving()
    {
        isMoving = false;
    }

    // Public getter for isMoving
    public bool IsMoving()
    {
        return isMoving;
    }

    // Method to increase the speed of the movement
    public void IncreaseSpeed(float multiplier)
    {
        speed *= multiplier; // Increase the speed by the multiplier
    }
}

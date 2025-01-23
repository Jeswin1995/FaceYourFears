using UnityEngine;

public class WallMover : MonoBehaviour
{
    [SerializeField]
    private float moveDistance = 1f; // Total distance to move

    [SerializeField]
    private float moveSpeed = 1f; // Speed of movement

    [SerializeField]
    private float pauseDuration = 1f; // Pause duration at the end of each move

    private Vector3 initialPosition; // Starting position of the wall
    private bool movingForward = true; // Direction flag
    private bool isPaused = false; // Pause state
    private float pauseTimer = 0f; // Timer to track pause duration

    private void Start()
    {
        initialPosition = transform.localPosition; // Store the initial local position
    }

    private void Update()
    {
        if (isPaused)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration)
            {
                isPaused = false;
                pauseTimer = 0f;
            }
            return;
        }

        MoveWall();
    }

    private void MoveWall()
    {
        // Determine target position based on local forward direction
        Vector3 targetPosition = movingForward
            ? initialPosition + transform.forward * moveDistance
            : initialPosition;

        // Move towards the target position
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        // Check if we've reached the target position
        if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
        {
            movingForward = !movingForward; // Toggle direction
            isPaused = true; // Start pause
        }
    }
}

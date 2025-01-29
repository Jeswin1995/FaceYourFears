using UnityEngine;

public class WallMover : MonoBehaviour
{
    public float speed = 2.0f;
    private Vector3 direction;
    private float movedDistance = 0.0f;
    private float lapDistance = 3.0f; // Default value, will be updated by event
    private Vector3 initialPosition;
    private bool isMoving = false;

    public int wallIndex = -1; // Unique identifier for this wall
    private bool isReturning = false; // Track if wall is returning

    public bool isSubscribed = false; // Flag to check if the wall has subscribed to the event

    // Event invoked when wall completes its move
    public delegate void WallMoveCompletedEvent(int wallIndex);
    public event WallMoveCompletedEvent OnWallMoveCompleted;

    void Start()
    {
        initialPosition = transform.position;

        // Subscribe to the event from the WallSceneController
        WallSceneController controller = FindObjectOfType<WallSceneController>();
        if (controller != null)
        {
            controller.OnWallMoveRequested += StartMovingFromEvent;
            controller.OnWallMoveDistanceUpdated += UpdateLapDistance; // Subscribe to lapDistance update event
            isSubscribed = true;  // Mark as subscribed
            Debug.Log("WallMover: Successfully subscribed to the WallMoveRequested event.");
        }
    }

    void Update()
    {
        if (isMoving && movedDistance < lapDistance)
        {
            float moveStep = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + direction * lapDistance, moveStep);
            movedDistance += moveStep;

            if (movedDistance >= lapDistance)
            {
                isMoving = false;
                movedDistance = 0.0f;
                OnWallMoveCompleted?.Invoke(wallIndex); // Notify the WallSceneController that the wall is done moving
            }
        }
    }

    private void StartMovingFromEvent(int wallIndex)
    {
        if (this.wallIndex == wallIndex)
        {
            StartMoving();
        }
    }

    public void StartMoving()
    {
        movedDistance = 0.0f;
        initialPosition = transform.position;
        isMoving = true;
    }

    public void UpdateLapDistance(float newLapDistance)
    {
        lapDistance = newLapDistance; // Update lapDistance based on the event from WallSceneController
        Debug.Log($"WallMover: lapDistance updated to {lapDistance} for wall index {wallIndex}");
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    public void IncreaseSpeed(float multiplier)
    {
        speed *= multiplier;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public bool IsReturning()
    {
        return isReturning;
    }

    public void SetReturning(bool returning)
    {
        isReturning = returning;
    }
}

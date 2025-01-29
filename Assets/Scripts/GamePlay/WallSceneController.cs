using System.Collections.Generic;
using UnityEngine;

public class WallSceneController : MonoBehaviour
{
    public WallSpawner wallManager;
    public float movementDistance = 10.0f;
    public float wallMoveDistance = 1.0f;

    [SerializeField]
    private float speedMultiplier = 2f;

    private List<GameObject> walls = new List<GameObject>();
    private bool isActive = false;
    private float movedDistance = 0.0f;

    private int currentWallIndex = 0;
    private int lapCount = 0;

    // Event to request wall movement
    public delegate void WallMoveEvent(int wallIndex);
    public event WallMoveEvent OnWallMoveRequested;

    // Event to update wallMoveDistance for all walls
    public delegate void WallMoveDistanceUpdatedEvent(float wallMoveDistance);
    public event WallMoveDistanceUpdatedEvent OnWallMoveDistanceUpdated;


    private void Start()
    {
        if (wallManager != null)
        {
            wallManager.WallsSpawnedEvent.AddListener(OnWallsSpawned);
        }
    }
    private void Update()
    {
        if (isActive)
        {
            bool allWallsSubscribed = true;

            // Check if all walls have subscribed
            foreach (var wall in walls)
            {
                WallMover wallMover = wall.GetComponent<WallMover>();
                if (wallMover != null && !wallMover.isSubscribed)
                {
                    allWallsSubscribed = false;
                    Debug.LogWarning($"WallMover for wall index {wallMover.wallIndex} is not subscribed yet.");
                }
            }

            // Trigger the first wall move once all walls are subscribed
            if (allWallsSubscribed)
            {
                TriggerWallMove(currentWallIndex);
                // Update wallMoveDistance for all walls
                UpdateWallMoveDistanceForWalls(wallMoveDistance); // You can set this value dynamically based on your needs
                isActive = false; // Prevent further checks after the move is triggered
            }
        }
    }

    private void OnWallsSpawned()
    {
        walls = wallManager.GetWalls();
        isActive = true;

        // Subscribe WallMovers to the event
        foreach (var wall in walls)
        {
            WallMover wallMover = wall.GetComponent<WallMover>();
            if (wallMover != null)
            {
                wallMover.OnWallMoveCompleted += HandleWallMoveCompleted; // Wall has completed its move
            }
        }

        // Trigger the first wall move
        TriggerWallMove(currentWallIndex);
    }

    private void TriggerWallMove(int wallIndex)
    {
        Debug.Log($"WallSceneController: Triggering wall move event for wallIndex: {wallIndex}");
        OnWallMoveRequested?.Invoke(wallIndex);

    }

    private void UpdateWallMoveDistanceForWalls(float newWallMoveDistance)
    {
        OnWallMoveDistanceUpdated?.Invoke(newWallMoveDistance); // Trigger the event to update wallMoveDistance
        Debug.Log($"WallSceneController: wallMoveDistance updated to {newWallMoveDistance} for all walls.");
    }

    private void HandleWallMoveCompleted(int wallIndex)
    {
        // When a wall completes its move, update the current index and move the next wall
        if (wallIndex == currentWallIndex)
        {
            currentWallIndex++;

            if (currentWallIndex >= walls.Count)
            {
                currentWallIndex = 0;
                movedDistance += wallMoveDistance;
                Debug.Log($"WallSceneController: Moved distance updated to {movedDistance}");

                if (movedDistance >= movementDistance)
                {
                    UpdateWallDirections();
                    movedDistance = 0.0f;

                    // Adjust speed after each lap
                    lapCount++;
                    UpdateWallSpeeds();
                }
            }

            // Trigger the next wall move
            TriggerWallMove(currentWallIndex);

        }
    }

    private void UpdateWallDirections()
    {
        foreach (var wall in walls)
        {
            WallMover mover = wall.GetComponent<WallMover>();
            if (mover != null)
            {
                Vector3 currentDirection = mover.GetDirection();
                Vector3 newDirection = -currentDirection;
                mover.SetDirection(newDirection);
                Debug.Log(mover.IsReturning() ? "Wall is now moving inward." : "Wall is now moving outward.");
            }
        }
    }

    private void UpdateWallSpeeds()
    {
        foreach (var wall in walls)
        {
            WallMover mover = wall.GetComponent<WallMover>();
            if (mover != null)
            {
                mover.IncreaseSpeed(speedMultiplier);
            }
        }
    }
}

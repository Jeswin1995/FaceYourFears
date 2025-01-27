using System.Collections.Generic;
using UnityEngine;

public class WallSceneController : MonoBehaviour
{
    public WallSpawner wallManager;
    public float movementDistance = 10.0f;

    [SerializeField]
    private float spacing = 2.0f; // Spacing between walls

    [SerializeField]
    private float initialTimeForMovement = 1.0f; // Initial time each wall spends moving
    [SerializeField]
    private float speedMultiplier = 2f; // Multiplier to decrease time after each lap

    private List<GameObject> walls = new List<GameObject>(); // Cached list of walls

    private bool isReturning = false;
    private bool isActive = false; // Controls the update loop
    private float movedDistance = 0.0f;

    private int currentWallIndex = 0; // Tracks which wall is currently moving
    private float movementTimer = 0.0f; // Tracks the time for the current wall's movement
    private bool isWallMoving = false;

    private int lapCount = 0; // Number of completed laps

    private void Start()
    {
        if (wallManager != null)
        {
            wallManager.WallsSpawnedEvent.AddListener(OnWallsSpawned);
        }
    }

    private void OnWallsSpawned()
    {
        walls = wallManager.GetWalls();
        isActive = true; // Enable the update loop
    }
    private void Update()
    {
        if (!isActive || walls == null || walls.Count == 0)
            return;

        if (!isWallMoving)
        {
            // Start moving the next wall
            WallMover mover = walls[currentWallIndex].GetComponent<WallMover>();
            if (mover != null)
            {
                mover.StartMoving();
                isWallMoving = true;
            }
        }

        // Update movement timer
        if (isWallMoving)
        {
            movementTimer += Time.deltaTime;
            if (movementTimer >= initialTimeForMovement)
            {
                // Stop moving the current wall
                WallMover mover = walls[currentWallIndex].GetComponent<WallMover>();
                if (mover != null)
                {
                    mover.StopMoving();
                }

                // Move to the next wall
                currentWallIndex++;
                if (currentWallIndex >= walls.Count)
                {
                    currentWallIndex = 0;
                    movedDistance += spacing;

                    if (movedDistance >= movementDistance)
                    {
                        isReturning = !isReturning;
                        UpdateWallDirections();
                        movedDistance = 0.0f;

                        // Increment lap count and adjust speed after each lap
                        lapCount++;
                        // Update speed for all walls
                        UpdateWallSpeeds();
                        AdjustMovementSpeed();
                    }
                }

                // Reset timer and prepare for the next wall
                movementTimer = 0.0f;
                isWallMoving = false;
            }
        }
    }

    private void UpdateWallDirections()
    {
        Vector3 newDirection = isReturning ? Vector3.back : Vector3.forward;

        var walls = wallManager.GetWalls();
        foreach (var wall in walls)
        {
            WallMover mover = wall.GetComponent<WallMover>();
            if (mover != null)
            {
                mover.SetDirection(newDirection);
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
                mover.AdjustSpeed(speedMultiplier); // Adjust speed using the multiplier
            }
        }
    }

    private void AdjustMovementSpeed()
    {
        // Decrease the time per wall movement (i.e., increase speed) after each lap
        initialTimeForMovement /= speedMultiplier ;
    }
}

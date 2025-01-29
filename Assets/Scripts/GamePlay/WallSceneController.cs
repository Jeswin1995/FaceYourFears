using System.Collections.Generic;
using UnityEngine;

public class WallSceneController : MonoBehaviour
{
    public WallSpawner wallManager;
    public float movementDistance = 10.0f;
    public float wallMoveDistance = 1.0f; // Distance for each wall to move before stopping

    [SerializeField]
    private float spacing = 2.0f; // Spacing between walls

    [SerializeField]
    private float speedMultiplier = 2f; // Multiplier to decrease time after each lap

    private List<GameObject> walls = new List<GameObject>(); // Cached list of walls

    private bool isReturning = false;
    private bool isActive = false; // Controls the update loop
    private float movedDistance = 0.0f;

    private int currentWallIndex = 0; // Tracks which wall is currently moving
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
                mover.StartMoving(wallMoveDistance);
                isWallMoving = true;
            }
        }

        // Update movement timer
        if (isWallMoving)
        {
            WallMover mover = walls[currentWallIndex].GetComponent<WallMover>();
            if (mover != null && !mover.IsMoving())
            {
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
                    }
                }

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
                mover.IncreaseSpeed(speedMultiplier); // Adjust speed using the multiplier
            }
        }
    }   
}

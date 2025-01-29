using System.Collections.Generic;
using UnityEngine;

public class WallSceneController : MonoBehaviour
{
    public WallSpawner wallManager;
    public float DistanceOneWall = 10.0f;
    public float LapDistance = 1.0f; // Distance for each wall to move before stopping



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
        foreach (var wall in walls)
        {
            WallMover wallMover = wall.GetComponent<WallMover>();
            if (wallMover != null)
            {
                // Instead of overriding, use the direction set by WallSpawner
                Vector3 correctDirection = wallMover.GetDirection();
                wallMover.SetDirection(correctDirection);
            }
        }
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
                mover.StartMoving(LapDistance);
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
                    movedDistance += LapDistance;
                    Debug.Log(movedDistance);

                    if (movedDistance >= DistanceOneWall)
                    {
                        isReturning = !isReturning;
                        UpdateWallDirections();
                        movedDistance = 0.0f;

                        // Adjust speed after each lap
                        lapCount++;
                        UpdateWallSpeeds();
                    }
                }

                isWallMoving = false;
            }
        }
    }


    private void UpdateWallDirections()
    {
        foreach (var wall in walls)
        {
            WallMover mover = wall.GetComponent<WallMover>();
            if (mover != null)
            {
                // Reverse the direction instead of applying a global forward/backward
                Vector3 currentDirection = mover.GetDirection();
                Vector3 newDirection = isReturning ? -currentDirection : currentDirection;
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

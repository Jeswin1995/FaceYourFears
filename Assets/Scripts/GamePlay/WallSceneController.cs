using UnityEngine;

public class WallSceneController : MonoBehaviour
{
    public WallSpawner wallManager;
    public float movementDistance = 10.0f;

    [SerializeField]
    private float spacing = 2.0f; // Spacing between walls

    [SerializeField]
    private float timeForMovement = 1.0f; // Time each wall spends moving

    private bool isReturning = false;
    private float movedDistance = 0.0f;

    private int currentWallIndex = 0; // Tracks which wall is currently moving
    private float movementTimer = 0.0f; // Tracks the time for the current wall's movement
    private bool isWallMoving = false;

    private void Update()
    {
        var walls = wallManager.GetWalls();
        if (walls == null || walls.Count == 0)
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
            if (movementTimer >= timeForMovement)
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
}

using Meta.XR.MRUtilityKit;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class WallSpawner : MonoBehaviour
{
    [SerializeField]
    private MRUK mruk; // Reference to the MRUK instance

    [SerializeField]
    private GameObject wallPrefab; // Reference to the wall prefab to spawn

    [SerializeField]
    private Transform parentObject; // Parent GameObject for instantiated walls

    [SerializeField]
    private float minimumWallWidth = 0.5f; // Minimum width to consider for spawning

    [SerializeField]
    private float wallSpawnOffset = 0.01f; // Small offset from the actual wall surface

    [SerializeField]
    private Vector3 initialPosition = Vector3.zero; // Default position for walls

    [SerializeField]
    private int numberOfWalls = 5; // Number of walls to spawn if no MRUK integration

    public UnityEvent WallsSpawnedEvent = new UnityEvent(); // Event triggered when walls are spawned

    private bool sceneLoaded = false;

    private List<GameObject> walls = new List<GameObject>(); // List of spawned walls

    private void Awake()
    {
        if (mruk == null)
        {
            Debug.LogError("MRUK reference is not set. Please assign it in the inspector.");
        }
        if (wallPrefab == null)
        {
            Debug.LogError("Wall prefab reference is not set. Please assign it in the inspector.");
        }
        if (parentObject == null)
        {
            Debug.LogError("Parent object reference is not set. Please assign it in the inspector.");
        }
    }

    private void OnEnable()
    {
        if (mruk != null)
        {
            mruk.SceneLoadedEvent.AddListener(OnSceneLoaded);
        }
    }

    private void OnDisable()
    {
        if (mruk != null)
        {
            mruk.SceneLoadedEvent.RemoveListener(OnSceneLoaded);
        }
    }

    private void OnSceneLoaded()
    {
        Debug.Log("Scene loaded successfully.");
        sceneLoaded = true;

        if (mruk != null)
        {
            SpawnWallsUsingMRUK();
            WallsSpawnedEvent.Invoke(); // Trigger the event after spawning walls
        }
    }

    private void SpawnWallsUsingMRUK()
    {
        if (!sceneLoaded || wallPrefab == null || mruk == null || parentObject == null)
            return;

        MRUKRoom currentRoom = mruk.GetCurrentRoom();
        if (currentRoom == null)
        {
            Debug.LogWarning("No current room available.");
            return;
        }

        foreach (var anchorInfo in currentRoom.Anchors)
        {
            if (anchorInfo.HasAnyLabel(MRUKAnchor.SceneLabels.WALL_FACE))
            {
                if (!anchorInfo.PlaneRect.HasValue)
                {
                    Debug.LogWarning($"2D bounds not available for {anchorInfo.name}");
                    continue;
                }

                Vector2 wallScale = anchorInfo.PlaneRect.Value.size;
                if (wallScale.x < minimumWallWidth && wallScale.y < minimumWallWidth)
                {
                    Debug.Log($"Skipping wall: {anchorInfo.name} due to insufficient width.");
                    continue;
                }

                Vector3 adjustedCenter = anchorInfo.transform.position + anchorInfo.transform.forward * wallSpawnOffset;

                GameObject wall = Instantiate(
                    wallPrefab,
                    adjustedCenter,
                    Quaternion.LookRotation(anchorInfo.transform.forward), // Face inwards
                    parentObject
                );

                wall.transform.localScale = new Vector3(wallScale.x, wallScale.y, wallPrefab.transform.localScale.z);
                walls.Add(wall);

                // Assign movement behavior
                WallMover wallMover = wall.GetComponent<WallMover>();
                if (wallMover != null)
                {
                    wallMover.SetDirection(Vector3.forward); // Initial movement direction
                }

                Debug.Log($"Spawned wall prefab at {adjustedCenter} with 2D size {wallScale}");
            }
        }

        Debug.Log($"{walls.Count} walls instantiated using MRUK.");
    }

    public List<GameObject> GetWalls()
    {
        return walls;
    }
}

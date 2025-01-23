using Meta.XR.MRUtilityKit;
using UnityEngine;
using System.Collections.Generic;

namespace GamePlay
{
    public class WallSpawner : MonoBehaviour
    {
        [SerializeField]
        private MRUK mruk; // Reference to the MRUK instance

        [SerializeField]
        private GameObject wallPrefab; // Reference to the wall prefab to spawn

        [SerializeField]
        private float minimumWallWidth = 0.5f; // Minimum width to consider for spawning

        [SerializeField]
        private float wallSpawnOffset = 0.01f; // Small offset from the actual wall surface

        private bool sceneLoaded = false;

        private void Awake()
        {
            // Ensure dependencies are set
            if (mruk == null)
            {
                Debug.LogError("MRUK reference is not set. Please assign it in the inspector.");
            }
            if (wallPrefab == null)
            {
                Debug.LogError("Wall prefab reference is not set. Please assign it in the inspector.");
            }
        }

        private void OnEnable()
        {
            // Subscribe to the scene loaded event
            if (mruk != null)
            {
                mruk.SceneLoadedEvent.AddListener(OnSceneLoaded);
            }
        }

        private void OnDisable()
        {
            // Unsubscribe from the scene loaded event
            if (mruk != null)
            {
                mruk.SceneLoadedEvent.RemoveListener(OnSceneLoaded);
            }
        }

        private void OnSceneLoaded()
        {
            Debug.Log("Scene loaded successfully.");
            sceneLoaded = true;
            SpawnWallsUsingMRUK();
        }

        private void SpawnWallsUsingMRUK()
        {
            if (!sceneLoaded || wallPrefab == null || mruk == null)
                return;

            // Get the current room
            MRUKRoom currentRoom = mruk.GetCurrentRoom();
            if (currentRoom == null)
            {
                Debug.LogWarning("No current room available.");
                return;
            }

            List<MRUKAnchor> walls = new List<MRUKAnchor>();

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

                    GameObject spawnedWall = Instantiate(
                        wallPrefab,
                        adjustedCenter,
                        Quaternion.LookRotation(-anchorInfo.transform.forward) // Face inwards
                    );

                    spawnedWall.transform.localScale = new Vector3(wallScale.x, wallScale.y, wallPrefab.transform.localScale.z);
                    spawnedWall.transform.SetParent(anchorInfo.transform);

                    Debug.Log($"Spawned wall prefab at {adjustedCenter} with 2D size {wallScale}");
                    walls.Add(anchorInfo);
                }
            }
        }
    }
}

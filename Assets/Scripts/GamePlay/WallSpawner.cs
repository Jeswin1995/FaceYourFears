using Meta.XR.MRUtilityKit;
using UnityEngine;

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

            // Create a filter for wall surfaces
            var labelFilter = new LabelFilter(MRUKAnchor.SceneLabels.WALL_FACE);

            // Get all wall surfaces in the room
            var wallSurfaces = currentRoom.WallAnchors;

            foreach (var surface in wallSurfaces)
            {
                // Spawn the wall prefab
                GameObject spawnedWall = Instantiate(
                    wallPrefab,
                    transform // Parent to this object
                );

            }
        }
    }
}
using Meta.XR.MRUtilityKit;
using UnityEngine;

namespace GamePlay
{
    public class SpiderSpawner : MonoBehaviour
    {
        [SerializeField]
        private MRUK mruk; // Reference to the MRUK instance

        [SerializeField]
        private FindSpawnPositions findSpawnPositions; // Reference to the FindSpawnPositions script

        
        [SerializeField] private float spawnInterval = 10f; // Spawn interval in seconds
        
        
         private bool sceneLoaded = false;
         private bool isSpawning = false;
         private float spawnTimer = 0f;

        private void Awake()
        {
            // Ensure dependencies are set
            if (mruk == null)
            {
                Debug.LogError("MRUK reference is not set. Please assign it in the inspector.");
            }
            if (findSpawnPositions == null)
            {
                Debug.LogError("FindSpawnPositions reference is not set. Please assign it in the inspector.");
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
            isSpawning = true; // Start spawning when the scene is loaded
        }

        private void Update()
        {
            if (sceneLoaded && isSpawning)
            {
                spawnTimer += Time.deltaTime;

                if (spawnTimer >= spawnInterval)
                {
                    findSpawnPositions.StartSpawn();
                    Debug.Log("Spider spawned.");
                    spawnTimer = 0f; // Reset the timer
                }
            }
        }

        /// <summary>
        /// Pause spider spawning.
        /// </summary>
        public void PauseSpawning()
        {
            isSpawning = false;
        }

        /// <summary>
        /// Resume spider spawning.
        /// </summary>
        public void ResumeSpawning()
        {
            isSpawning = true;
        }

        public void DestroyAllSpidersAndStopSpawning()
        {
            isSpawning = false;
            GameObject[] spiders = GameObject.FindGameObjectsWithTag("Spider");
            foreach (GameObject spider in spiders)
            {
                Destroy(spider);
            }
            Debug.Log("All spiders destroyed and spawning stopped.");
        }
    }
}

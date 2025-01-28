using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
        private FindSpawnPositions spawnPositionManager;
    
        [SerializeField]
        private float gridSpacing = 0.5f;
    
        [SerializeField]
        private Vector2 minGridSize = new Vector2(1f, 1f);
    
        [SerializeField]
        private float roomMargin = 0.5f;
    
        [SerializeField]
        private float heightFromGround = 1.0f;
    
        [SerializeField]
        private int maxObjectsPerBatch = 20;
    
        [Header("Dynamic Sizing Settings")]
        [SerializeField]
        private float totalDuration = 180f;
        [SerializeField]
        private float startingScale = 1f;
        [SerializeField]
        private float endingScale = 0.2f;
        [SerializeField]
        private float spawnInterval = 2f;
        
        private float elapsedTime = 0f;
        private float nextSpawnTime = 0f;
        private bool isSpawning = false;
        private int failedSpawnAttempts = 0;
        private const int MAX_FAILED_ATTEMPTS = 3; // Stop after 3 consecutive failed attempts
    
        private void Start()
        {
            if (MRUK.Instance)
            {
                MRUK.Instance.RegisterSceneLoadedCallback(OnSceneLoaded);
            }
        }
    
        private void OnSceneLoaded()
        {
            var currentRoom = MRUK.Instance.GetCurrentRoom();
            if (currentRoom != null)
            {
                isSpawning = true;
                failedSpawnAttempts = 0;
                StartSpawningCycle(currentRoom);
            }
        }
    
        private void Update()
        {
            if (!isSpawning) return;
    
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime >= totalDuration)
            {
                isSpawning = false;
                Debug.Log("Spawning completed: Time limit reached");
                return;
            }
    
            UpdateObjectScale();
    
            if (Time.time >= nextSpawnTime)
            {
                nextSpawnTime = Time.time + spawnInterval;
                var currentRoom = MRUK.Instance.GetCurrentRoom();
                if (currentRoom != null)
                {
                    StartSpawningCycle(currentRoom);
                }
            }
        }
    
        private void UpdateObjectScale()
        {
            float progress = elapsedTime / totalDuration;
            float currentScale = Mathf.Lerp(startingScale, endingScale, progress);
    
            if (spawnPositionManager.SpawnObject != null)
            {
                Vector3 newScale = Vector3.one * currentScale;
                if (Vector3.Distance(spawnPositionManager.SpawnObject.transform.localScale, newScale) > 0.001f)
                {
                    spawnPositionManager.SpawnObject.transform.localScale = newScale;
                    gridSpacing = 0.5f * currentScale;
                }
            }
        }
    
        private void StartSpawningCycle(MRUKRoom room)
        {
            if (failedSpawnAttempts >= MAX_FAILED_ATTEMPTS)
            {
                isSpawning = false;
                Debug.Log("Spawning stopped: Too many failed spawn attempts");
                return;
            }
    
            CalculateVerticalGridDimensions(room, out int columnsCount, out int rowsCount);
            
            int totalObjects = columnsCount * rowsCount;
            int remainingObjects = Mathf.Min(maxObjectsPerBatch, totalObjects);
            
            int originalMaxIterations = spawnPositionManager.MaxIterations;
            
            StartCoroutine(SpawnBatches(room, remainingObjects, originalMaxIterations));
        }
        
        
    
        private System.Collections.IEnumerator SpawnBatches(MRUKRoom room, int remainingObjects, int originalMaxIterations)
        {
            int previousObjectCount = GetSpawnedObjectCount();
    
            while (remainingObjects > 0 && isSpawning)
            {
                int batchSize = Mathf.Min(remainingObjects, maxObjectsPerBatch);
                
                spawnPositionManager.SpawnAmount = batchSize;
                spawnPositionManager.MaxIterations = 100;
                spawnPositionManager.StartSpawn(room);
                
                yield return new WaitForSeconds(0.1f); 
    
                int currentObjectCount = GetSpawnedObjectCount();
                if (currentObjectCount == previousObjectCount)
                {
                    failedSpawnAttempts++;
                    Debug.LogWarning($"Failed spawn attempt {failedSpawnAttempts}/{MAX_FAILED_ATTEMPTS}");
                    
                    if (failedSpawnAttempts >= MAX_FAILED_ATTEMPTS)
                    {
                        isSpawning = false;
                        Debug.Log("Spawning stopped: No more valid positions available");
                        yield break;
                    }
                }
                else
                {
                    failedSpawnAttempts = 0;
                }
    
                previousObjectCount = currentObjectCount;
                remainingObjects -= batchSize;
                yield return null;
            }
            
            spawnPositionManager.MaxIterations = originalMaxIterations;
        }
    
        private int GetSpawnedObjectCount()
        {
            return transform.childCount;
        }
    
        private void CalculateVerticalGridDimensions(MRUKRoom room, out int columnsCount, out int rowsCount)
        {
            Bounds roomBounds = room.GetRoomBounds();
            float availableWidth = roomBounds.size.x - (roomMargin * 2);
            float availableHeight = roomBounds.size.y - (roomMargin * 2) - heightFromGround;
            
            if (availableWidth < minGridSize.x || availableHeight < minGridSize.y)
            {
                Debug.LogWarning("Wall space is too small for grid spawning!");
                columnsCount = 0;
                rowsCount = 0;
                return;
            }
    
            columnsCount = Mathf.FloorToInt(availableWidth / (gridSpacing * 1.5f));
            rowsCount = Mathf.FloorToInt(availableHeight / (gridSpacing * 1.5f));
    
            float wallArea = availableWidth * availableHeight;
            int maxTotalObjects = Mathf.FloorToInt(wallArea / (gridSpacing * gridSpacing * 4));
    
            if (columnsCount * rowsCount > maxTotalObjects)
            {
                float ratio = availableWidth / availableHeight;
                columnsCount = Mathf.FloorToInt(Mathf.Sqrt(maxTotalObjects * ratio));
                rowsCount = Mathf.FloorToInt(maxTotalObjects / (float)columnsCount);
            }
        }
}

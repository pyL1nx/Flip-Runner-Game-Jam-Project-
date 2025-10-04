using UnityEngine;

public class ObstacleSpawnerRepeat : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform[] spawnPoints; // expects exactly 2
    [SerializeField] private float interval = 1.5f;
    [SerializeField, Range(0f, 1f)] private float spawnProbability = 1f;
    [SerializeField] private int maxPerPlatform = 999;

    [Header("Batch alternation")]
    [SerializeField] private Vector2Int batchSizeRange = new Vector2Int(1, 3); // e.g., 1..3 spawns per lane before switching

    private float timer;
    private int spawnedCount;

    // Alternation state
    private int currentLane = 0;      // 0 or 1
    private int batchRemaining = 0;   // spawns left in this lane before switching

    private void OnEnable()
    {
        InitBatchIfNeeded();
    }

    private void Update()
    {
        if (spawnedCount >= maxPerPlatform) return;
        timer += Time.deltaTime;

        while (timer >= interval)
        {
            timer -= interval;

            if (Random.value > spawnProbability) continue;

            SpawnOneAtLane(currentLane);
            spawnedCount++;
            batchRemaining = Mathf.Max(0, batchRemaining - 1);

            if (batchRemaining == 0)
            {
                // Switch lane and roll a new batch size
                currentLane = 1 - currentLane; // toggle 0 <-> 1
                batchRemaining = Random.Range(batchSizeRange.x, batchSizeRange.y + 1);
            }

            if (spawnedCount >= maxPerPlatform) break;
        }
    }

    private void InitBatchIfNeeded()
    {
        if (spawnPoints == null || spawnPoints.Length < 2) return;
        if (batchRemaining <= 0)
        {
            currentLane = Random.value < 0.5f ? 0 : 1; // start on either lane
            batchRemaining = Random.Range(batchSizeRange.x, batchSizeRange.y + 1);
        }
    }

    private void SpawnOneAtLane(int laneIndex)
    {
        if (obstaclePrefabs.Length == 0 || spawnPoints.Length < 2) return;

        int clampedLane = Mathf.Clamp(laneIndex, 0, 1);
        var prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        var point  = spawnPoints[clampedLane];
        Instantiate(prefab, point.position, point.rotation, transform);
    }
}

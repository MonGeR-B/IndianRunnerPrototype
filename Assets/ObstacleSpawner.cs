using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    public float spawnZ = 50f;
    public float spawnInterval = 2f;
    public float laneDistance = 3f;

    public float minSpawnInterval = 0.8f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        float currentInterval = Mathf.Lerp(
            spawnInterval,
            minSpawnInterval,
            GameManager.instance.gameSpeed / GameManager.instance.maxSpeed
        );

        if (timer >= currentInterval)
        {
            SpawnPattern();
            timer = 0f;
        }
    }

    void SpawnPattern()
    {
        int pattern = Random.Range(0, 3);

        if (pattern == 0)
        {
            SpawnInLane(RandomLane());
        }
        else if (pattern == 1)
        {
            int lane = RandomLane();
            SpawnInLane(lane);
            SpawnInLane((lane + 1) % 3);
        }
        else
        {
            SpawnInLane(1);
        }
    }

    void SpawnInLane(int lane)
    {
        float x = (lane - 1) * laneDistance;
        Vector3 spawnPos = new Vector3(x, 0.5f, spawnZ);

        GameObject obj = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        int type = Random.Range(0, 3);

        Obstacle obstacle = obj.AddComponent<Obstacle>();

        if (type == 1)
        {
            // TALL → requires jump
            obj.transform.localScale = new Vector3(1, 2.5f, 1);
            obj.transform.position += Vector3.up * 1f;
            obstacle.type = ObstacleType.Tall;
        }
        else if (type == 2)
        {
            // LOW → requires slide
            obj.transform.localScale = new Vector3(1, 0.5f, 1);
            obj.transform.position += Vector3.up * 1f;
            obstacle.type = ObstacleType.Low;
        }
        else
        {
            obstacle.type = ObstacleType.Normal;
        }
    }

    int RandomLane()
    {
        return Random.Range(0, 3);
    }
}
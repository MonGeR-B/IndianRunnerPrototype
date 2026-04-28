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
            // single obstacle
            SpawnInLane(RandomLane());
        }
        else if (pattern == 1)
        {
            // force decision (left or right)
            SpawnInLane(0);
            SpawnInLane(2);
        }
        else
        {
            // block middle (forces lane change)
            SpawnInLane(1);
        }
    }

    void SpawnInLane(int lane)
    {
        float x = (lane - 1) * laneDistance;

        Vector3 spawnPos = new Vector3(x, 0.5f, spawnZ);

        GameObject obj = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        int type = Random.Range(0, 3);

        // 🔥 Shape variation
        if (type == 0)
            obj.transform.localScale = new Vector3(1, 1, 1); // normal
        else if (type == 1)
            obj.transform.localScale = new Vector3(1, 2.5f, 1); // tall (jump)
        else
            obj.transform.localScale = new Vector3(1, 0.5f, 1); // low (slide)

        // 🔥 Color variation
        Renderer r = obj.GetComponent<Renderer>();

        int colorType = Random.Range(0, 3);

        if (colorType == 0) r.material.color = Color.red;
        if (colorType == 1) r.material.color = Color.yellow;
        if (colorType == 2) r.material.color = Color.blue;
    }

    int RandomLane()
    {
        return Random.Range(0, 3);
    }
}
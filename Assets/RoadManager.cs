using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject[] tiles;
    public float tileLength = 30f;

    void Update()
    {
        // Move tiles
        foreach (GameObject tile in tiles)
        {
            tile.transform.Translate(
                Vector3.back * GameManager.instance.gameSpeed * Time.deltaTime
            );
        }

        // Recycle tiles
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.z < -tileLength)
            {
                tile.transform.position = new Vector3(
                    0,
                    0,
                    GetMaxZ() + tileLength
                );
            }
        }
    }

    float GetMaxZ()
    {
        float maxZ = float.MinValue;

        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.z > maxZ)
                maxZ = tile.transform.position.z;
        }

        return maxZ;
    }
}
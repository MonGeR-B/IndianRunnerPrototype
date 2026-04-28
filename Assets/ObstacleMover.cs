using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 10f;
    public float destroyZ = -10f;

    void Update()
    {
        // Move obstacle
        transform.Translate(Vector3.back * GameManager.instance.gameSpeed * Time.deltaTime);

        // Destroy when behind player
        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
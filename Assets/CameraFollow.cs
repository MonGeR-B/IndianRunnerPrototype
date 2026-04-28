using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0, 6, -10);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // 🔥 Step 3: Smooth lane follow (X-axis lag)
        float smoothX = Mathf.Lerp(
            transform.position.x,
            target.position.x,
            3f * Time.deltaTime
        );

        // 🔥 Step 4: Dynamic zoom based on speed
        float dynamicZ = offset.z - GameManager.instance.gameSpeed * 0.2f;

        Vector3 desiredPosition = new Vector3(
            smoothX,
            target.position.y + offset.y,
            target.position.z + dynamicZ
        );

        // 🔥 Step 1: Smooth follow
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(target);
    }
}
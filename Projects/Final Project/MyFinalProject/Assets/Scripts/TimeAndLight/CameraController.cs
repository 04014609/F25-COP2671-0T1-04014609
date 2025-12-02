using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;        // Player position reference
    public Vector2 minPosition;      // Minimum (x,y) clamp corner
    public Vector2 maxPosition;      // Maximum (x,y) clamp corner
    public float pixelStep = 0.0625f; // 1/16 units = 1 pixel if tile size = 16px

    void Start()
    {
        // Automatically find the player in the scene
        player = Object.FindFirstObjectByType<PlayerController>().transform;
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        // Follow player position
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

        // Snap movement to pixel grid for smoother camera motion
        targetPosition.x = Mathf.Round(targetPosition.x / pixelStep) * pixelStep;
        targetPosition.y = Mathf.Round(targetPosition.y / pixelStep) * pixelStep;

        // Clamp camera inside boundaries
        float clampedX = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}

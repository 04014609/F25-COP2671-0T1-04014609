using UnityEngine;

public class CropSelector : MonoBehaviour
{
    public float checkDistance = 0.8f;
    public LayerMask cropLayer; // we'll set this later

    private PlayerController player;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Direction player is facing
        Vector2 dir = player.LastMoveDirection;

        if (dir == Vector2.zero) return;

        // Check tile in front of player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, checkDistance, cropLayer);

        if (hit.collider != null)
        {
            CropBlock block = hit.collider.GetComponent<CropBlock>();
            if (block != null)
            {
                FarmingController.Instance.SetSelectedBlock(block);
            }
        }
        else
        {
            // nothing selected
            FarmingController.Instance.SetSelectedBlock(null);
        }
    }
}

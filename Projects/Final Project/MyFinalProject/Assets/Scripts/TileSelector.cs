using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public PlayerController playerController;
    public Tilemap cropTilemap;
    public SpriteRenderer cursorSprite;

    [Header("Cursor Offset Tuning")]
    // Adjust this until cursor matches the tile in front of player
    public float cursorYOffset = -0.5f;

    private void Update()
    {
        if (!player || !cursorSprite || !cropTilemap || !playerController)
            return;

        // 1. Target point = player pivot + offset downward
        Vector3 targetFeet = player.position + new Vector3(0, cursorYOffset, 0);

        // 2. Facing direction
        Vector2 dir = playerController.LastMoveDirection;
        if (dir.sqrMagnitude < 0.1f)
            dir = Vector2.down;

        // 3. Tile in front
        Vector3 worldTarget = targetFeet + (Vector3)dir;

        // 4. Convert world → tile
        Vector3Int cellPos = cropTilemap.WorldToCell(worldTarget);

        // 5. Move cursor to tile center
        transform.position = cropTilemap.GetCellCenterWorld(cellPos);

        // 6. Check if farmland tile exists
        if (cropTilemap.HasTile(cellPos))
        {
            cursorSprite.enabled = true;

            CropBlock block = FindBlockAt(cellPos);
            if (block != null)
                FarmingController.Instance.SetSelectedBlock(block);
        }
        else
        {
            cursorSprite.enabled = false;
            FarmingController.Instance.SetSelectedBlock(null);
        }
    }

    private CropBlock FindBlockAt(Vector3Int cellPos)
    {
        CropBlock[] blocks = Object.FindObjectsByType<CropBlock>(FindObjectsSortMode.None);

        foreach (CropBlock block in blocks)
        {
            if (block.Location.x == cellPos.x && block.Location.y == cellPos.y)
                return block;
        }

        return null;
    }
}

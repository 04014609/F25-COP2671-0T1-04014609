using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    [Header("References")]
    public Transform player;                     // Player transform
    public PlayerController playerController;    // PlayerController script
    public Tilemap cropTilemap;                  // Crops Tilemap
    public SpriteRenderer cursorSprite;          // Cursor sprite renderer

    [Header("Cursor Offset Tuning")]
    // This lowers the target point so it aligns with the player's feet visually.
    public float cursorYOffset = -0.55f;

    // Down direction fix (extra upward correction when facing down)
    public float downFixOffset = 0.45f;

    private void Update()
    {
        if (!player || !playerController || !cropTilemap || !cursorSprite)
            return;

        // 1. Start from the player's pivot + visual feet offset
        Vector3 targetFeet = player.position + new Vector3(0, cursorYOffset, 0);

        // 2. Get the direction the player is facing
        Vector2 dir = playerController.LastMoveDirection;
        if (dir.sqrMagnitude < 0.1f)
            dir = Vector2.down;  // default when idle

        // 3. Tile in front of the player
        Vector3 worldTarget = targetFeet + (Vector3)dir;

        // SPECIAL FIX: cursor is too far ahead when facing DOWN
        if (dir == Vector2.down)
        {
            worldTarget += new Vector3(0, downFixOffset, 0);
        }

        // 4. Convert world → tile
        Vector3Int cellPos = cropTilemap.WorldToCell(worldTarget);

        // 5. Move cursor to tile center
        transform.position = cropTilemap.GetCellCenterWorld(cellPos);

        // 6. Check if farmland tile exists
        if (cropTilemap.HasTile(cellPos))
        {
            cursorSprite.enabled = true;

            // Select the cropblock at this tile
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

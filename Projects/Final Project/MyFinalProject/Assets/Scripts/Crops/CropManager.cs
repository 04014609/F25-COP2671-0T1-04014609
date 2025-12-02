using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class CropManager : MonoBehaviour
{
    private Grid _cropGrid;
    private List<CropBlock> _cropContainer = new();
    private List<CropBlock> _plantedCrops = new();

    [Header("References")]
    [SerializeField] private CropBlock _cropBlockPrefab; // Assign in Inspector!

    private void Awake()
    {
        _cropGrid = GetComponent<Grid>();
    }

    private void Start()
    {
        if (_cropGrid == null) return;

        // Find ALL tilemaps under this object
        var tilemaps = GetComponentsInChildren<Tilemap>();
        if (tilemaps.Length == 0) return;

        foreach (var tilemap in tilemaps)
        {
            // Optional — hide tilemap visuals
            var renderer = tilemap.GetComponent<TilemapRenderer>();
            if (renderer != null)
                renderer.enabled = false;

            GenerateGridUsingTilemap(tilemap);
        }
    }

    private void GenerateGridUsingTilemap(Tilemap tilemap)
    {
        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cell = new(x, y, 0);

                // Only spawn a CropBlock if the tile actually exists
                if (!tilemap.HasTile(cell))
                    continue;

                // ⭐ Align crop block using the tilemap anchor
                Vector3 worldPos =
                    tilemap.CellToWorld(cell) +
                    (Vector3)tilemap.tileAnchor;  // <- FIXED ALIGNMENT

                // Spawn CropBlock
                CropBlock block = Instantiate(
                    _cropBlockPrefab,
                    worldPos,
                    Quaternion.identity,
                    transform
                );

                block.Initialize(tilemap.name, new Vector2Int(x, y), this);

                _cropContainer.Add(block);
            }
        }
    }

    // Add a planted crop to list
    public void AddToPlantedCrops(CropBlock crop)
    {
        if (!_plantedCrops.Contains(crop))
            _plantedCrops.Add(crop);
    }

    // Remove crop by location
    public void RemoveFromPlantedCrops(Vector2Int loc)
    {
        _plantedCrops.RemoveAll(c => c.Location == loc);
    }
}

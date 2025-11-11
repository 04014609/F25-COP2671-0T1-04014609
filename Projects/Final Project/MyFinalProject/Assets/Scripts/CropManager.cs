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
    [SerializeField] private CropBlock _cropBlockPrefab; // assign prefab in Inspector

    private void Awake()
    {
        _cropGrid = GetComponent<Grid>();
    }

    private void Start()
    {
        if (_cropGrid == null) return;

        var tilemaps = GetComponentsInChildren<Tilemap>();
        if (tilemaps.Length == 0) return;

        // Turn off tilemap renderers to hide visual grid layers
        foreach (var tilemap in tilemaps)
        {
            var r = tilemap.GetComponent<TilemapRenderer>();
            if (r != null) r.enabled = false;

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
                if (!tilemap.HasTile(cell)) continue;

                Vector3 pos = tilemap.CellToWorld(cell);
                CropBlock block = Instantiate(_cropBlockPrefab, pos, Quaternion.identity, transform);
                block.Initialize(tilemap.name, new Vector2Int(x, y), this);
                _cropContainer.Add(block);
            }
        }
    }

    public void AddToPlantedCrops(CropBlock crop) => _plantedCrops.Add(crop);

    public void RemoveFromPlantedCrops(Vector2Int loc) =>
        _plantedCrops.RemoveAll(c => c.Location == loc);
}


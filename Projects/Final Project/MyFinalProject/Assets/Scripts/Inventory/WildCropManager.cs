using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WildCropManager : MonoBehaviour
{
    [Header("Wild Crop Settings")]
    [SerializeField] private SeedPacket[] wildSeeds;  // Strawberry, Sunflower, etc.
    [SerializeField] private float spawnChance = 0.10f;
    [SerializeField] private int maxWildCrops = 50;
    [SerializeField] private int spawnPerDay = 6;

    [Header("References")]
    [SerializeField] private CropBlock cropBlockPrefab;

    private List<Vector3Int> wildCells = new();
    private Tilemap[] wildTilemaps;

    private int CurrentWildCount => FindObjectsByType<CropBlock>(FindObjectsSortMode.None)
                                    .Count(b => b.name.Contains("Wild"));

    private void Awake()
    {
        wildTilemaps = GetComponentsInChildren<Tilemap>();
        CacheTileCells();
    }

    private void Start()
    {
        TimeManager.Instance.OnDayPassed.AddListener(SpawnWildCrops);
    }

    private void CacheTileCells()
    {
        wildCells.Clear();

        foreach (var tilemap in wildTilemaps)
        {
            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cell = new(x, y, 0);
                    if (tilemap.HasTile(cell))
                        wildCells.Add(cell);
                }
            }
        }

        Debug.Log($"[WildCropManager] Cached {wildCells.Count} wild tiles.");
    }

    private void SpawnWildCrops()
    {
        // SAFETY 1 — tiles missing
        if (wildCells.Count == 0)
        {
            Debug.LogWarning("[WildCropManager] No wild tiles found!");
            return;
        }

        // SAFETY 2 — no seeds assigned
        if (wildSeeds == null || wildSeeds.Length == 0)
        {
            Debug.LogWarning("[WildCropManager] No wild seeds assigned!");
            return;
        }

        int existing = CurrentWildCount;
        int toSpawn = Mathf.Min(spawnPerDay, maxWildCrops - existing);

        if (toSpawn <= 0)
        {
            Debug.Log("[WildCropManager] Wild crop limit reached.");
            return;
        }

        int spawned = 0;

        foreach (var tilemap in wildTilemaps)
        {
            foreach (var cell in wildCells)
            {
                if (spawned >= toSpawn) return;
                if (Random.value > spawnChance) continue;

                // Safe world position
                Vector3 worldPos =
                    tilemap.CellToWorld(cell) +
                    (Vector3)tilemap.tileAnchor;

                // Safe random seed
                int idx = Random.Range(0, wildSeeds.Length);
                if (idx < 0 || idx >= wildSeeds.Length) continue;

                SeedPacket seed = wildSeeds[idx];
                if (seed == null || seed.HarvestPrefab == null) continue;

                // Spawn wild CropBlock
                CropBlock block = Instantiate(
                    cropBlockPrefab,
                    worldPos,
                    Quaternion.identity,
                    this.transform
                );

                block.Initialize("Wild", new Vector2Int(cell.x, cell.y), null);

                // Mark as wild + allow auto-growth
                block.PreventUse();
                block.WaterSoil();
                block.PlantSeed(seed);

                spawned++;
            }
        }

        Debug.Log($"[WildCropManager] Spawned {spawned} wild crops safely.");
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WildCropManager : MonoBehaviour
{
    [Header("Wild Crop Settings")]
    [SerializeField] private SeedPacket[] wildSeeds;
    [SerializeField] private float spawnChance = 0.10f; // 10% chance per tile each day
    [SerializeField] private int maxCropsPerDay = 6;    // limit spawns

    [Header("References")]
    [SerializeField] private CropBlock cropBlockPrefab; // same prefab used for farm crops

    private Grid _grid;
    private List<CropBlock> _wildCrops = new();
    private Tilemap[] _wildTilemaps;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _wildTilemaps = GetComponentsInChildren<Tilemap>();
    }

    private void Start()
    {
        // hook into NEXT DAY event
        TimeManager.Instance.OnDayPassed.AddListener(SpawnWildCrops);
    }

    private void SpawnWildCrops()
    {
        int spawned = 0;

        foreach (var tilemap in _wildTilemaps)
        {
            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    if (spawned >= maxCropsPerDay)
                        return;

                    Vector3Int cell = new(x, y, 0);

                    if (!tilemap.HasTile(cell))
                        continue;

                    if (Random.value > spawnChance)
                        continue;

                    // Convert tile position to world position
                    Vector3 worldPos =
                        tilemap.CellToWorld(cell) + (Vector3)tilemap.tileAnchor;

                    // Spawn CropBlock
                    CropBlock block = Instantiate(
                        cropBlockPrefab,
                        worldPos,
                        Quaternion.identity,
                        this.transform
                    );

                    block.Initialize("Wild", new Vector2Int(x, y), null);

                    // pick a random wild seed
                    SeedPacket chosen = wildSeeds[Random.Range(0, wildSeeds.Length)];

                    // PLANT immediately (auto-wild)
                    PlantWildCrop(block, chosen);

                    _wildCrops.Add(block);
                    spawned++;
                }
            }
        }
    }

    private void PlantWildCrop(CropBlock block, SeedPacket seed)
    {
        // convert CropBlock into a planted wild crop
        block.PreventUse();           // player can't plow or water wild crops
        block.WaterSoil();            // mark watered so it grows automatically
        block.PlantSeed(seed);
    }
}

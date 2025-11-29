using UnityEngine;
using UnityEngine.Events;

public class FarmingController : MonoBehaviour
{
    public static FarmingController Instance;

    public static UnityEvent OnHoe = new UnityEvent();
    public static UnityEvent OnWater = new UnityEvent();
    public static UnityEvent OnSeed = new UnityEvent();
    public static UnityEvent OnHarvest = new UnityEvent();
   


    [SerializeField] private CropBlock selectedBlock;
    [SerializeField] private SeedPacket currentSeed;

    private void Awake()
    {
        Instance = this;   // IMPORTANT FIX
    }

    private void OnEnable()
    {
        OnHoe.AddListener(HoeSelected);
        OnWater.AddListener(WaterSelected);
        OnSeed.AddListener(SeedSelected);
        OnHarvest.AddListener(HarvestSelected);
    }

    private void OnDisable()
    {
        OnHoe.RemoveListener(HoeSelected);
        OnWater.RemoveListener(WaterSelected);
        OnSeed.RemoveListener(SeedSelected);
        OnHarvest.RemoveListener(HarvestSelected);
    }

    private void HoeSelected()
    {
        if (selectedBlock != null)
            selectedBlock.PlowSoil();
    }

    private void WaterSelected()
    {
        if (selectedBlock != null)
            selectedBlock.WaterSoil();
    }

    private void SeedSelected()
    {
        if (selectedBlock != null && currentSeed != null)
            selectedBlock.PlantSeed(currentSeed);
    }

    private void HarvestSelected()
    {
        if (selectedBlock != null)
            selectedBlock.HarvestPlants();
    }

    public void SetSelectedBlock(CropBlock block)  // <-- UPDATED NAME
    {
        selectedBlock = block;
    }

    public void SetSeed(SeedPacket seed)
    {
        currentSeed = seed;
        Debug.Log("Selected seed: " + (seed != null ? seed.CropName : "None"));
    }
}

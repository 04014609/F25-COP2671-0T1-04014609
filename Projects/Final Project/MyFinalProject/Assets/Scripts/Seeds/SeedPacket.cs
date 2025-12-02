using UnityEngine;

[CreateAssetMenu(fileName = "NewSeedPacket", menuName = "Farm/Seed Packet")]
public class SeedPacket : ScriptableObject
{
    public enum GrowthStage
    {
        Seed,
        Sprout,
        Young,
        Mature
    }

    [Header("Basic Info")]
    public string CropName;
    public Sprite[] growthSprites;
    public Sprite CoverImage;
    public Harvestable HarvestPrefab;

    [Header("Timing")]
    public float daysToGrow = 4f;

    public Sprite GetIconForStage(GrowthStage stage)
    {
        int index = (int)stage;
        if (growthSprites != null && growthSprites.Length > index)
            return growthSprites[index];
        return null;
    }
}

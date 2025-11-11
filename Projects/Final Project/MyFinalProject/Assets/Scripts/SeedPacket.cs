using UnityEngine;

[CreateAssetMenu(fileName = "NewSeedPacket", menuName = "Farm/Seed Packet")]
public class SeedPacket : ScriptableObject
{
    public string CropName;
    public GameObject HarvestPrefab;
    public Sprite[] GrowthStageIcons;

    public enum GrowthStage
    {
        Seed,
        Sprout,
        Young,
        Mature
    }

    public Sprite GetIconForStage(GrowthStage stage)
    {
        int index = (int)stage;
        if (GrowthStageIcons != null && index < GrowthStageIcons.Length)
            return GrowthStageIcons[index];
        return null;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewHarvestItem", menuName = "Farm/Harvest Item")]
public class HarvestItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int basePrice;
    public int startingQuantity = 0;
}

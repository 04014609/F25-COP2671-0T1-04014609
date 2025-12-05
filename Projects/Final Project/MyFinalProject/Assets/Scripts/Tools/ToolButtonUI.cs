using UnityEngine;
using UnityEngine.UI;

public class ToolButtonUI : MonoBehaviour
{
    public ToolType tool;      // set in Inspector
    private Image img;

    private Color normalColor = Color.white;
    private Color selectedColor = new Color(0.7f, 0.7f, 0.7f); // darker

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    private void OnEnable()
    {
        // all events call the SAME update method
        FarmingController.OnHoe.AddListener(UpdateVisual);
        FarmingController.OnWater.AddListener(UpdateVisual);
        FarmingController.OnSeed.AddListener(UpdateVisual);
        FarmingController.OnHarvest.AddListener(UpdateVisual);
    }

    private void OnDisable()
    {
        FarmingController.OnHoe.RemoveListener(UpdateVisual);
        FarmingController.OnWater.RemoveListener(UpdateVisual);
        FarmingController.OnSeed.RemoveListener(UpdateVisual);
        FarmingController.OnHarvest.RemoveListener(UpdateVisual);
    }

    private void UpdateVisual()
    {
        if (FarmingController.CurrentTool == tool)
            img.color = selectedColor;
        else
            img.color = normalColor;
    }
}

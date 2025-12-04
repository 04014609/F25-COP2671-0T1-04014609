using UnityEngine;

public class InventoryToggleUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup inventoryPanel; // The InventoryPanel CanvasGroup
    private bool isOpen = false;

    private void Start()
    {
        HidePanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        isOpen = !isOpen;

        if (isOpen)
            ShowPanel();
        else
            HidePanel();
    }

    private void ShowPanel()
    {
        inventoryPanel.alpha = 1f;
        inventoryPanel.interactable = true;
        inventoryPanel.blocksRaycasts = true;
    }

    private void HidePanel()
    {
        inventoryPanel.alpha = 0f;
        inventoryPanel.interactable = false;
        inventoryPanel.blocksRaycasts = false;
    }
}

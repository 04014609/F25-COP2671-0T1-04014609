using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;   // assign prefab
    public Transform gridParent;        // assign the Grid with GridLayoutGroup
    private InventorySystem inventory;


    private void Awake()
    {
        inventory = FindFirstObjectByType<InventorySystem>();
    }

    private void OnEnable()
    {
        InventorySystem.OnInventoryChanged += RefreshUI;
    }

    private void OnDisable()
    {
        InventorySystem.OnInventoryChanged -= RefreshUI;
    }

    public void RefreshUI()
    {
        // Clear existing UI
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        // Rebuild UI
        foreach (var slot in inventory.items)
        {
            var newSlot = Instantiate(itemSlotPrefab, gridParent);
            var ui = newSlot.GetComponent<ItemSlotUI>();
            ui.SetSlot(slot.item, slot.quantity);
        }
    }
}

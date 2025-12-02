using UnityEngine;

public class InventoryToggleUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel; // assign your Inventory Panel


    private void Start()
    {
        inventoryPanel.SetActive(false);
    }


    private void Update()
    {
        // Press W to toggle inventory
        if (Input.GetKeyDown(KeyCode.W))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}

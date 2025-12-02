using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> items = new List<InventorySlot>();

    public delegate void InventoryChanged();
    public static event InventoryChanged OnInventoryChanged;

    public void AddItem(HarvestItem item)
    {
        // Look for existing item
        InventorySlot slot = items.Find(s => s.item == item);

        if (slot != null)
        {
            slot.quantity++;
        }
        else
        {
            items.Add(new InventorySlot(item, 1));
        }

        OnInventoryChanged?.Invoke();
    }
}

[System.Serializable]
public class InventorySlot
{
    public HarvestItem item;   // Which item
    public int quantity;       // How many of it

    public InventorySlot(HarvestItem item, int qty)
    {
        this.item = item;
        quantity = qty;
    }
}

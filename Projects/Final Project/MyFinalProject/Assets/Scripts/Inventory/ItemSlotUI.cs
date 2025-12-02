using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text quantityText;

    public void SetSlot(HarvestItem item, int quantity)
    {
        icon.sprite = item.icon;
        quantityText.text = quantity.ToString();
    }
}

using UnityEngine;

public class Harvestable : MonoBehaviour
{
    public HarvestItem itemData;          // <-- NEW: ScriptableObject reference
    public string cropName;
    public SpriteRenderer spriteRenderer;
    public Sprite cropSprite;
    public float popScale = 1.5f;
    public float fadeDuration = 0.3f;

    private bool isCollected = false;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (cropSprite != null)
            spriteRenderer.sprite = cropSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // ⭐ NEW: Add to Inventory BEFORE animation starts
            InventorySystem inv = collision.GetComponent<InventorySystem>();
            if (inv != null && itemData != null)
            {
                inv.AddItem(itemData);
                Debug.Log($"Added {itemData.itemName} to inventory");
            }
            else
            {
                Debug.LogWarning("Harvestable has NO itemData assigned!");
            }

            // Continue with your animation
            StartCoroutine(PopAndFade());
        }
    }

    private System.Collections.IEnumerator PopAndFade()
    {
        Vector3 originalScale = transform.localScale;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            float scale = Mathf.Lerp(1f, popScale, t);
            transform.localScale = originalScale * scale;
            float alpha = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}

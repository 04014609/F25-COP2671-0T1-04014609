using UnityEngine;

public class Harvestable : MonoBehaviour
{
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
            Debug.Log($"{cropName} collected!");
            StartCoroutine(PopAndFade());
        }
    }

    private System.Collections.IEnumerator PopAndFade()
    {
        Vector3 originalScale = transform.localScale;
        float timer = 0f;

        // POP UP
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

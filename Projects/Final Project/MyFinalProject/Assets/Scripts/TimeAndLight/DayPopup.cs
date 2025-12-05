using UnityEngine;
using TMPro;
using System.Collections;

public class DayPopupUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI popupText;

    public float fadeInTime = 0.5f;
    public float stayTime = 1.5f;
    public float fadeOutTime = 0.5f;

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;
        StopAllCoroutines();
        StartCoroutine(PopupRoutine());
    }

    private IEnumerator PopupRoutine()
    {
        float t = 0;

        // Fade In
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeInTime);
            yield return null;
        }

        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(stayTime);

        // Fade Out
        t = 0;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeOutTime);
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}

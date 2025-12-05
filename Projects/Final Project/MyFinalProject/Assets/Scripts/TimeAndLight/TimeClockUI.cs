using UnityEngine;
using TMPro;

public class TimeClockUI : MonoBehaviour
{
    public TextMeshProUGUI clockText;

    private void Update()
    {
        if (clockText == null)
        {
            Debug.LogWarning("ClockText is NOT assigned!");
            return;
        }

        int h = TimeManager.Instance.hours;
        int m = TimeManager.Instance.minutes;

        clockText.text = $"{h:00}:{m:00}";
    }
}

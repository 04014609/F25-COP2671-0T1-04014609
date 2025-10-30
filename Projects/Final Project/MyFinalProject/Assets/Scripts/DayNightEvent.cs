using UnityEngine;
using UnityEngine.Events;

public class DayNightEvents : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnSunrise;
    public UnityEvent OnSunset;

    private bool hasTriggeredSunrise = false;
    private bool hasTriggeredSunset = false;

    void Update()
    {
        int hours = TimeManager.Instance.hours;
        int minutes = TimeManager.Instance.minutes;

        // Sunrise
        if (hours == 6 && !hasTriggeredSunrise)
        {
            Debug.Log("Sunrise Event Triggered");
            OnSunrise?.Invoke();
            hasTriggeredSunrise = true;
            hasTriggeredSunset = false;
        }

        // Sunset
        if (hours == 18 && !hasTriggeredSunset)
        {
            Debug.Log("Sunset Event Triggered");
            OnSunset?.Invoke();
            hasTriggeredSunset = true;
            hasTriggeredSunrise = false;
        }
    }
}


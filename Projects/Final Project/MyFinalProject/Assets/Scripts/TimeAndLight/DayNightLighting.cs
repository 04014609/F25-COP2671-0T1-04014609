using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightLighting : MonoBehaviour
{
    [Header("References")]
    public Light2D globalLight;  // For 2D URP light (use Light2D)

    [Header("Lighting Settings")]
    public Gradient lightColor;  // Controls light color over time
    public AnimationCurve lightIntensity; // Controls brightness over time

    [Range(0, 24)]
    public float currentHour; // Useful for testing in editor

    private void Update()
    {
        // Get the time from TimeManager
        float hour = TimeManager.Instance.hours + (TimeManager.Instance.minutes / 60f);
        currentHour = hour;

        // Normalize to 0–1 range (0 = midnight, 1 = next midnight)
        float t = hour / 24f;

        // Apply color and intensity from curves
        if (globalLight != null)
        {
            globalLight.color = lightColor.Evaluate(t);
            globalLight.intensity = lightIntensity.Evaluate(t);
        }
    }

    //  Triggered at 6:00
    public void OnSunrise()
    {
        Debug.Log(" Lighting: Sunrise transition triggered.");
        if (globalLight != null)
        {
            // Sample color & intensity near morning values (0.25 ≈ 6 AM)
            globalLight.color = lightColor.Evaluate(0.25f);
            globalLight.intensity = lightIntensity.Evaluate(0.25f);
        }
    }

    //  Triggered at 18:00
    public void OnSunset()
    {
        Debug.Log("Lighting: Sunset transition triggered.");
        if (globalLight != null)
        {
            // Sample color & intensity near evening values (0.75 - 6 PM)
            globalLight.color = lightColor.Evaluate(0.75f);
            globalLight.intensity = lightIntensity.Evaluate(0.75f);
        }
    }
}

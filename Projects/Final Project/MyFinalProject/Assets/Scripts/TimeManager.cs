using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;  // Singleton reference

    [Header("Time Settings")]
    public float realSecondsPerGameMinute = 1f; // 1 real sec = 1 game min
    public int hours = 6;
    public int minutes = 0;

    [Header("Events")]
    public UnityEvent OnMinutePassed; // Called every in-game minute

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Start the time coroutine
        StartCoroutine(TimeTick());
    }

    private System.Collections.IEnumerator TimeTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(realSecondsPerGameMinute);

            minutes++;

            if (minutes >= 60)
            {
                minutes = 0;
                hours++;

                if (hours >= 24)
                    hours = 0;
            }

            Debug.Log($"Time: {hours:D2}:{minutes:D2}");

            // Trigger event for other scripts
            OnMinutePassed?.Invoke();
        }
    }
}

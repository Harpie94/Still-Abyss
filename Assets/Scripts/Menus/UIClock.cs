using UnityEngine;

public class ClockUI : MonoBehaviour
{
    public float cycleDurationMinutes = 1f;
    private float cycleDurationSeconds;
    private float timer = 0f;
    private bool isDay = true;

    public float CurrentHour { get; private set; }
    public float CurrentMinute { get; private set; }
    public Transform mainClock;

    void Start()
    {
        cycleDurationSeconds = cycleDurationMinutes * 60f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / cycleDurationSeconds;

        float startHour, endHour;

        if (isDay)
        {
            startHour = 6f;
            endHour = 18f;
        }
        else
        {
            startHour = 18f;
            endHour = 30f;
        }

        float currentHour = Mathf.Lerp(startHour, endHour, t);
        if (currentHour >= 24f) currentHour -= 24f;

        int hours = Mathf.FloorToInt(currentHour) % 24;
        int minutes = Mathf.FloorToInt((currentHour - hours) * 60f);

        CurrentHour = hours;
        CurrentMinute = minutes;

        // ----- ROTATION -----
        float angle = (currentHour / 24f) * 360f;

        if (mainClock != null)
            mainClock.localRotation = Quaternion.Euler(0, 0, angle + 90);

        if (timer >= cycleDurationSeconds)
        {
            timer = 0f;
            isDay = !isDay;
        }
    }
}

using UnityEngine;
using TMPro;

public class DayNightScript : MonoBehaviour
{
    public Light sun;
    public float cycleDurationMinutes = 1f;
    public int totalCycles = 5;
    public bool isDay = true;
    public bool InfiniteCycle = false;
    public TMP_Text timerText;

    private float cycleDurationSeconds;
    private int currentCycle = 0;
    private float timer = 0f;

    void Start()
    {
        cycleDurationSeconds = cycleDurationMinutes * 60f;
        if (sun == null)
        {
            sun = GetComponent<Light>();
        }
    }

    void Update()
    {
        if (!InfiniteCycle && currentCycle >= totalCycles * 2)
            return;

        timer += Time.deltaTime;
        float t = timer / cycleDurationSeconds;

        float startAngle, endAngle;
        float startHour, endHour;

        //Ce qui défini le début et la fin des cycles jour et nuit selon les angles et les heures
        if (isDay)
        {
            startAngle = 0f;
            endAngle = 180f;
            startHour = 6f;
            endHour = 18f;
        }
        else
        {
            startAngle = 180f;
            endAngle = 360f;
            startHour = 18f;
            endHour = 30f;
        }

        float targetAngle = Mathf.Lerp(startAngle, endAngle, t);
        sun.transform.rotation = Quaternion.Euler(targetAngle, 0f, 0f);

        float currentHour = Mathf.Lerp(startHour, endHour, t);
        if (currentHour >= 24f) currentHour -= 24f;

        int hours = Mathf.FloorToInt(currentHour) % 24;
        int minutes = Mathf.FloorToInt((currentHour - hours) * 60f);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", hours, minutes);

        if (timer >= cycleDurationSeconds)
        {
            timer = 0f;
            isDay = !isDay;
            currentCycle++;
        }
    }
}

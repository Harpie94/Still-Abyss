using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DayNightScript : MonoBehaviour
{
    public float cycleDurationMinutes = 1f;
    public int totalCycles = 5;
    public bool isDay = true;
    public bool InfiniteCycle = false;
    public float lightThreshold = 7f;
    public float dayFactor = 4f;
    public TMP_Text timerText;
    public float CurrentHour { get; private set; }
    public float CurrentMinute { get; private set; }

    public UnityEvent DayNightSwitch;

    private float cycleDurationSeconds;
    private int currentCycle = 0;
    private float timer = 0f;

    void Start()
    {
        cycleDurationSeconds = cycleDurationMinutes * 60f;
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

        //float targetAngle = Mathf.Lerp(startAngle, endAngle, t);
        //sun.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        float currentHour = Mathf.Lerp(startHour, endHour, t);
        if (currentHour >= 24f) currentHour -= 24f;

        int hours = Mathf.FloorToInt(currentHour) % 24;
        int minutes = Mathf.FloorToInt((currentHour - hours) * 60f);

        SetLightIntensity(currentHour);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", hours, minutes);

        if (timer >= cycleDurationSeconds)
        {
            timer = 0f;
            isDay = !isDay;
            DayNightSwitch?.Invoke();
            currentCycle++;
        }

        CurrentHour = hours;
        CurrentMinute = minutes;
    }

    void SetLightIntensity(float currentHour)
    {
        float intensityCalculator = Mathf.Abs(12 - currentHour);
        intensityCalculator = Mathf.Clamp(intensityCalculator, 0f, lightThreshold);
        intensityCalculator = Mathf.Abs(intensityCalculator - lightThreshold);
        intensityCalculator = Mathf.Clamp(intensityCalculator, 0f, lightThreshold - dayFactor);
        intensityCalculator = Mathf.Lerp(0, 1, intensityCalculator / (lightThreshold - dayFactor));
        RenderSettings.ambientLight = new Color(0.3671235f * intensityCalculator, 0.5036934f * intensityCalculator, 0.6226415f * intensityCalculator);
    }
}

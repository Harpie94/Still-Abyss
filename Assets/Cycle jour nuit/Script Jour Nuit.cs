using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Light sun;
    public float cycleDurationMinutes = 1f;
    public int totalCycles = 5;

    private float cycleDurationSeconds;
    private int currentCycle = 0;
    private bool isDay = true;
    private float timer = 0f;

    void Start()
    {
        cycleDurationSeconds = cycleDurationMinutes * 60f;
        if (sun == null)
        {
            sun = GetComponent<Light>();
        }
        SetSunRotation(isDay);
    }

    void Update()
    {
        if (currentCycle >= totalCycles * 2)
            return;

        timer += Time.deltaTime;
        float t = timer / cycleDurationSeconds;

        float targetAngle = isDay ? Mathf.Lerp(0f, 180f, t) : Mathf.Lerp(180f, 360f, t);
        sun.transform.rotation = Quaternion.Euler(targetAngle, 0f, 0f);

        if (timer >= cycleDurationSeconds)
        {
            timer = 0f;
            isDay = !isDay;
            currentCycle++;
            SetSunRotation(isDay);
        }
    }

    void SetSunRotation(bool day)
    {
        float angle = day ? 0f : 180f;
        sun.transform.rotation = Quaternion.Euler(angle, 0f, 0f);
    }
}

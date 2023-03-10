using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayAndNightCycle : MonoBehaviour
{
    [SerializeField]
    private float timeMultiplier;

    [SerializeField]
    private float startHour;

    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private float sunRiseHour;

    [SerializeField]
    private float sunSetHour;

    [SerializeField]
    private Color dayAmbientLight;

    [SerializeField]
    private Color nightAmbientLight;

    [SerializeField]
    private AnimationCurve lightChangeCurve;

    [SerializeField]
    private float maxSunLightIntensity;

    [SerializeField]
    private Light moonLight;

    [SerializeField]
    private float maxMoonLightIntensity;

    [SerializeField]
    private DateTime currentTime;

    [SerializeField]
    private TimeSpan sunRiseTime;

    [SerializeField]
    private TimeSpan sunsSetTime;  

    [SerializeField]
    private TextMeshProUGUI timeText;

    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunRiseTime = TimeSpan.FromHours(sunRiseHour);
        sunsSetTime = TimeSpan.FromHours(sunSetHour);
    }

    void Update()
    {
        UpdateTimeOfDelay();
        RotateSun();
    }

    private void UpdateTimeOfDelay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime + timeMultiplier);

        if(timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if(currentTime.TimeOfDay > sunRiseTime && currentTime.TimeOfDay < sunsSetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunRiseTime, sunsSetTime);
            TimeSpan timeSinceSurface = CalculateTimeDifference(sunRiseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSurface.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunsetDuration = CalculateTimeDifference(sunsSetTime, sunRiseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsSetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SolarLightManager : MonoBehaviour
{
    #region Solar Light Manager Variables

    public enum DayMoment
    {
        Dawn, //amanecer
        Day, //día
        Noon, //mediodía
        Afternoon, //tarde
        Dusk, //atardecer
        Evening, //noche
        Midnight //medianoche
    }

    [Header("Day Moment")]
    public DayMoment dayMoment;

    [Header("Current Sunlight")]
    public float sunLightAmount;

    [Header("Sunlight Amounts")]
    public float dawnSunlightAmount;
    public float daySunlightAmount;
    public float noonSunlightAmount;
    public float afternoonSunlightAmount;
    public float duskSunlightAmount;
    public float eveningSunlightAmount;
    public float midnightSunlightAmount;

    #endregion Solar Light Manager Variables

    #region Time Management and Different Sunlight Variables

    private int minutes;
    private int hours;
    private float tempSeconds;

    public int Minutes { get { return minutes; } set { minutes = value; OnMinutesChange(value); } }
    public int Hours { get { return hours; } set { hours = value; OnHoursChange(value); } }

    [SerializeField]
    private Gradient gradientDawnToDay;
    [SerializeField]
    private Gradient gradientDayToNoon;
    [SerializeField]
    private Gradient gradientNoonToAfternoon;
    [SerializeField]
    private Gradient gradientAfternoonToDusk;
    [SerializeField]
    private Gradient gradientDuskToEvening;
    [SerializeField]
    private Gradient gradientEveningToMidnight;
    [SerializeField]
    private Gradient gradientMidnightToDawn;

    /*[SerializeField]
    private Texture2D skyboxNight;
    [SerializeField]
    private Texture2D skyboxSunrise;
    [SerializeField]
    private Texture2D skyboxDay;
    [SerializeField]
    private Texture2D skyboxSunset;*/

    [SerializeField]
    private Light terrainLight;

    #endregion Time Management and Different Sunlight Variables

    #region UI

    [Header("UI")]
    public TextMeshProUGUI currentSunlightAmountText;

    #endregion UI

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        Hours = 22;
        OnHoursChange(Hours);
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        tempSeconds += Time.deltaTime;

        if (tempSeconds >= 1)
        {
            Minutes += 1;
            tempSeconds = 0;
        }

        Debug.Log("Time: " + Hours + " : " + Minutes);

        HandleUI();
    }

    #endregion Update

    #region Handle SunLight Depending On Day Moment

    private void HandleSunLightDependingOnDayMoment()
    {
        switch (dayMoment)
        {
            case DayMoment.Dawn:
                //Interpolar la cantidad de luz solar entre diferentes variables

                //StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 10f));
                StartCoroutine(LerpLight(gradientMidnightToDawn, 10f));
                sunLightAmount = Mathf.Lerp(midnightSunlightAmount, dawnSunlightAmount, 10f);

                break;
            case DayMoment.Day:

                //StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 10f));
                StartCoroutine(LerpLight(gradientDawnToDay, 10f));
                sunLightAmount = Mathf.Lerp(dawnSunlightAmount, daySunlightAmount, 10f);

                break;
            case DayMoment.Noon:

                //StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
                StartCoroutine(LerpLight(gradientDayToNoon, 10f));
                sunLightAmount = Mathf.Lerp(daySunlightAmount, noonSunlightAmount, 10f);

                break;
            case DayMoment.Afternoon:

                //StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
                StartCoroutine(LerpLight(gradientNoonToAfternoon, 10f));
                sunLightAmount = Mathf.Lerp(noonSunlightAmount, afternoonSunlightAmount, 10f);

                break;
            case DayMoment.Dusk:

                //StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
                StartCoroutine(LerpLight(gradientAfternoonToDusk, 10f));
                sunLightAmount = Mathf.Lerp(afternoonSunlightAmount, duskSunlightAmount, 10f);

                break;
            case DayMoment.Evening:

                //StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
                StartCoroutine(LerpLight(gradientDuskToEvening, 10f));
                sunLightAmount = Mathf.Lerp(duskSunlightAmount, eveningSunlightAmount, 10f);

                break;
            case DayMoment.Midnight:

                //StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 10f));
                StartCoroutine(LerpLight(gradientEveningToMidnight, 10f));
                sunLightAmount = Mathf.Lerp(eveningSunlightAmount, midnightSunlightAmount, 10f);

                break;
            default:
                break;
        }
    }

    #endregion Handle SunLight Depending On Day Moment

    #region Handle Time And Sunlight Depending On Time

    private void OnMinutesChange(int value)
    {
        terrainLight.transform.Rotate(Vector3.up, (1f / 1440f) * 360f, Space.World);

        if (value >= 60)
        {
            Hours += 1;
            minutes = 0;
        }

        if (Hours >= 24)
        {
            Hours = 0;
        }
    }

    private void OnHoursChange(int value)
    {
        if (value == 6)
        {
            dayMoment = DayMoment.Dawn;
            HandleSunLightDependingOnDayMoment();
        }
        else if (value == 9)
        {
            dayMoment = DayMoment.Day;
            HandleSunLightDependingOnDayMoment();
        }
        else if (value == 12)
        {
            dayMoment = DayMoment.Noon;
            HandleSunLightDependingOnDayMoment();
        }
        else if (value == 17)
        {
            dayMoment = DayMoment.Afternoon;
            HandleSunLightDependingOnDayMoment();
        }
        else if (value == 22)
        {
            dayMoment = DayMoment.Evening;
            HandleSunLightDependingOnDayMoment();
        }
        else if (value == 23 || (value < 1 && value < 6))
        {
            dayMoment = DayMoment.Midnight;
            HandleSunLightDependingOnDayMoment();
        }
    }

    private IEnumerator LerpSkybox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);

        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }

        RenderSettings.skybox.SetTexture("_Texture1", b);
    }

    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (int i = 0; i < time; i++)
        {
            terrainLight.color = lightGradient.Evaluate(i / time);
            yield return null;
        }
    }

    #endregion Handle Time And Sunlight Depending On Time

    #region Handle UI

    private void HandleUI()
    {
        currentSunlightAmountText.text = sunLightAmount.ToString("n0");
    }

    #endregion Handle UI

}

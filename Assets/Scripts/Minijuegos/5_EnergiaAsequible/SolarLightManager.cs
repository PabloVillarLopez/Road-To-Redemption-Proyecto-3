using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float sunLightAmount;

    #endregion Solar Light Manager Variables

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion Update

    #region Handle SunLight Depending On Day Moment

    private void HandleSunLightDependingOnDayMoment()
    {
        switch (dayMoment)
        {
            case DayMoment.Dawn:
                //Interpolar la cantidad de luz solar entre diferentes variables
                break;
            case DayMoment.Day:
                break;
            case DayMoment.Noon:
                break;
            case DayMoment.Afternoon:
                break;
            case DayMoment.Dusk:
                break;
            case DayMoment.Evening:
                break;
            case DayMoment.Midnight:
                break;
            default:
                break;
        }
    }

    #endregion Handle SunLight Depending On Day Moment
}

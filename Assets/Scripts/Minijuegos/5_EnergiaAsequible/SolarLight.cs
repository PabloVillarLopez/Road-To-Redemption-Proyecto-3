using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SolarLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    public GameObject sunIcon;
    public GameObject sunPercentIcon;
    public TextMeshProUGUI sunPercentText;
    public int sunPercent;
    public static bool canShowSunPercent = false;
    public bool sunSelectedCorrectly = false;
    public int[] maxSunPercent = new int[4];
    public GameObject[] solarLightPlaces;
    public GameObject contratulationsPanelPhase2;
    public GameObject mistakePanelPhase2;
    public static bool randomCorroutinCanStart = false;
    public GameObject plaqueMounted;
    public int id;

    [Header("Plaque mounted spawn points")]
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject spawnPoint3;
    public GameObject spawnPoint4;

    // Start is called before the first frame update
    void Start()
    {
        SetInitialRandomSunPercent();

        contratulationsPanelPhase2.SetActive(false);
        mistakePanelPhase2.SetActive(false);
        sunPercentText.gameObject.SetActive(false);
        sunIcon.SetActive(false);
        sunPercentIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSunPercentText();
        Invoke("CheckMinigameFinished", 3f);
        //Invoke("RandomChange", 3f);
        //Invoke("ResetRandomChange", 3f);
    }

    /*private void OnMouseOver()
    {
       
    }

    private void OnMouseExit()
    {
        
    }*/

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.gameObject);
        eventData.pointerEnter.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        //transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        Debug.Log("Entrado");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.gameObject);
        eventData.pointerEnter.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        //transform.localScale = new Vector3(1f, 1f, 1f);
        Debug.Log("Salido");
    }

    public IEnumerator RandomChangeSunPercent()
    {
        while (!sunSelectedCorrectly)
        {
            yield return new WaitForSeconds(3f);
            sunPercent = Random.Range(0, 101);
            //StartCoroutine(RandomChangeSunPercent());
        }

    }

    private void RandomChange()
    {
        if (!sunSelectedCorrectly && randomCorroutinCanStart)
        {
            StartCoroutine(RandomChangeSunPercent());
            randomCorroutinCanStart = false;
        }
        else
        {
            StopAllCoroutines();
        }
        
    }

    private void ResetRandomChange()
    {
        randomCorroutinCanStart = true;
    }

    private void SetInitialRandomSunPercent()
    {
        sunPercent = Random.Range(0, 101);
    }

    private void UpdateSunPercentText()
    {
        if (canShowSunPercent)
        {
            sunPercentText.gameObject.SetActive(true);
            sunIcon.SetActive(true);
            sunPercentIcon.SetActive(true);
            sunPercentText.text = sunPercent.ToString();
        }
    }

    private int FindHighestSunValue()
    {
        for (int i = 0; i < solarLightPlaces.Length; i++)
        {
            Debug.Log(i);
            maxSunPercent[i] = solarLightPlaces[i].GetComponent<SolarLight>().sunPercent;
            
        }

        int max = Mathf.Max(maxSunPercent);
        return max;
    }

    private void CheckBestOptionToPlacePlaque()
    {
        if (sunPercent == FindHighestSunValue())
        {
            sunSelectedCorrectly = true;
            contratulationsPanelPhase2.SetActive(true);
            plaqueMounted.SetActive(true);

            switch (id)
            {
                case 1:
                    plaqueMounted.transform.localPosition = spawnPoint1.transform.position;
                    plaqueMounted.transform.localScale = new Vector3(15f, 15f, 15f);
                    break;
                case 2:
                    plaqueMounted.transform.localPosition = spawnPoint2.transform.position;
                    plaqueMounted.transform.localScale = new Vector3(15f, 15f, 15f);
                    break;
                case 3:
                    plaqueMounted.transform.localPosition = spawnPoint3.transform.position;
                    plaqueMounted.transform.localScale = new Vector3(15f, 15f, 15f);
                    break;
                case 4:
                    plaqueMounted.transform.localPosition = spawnPoint4.transform.position;
                    plaqueMounted.transform.localScale = new Vector3(15f, 15f, 15f);
                    break;
                default:
                    break;
            }
            
            energyMinigameManager.minigamePhase2Completed = true;
            //StopAllCoroutines();
        }
        else
        {
            mistakePanelPhase2.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CheckBestOptionToPlacePlaque();
    }

    private void CheckMinigameFinished()
    {
        if (energyMinigameManager.minigamePhase2Completed)
        {
            StopAllCoroutines();
        }
    }
}

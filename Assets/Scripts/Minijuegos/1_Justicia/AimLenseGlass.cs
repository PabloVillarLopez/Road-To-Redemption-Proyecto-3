using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AimLenseGlass : MonoBehaviour
{
    #region Aim with Lense Variables
    [Header("Aim with Lense Variables")]
    public Vector3 normalPose, aimPose;
    private Vector3 aproachAimPose;
    public float aimSpeed;
    public Camera playerCam;
    public GameObject aimPoint;
    public GameObject pressLeftClickIndicatorEnglish;
    public GameObject pressLeftClickIndicatorSpanish;
    public TextMeshProUGUI InvestigatingText;

    #endregion Aim with Lense Variables

    #region Take clues Variable
    [Header("Take clues Variable")]
    public LayerMask Clues;
    public static int takenClues;

    #endregion Take clues Variable

    #region Show Clues Icons Variables
    [Header("Clue Icons")]
    [Space]
    public GameObject[] clueIcons;

    #endregion Show Clues Icons Variables

    [Header("Sound References")]
    public ObserveObject minigameManager;

    [Header("VFX Clues")]
    public GameObject[] cluesVFX;

    [Header("Arrows")]
    public GameObject[] arrowsClues;

    private void Awake()
    {
        for (int i = 0; i < clueIcons.Length; i++)
        {
            clueIcons[i].SetActive(false);
        }
    }

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        
        aproachAimPose = new Vector3(0.020f, -0.3f, 1f);

        CheckLanguageAndShowLeftClickInteractor();
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartLenseAim();
        }
        else
        {
            StopLenseAim();
        }

        Debug.Log(Vector3.Distance(transform.localPosition, aimPose) < 0.15f);

        if (Vector3.Distance(transform.localPosition, aimPose) < 0.15f) //transform.localPosition.x ) ; //aimPose)
        {
            TakeClues();
        }

    }

    #endregion Update

    #region Start Lense Aim
    private void StartLenseAim()
    {
        transform.localPosition = Vector3.Slerp(transform.localPosition, aimPose, aimSpeed * Time.deltaTime);
        playerCam.fieldOfView -= 40 * Time.deltaTime;
        playerCam.fieldOfView = Mathf.Clamp(playerCam.fieldOfView, 30, 60);
        HideInteractors();
        //PressLeftClickText.text = string.Empty;
        InvestigatingText.text = "Investigando...";
    }

    #endregion Start Lense Aim

    #region Stop Lense Aim
    private void StopLenseAim()
    {
        transform.localPosition = Vector3.Slerp(transform.localPosition, normalPose, aimSpeed * Time.deltaTime);
        playerCam.fieldOfView += 40 * Time.deltaTime;
        playerCam.fieldOfView = Mathf.Clamp(playerCam.fieldOfView, 30, 60);
        CheckLanguageAndShowLeftClickInteractor();
        //PressLeftClickText.text = "Press Left Click";
        InvestigatingText.text = string.Empty;
    }

    #endregion Stop Lense Aim

    #region Take Clues
    private void TakeClues()
    {
        RaycastHit hit;

        if (Physics.Raycast(aimPoint.transform.position, aimPoint.transform.TransformDirection(Vector3.forward), out hit, 10f, Clues)) //playerCam.transform.forward
        {
            minigameManager.PlaySound(1);
            Debug.DrawRay(aimPoint.transform.position, aimPoint.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); //playerCam.transform.forward
            ObserveObject.TakenClues.Add(hit.transform.gameObject);
            hit.transform.gameObject.SetActive(false);
            clueIcons[hit.transform.gameObject.GetComponent<RotateClue>().id].SetActive(true);
            cluesVFX[hit.transform.gameObject.GetComponent<RotateClue>().id].SetActive(false);
            arrowsClues[hit.transform.gameObject.GetComponent<RotateClue>().id].SetActive(false);
            takenClues++;
        }

        Debug.DrawRay(aimPoint.transform.position, aimPoint.transform.TransformDirection(Vector3.forward), Color.yellow); //playerCam.transform.forward

        if (takenClues >= 3)
        {
            //PressLeftClickText.text = string.Empty;
            HideInteractors();
            InvestigatingText.text = string.Empty;
        }
    }

    #endregion Take Clues

    private void CheckLanguageAndShowLeftClickInteractor()
    {
        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            pressLeftClickIndicatorEnglish.SetActive(true);
            pressLeftClickIndicatorSpanish.SetActive(false);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            pressLeftClickIndicatorEnglish.SetActive(false);
            pressLeftClickIndicatorSpanish.SetActive(true);
        }
    }

    private void HideInteractors()
    {
        pressLeftClickIndicatorEnglish.SetActive(false);
        pressLeftClickIndicatorSpanish.SetActive(false);
    }
}

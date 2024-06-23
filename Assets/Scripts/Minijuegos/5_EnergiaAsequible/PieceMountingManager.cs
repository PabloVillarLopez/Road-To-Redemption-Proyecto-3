using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PieceMountingManager : MonoBehaviour
{
    #region Piece Variables

    private float pressingTimer;
    public static bool piece1WellMounted, piece2WellMounted, piece3WellMounted, piece4WellMounted, piece5WellMounted, piece6WellMounted, piece7WellMounted;
    public static bool piece8WellMounted, piece9WellMounted, piece10WellMounted, piece11WellMounted;

    public Camera playerCamera;
    public GameObject player;
    public Camera mountingCamera;
    public Camera phase2Camera;
    public LayerMask solarPieces;
    public GameObject solarPlaqueMounted;
    public GameObject[] pieces;
    public GameObject congratulationsPanel;
    //public TextMeshProUGUI pressLeftClickText;
    public GameObject pressLeftClickInteractorEnglish;
    public GameObject pressLeftClickInteractorSpanish;
    private Animator currentAnimator;
    public GameObject interactIndicatorEnglish;
    public GameObject interactIndicatorSpanish;
    public GameObject middleSight;

    [Header("Instructions Panel")]
    public GameObject instructionsPanel;
    public TextMeshProUGUI instructionsPanelText;

    private int piecesMounted = 0;
    public energyMinigameManager minigameManager;

    #endregion Piece Variables

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        mountingCamera.gameObject.SetActive(false);
        phase2Camera.gameObject.SetActive(false);
        solarPlaqueMounted.SetActive(false);
        congratulationsPanel.SetActive(false);

        pressLeftClickInteractorEnglish.SetActive(false);
        pressLeftClickInteractorSpanish.SetActive(false);
        //pressLeftClickText.gameObject.SetActive(false);

    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        DetectPart();
    }

    #endregion Update

    #region OnMouse Methods

    /*private void OnMouseDrag()
    {
        pressingTimer++;
    }*/

    /*private void OnMouseUp()
    {
        if (pressingTimer < 5)
        {
            pressingTimer = 0;
        } 
    }*/

    #endregion OnMouse Methods

    #region CheckTimer Method

    /*private void CheckTimer()
    {
        if (pressingTimer >= 5)
        {
            //Hacer la animación o mover a algún sitio
        }
    }*/

    #endregion CheckTimer Method

    private void DetectPart()
    {
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.gameObject.transform.position, playerCamera.gameObject.transform.forward); //playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(playerCamera.gameObject.transform.position, playerCamera.gameObject.transform.forward + new Vector3(0, 0, 1000), Color.yellow);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, solarPieces) && playerCamera.gameObject.activeInHierarchy)
        {
            Debug.Log("Ha dado");

            if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                pressLeftClickInteractorEnglish.SetActive(true);
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                pressLeftClickInteractorSpanish.SetActive(true);
            }

            //pressLeftClickText.gameObject.SetActive(true);

            currentAnimator = hit.collider.gameObject.GetComponent<Animator>();

            if (Input.GetMouseButtonDown(0))
            {
                hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                minigameManager.PlaySound(1);
                StartCoroutine(MountPiece());
            }
        }
        else
        {
            //pressLeftClickText.gameObject.SetActive(false);
            pressLeftClickInteractorEnglish.SetActive(false);
            pressLeftClickInteractorSpanish.SetActive(false);
        }

        
    }

    private IEnumerator MountPiece()
    {
        PauseMenuManager.canPause = false;
        middleSight.SetActive(false);

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            interactIndicatorEnglish.SetActive(false);
        }

        if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            interactIndicatorSpanish.SetActive(false);
        }
        
        player.SetActive(false);
        piecesMounted++;
        pressLeftClickInteractorEnglish.SetActive(false);
        pressLeftClickInteractorSpanish.SetActive(false);
        //pressLeftClickText.gameObject.SetActive(false);
        instructionsPanel.SetActive(false);
        mountingCamera.gameObject.SetActive(true); //activaría la cámara de montar piezas
        playerCamera.gameObject.SetActive(false); //desactivaría la cámara del personaje
        currentAnimator.Play("Mounting"); //daría play a la animación según que pieza sea
        yield return new WaitForSeconds(3f); //esperar unos segundos
        if (piecesMounted >= 3)
        {
            minigameManager.PlaySound(2); //positive feedback sound
            for (int i = 0; i < pieces.Length; i++)
            {
                pieces[i].SetActive(false);
            }

            solarPlaqueMounted.SetActive(true);
            congratulationsPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            middleSight.SetActive(false);
            energyMinigameManager.phase = energyMinigameManager.Phase.PHASE2;
        }
        else
        {
            player.SetActive(true);
            playerCamera.gameObject.SetActive(true); //volvería a activar la cámara del personaje
            mountingCamera.gameObject.SetActive(false); //volvería a desactivar la cámara de montar piezas
            instructionsPanel.SetActive(false);
            middleSight.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        PauseMenuManager.canPause = true;
    }

    public void BackToPlayerCamera()
    {
        minigameManager.PlaySound(3); //buttonSound
        mountingCamera.gameObject.SetActive(false);
        solarPlaqueMounted.SetActive(false);
        phase2Camera.gameObject.SetActive(true);
        SolarLight.canShowSunPercent = true;
        //instructionsPanel.SetActive(true);
        //instructionsPanel.transform.localPosition = new Vector3(67, -377, 0);
        //instructionsPanelText.text = "Fase 2. Selecciona la parte del tejado que más porcentaje de sol tenga en ese momento para colocar la placa solar.";


        //playerCamera.gameObject.SetActive(true);
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = false;
    }
}

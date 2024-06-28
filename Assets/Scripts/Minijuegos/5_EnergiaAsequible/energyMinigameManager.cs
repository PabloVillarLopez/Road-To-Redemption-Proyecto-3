using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class energyMinigameManager : MonoBehaviour
{
    #region Minigame Phases


    public enum Phase
    {
        PHASE1, //Mount solar plaques
        PHASE2, //Install solar plaques
        PHASE3  //Install cables
    }

    [Header("Minigame Phase")]
    [Tooltip("Tres fases: Montaje placa solar, Instalación placas y Cableado")]
    public static Phase phase;
    public PanelFader cablePanelFader;

    #endregion Minigame Phases

    #region Phase 3 Variables
    [Header("Phase 3 Variables")]
    public GameObject cablePanel;
    public GameObject congratulationsPhase3Panel;
    //public TextMeshProUGUI congratulationsPhase3PanelText;

    public static int globalElectricity;
    public TextMeshProUGUI globalElectricityText;
    public SlotScript slotScript;

    [Header("Check if cables positioned")]
    public NewDragAnDrop cable1;
    public NewDragAnDrop cable2;
    public NewDragAnDrop cable3;
    public NewDragAnDrop cable4;
    public NewDragAnDrop cable5;
    public NewDragAnDrop cable6;

    #endregion Phase 3 Variables

    [Header("SolarLight References")]
    public SolarLight solarLightPlace1;
    public SolarLight solarLightPlace2;
    public SolarLight solarLightPlace3;
    public SolarLight solarLightPlace4;

    public static bool minigamePhase2Completed = false;

    [Header("Instructions Panel")]
    public GameObject instructionsPanel;
    public TextMeshProUGUI instructionsPanelText;

    [Header("Phase 3 slot cables")]
    public GameObject[] slotCables;

    [Header("Stamp Panel")]
    //public GameObject stampPanel;
    public bool canCheckElectricity = true;

    [Header("Cable Camera")]
    [Space]
    public Camera cableCamera;

    [Header("UI Panels")]
    public GameObject initialInstructionsEnglish;
    public GameObject initialInstructionsSpanish;
    public GameObject tutorial1English;
    public GameObject tutorial1Spanish;
    public GameObject tutorial2English;
    public GameObject tutorial2Spanish;
    public GameObject tutorial3English;
    public GameObject tutorial3Spanish;
    public GameObject finishPanelEnglish;
    public GameObject finishPanelSpanish;
    public GameObject pauseIndicatorEnglish;
    public GameObject pauseIndicatorSpanish;
    public GameObject robotIcon;
    public GameObject instructionsIndicatorEnglish;
    public GameObject instructionsIndicatorSpanish;
    private bool canShowInstructions = false;

    [Header("Sound Variables")]
    public List<AudioClip> soundClips = new List<AudioClip>(); // Lista de clips de sonido
    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioSource audioSourceBackground;

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < slotCables.Length; i++)
        {
            slotCables[i].GetComponent<SlotScript>().initialSlotIndividualElectricity = slotCables[i].GetComponent<SlotScript>().slotIndividualElectricity;
        }

        cablePanel.SetActive(false);
        instructionsPanel.SetActive(false);
        //instructionsPanelText.text = "Fase 1. Encuentra las partes de la placa solar y mira alrededor de ellas hasta que salga que puedes hacer click izquierdo sobre ellas.";
        congratulationsPhase3Panel.SetActive(false);
        slotScript.electricityFailObject.SetActive(false);
        //stampPanel.SetActive(false);
        cableCamera.gameObject.SetActive(false);
        robotIcon.SetActive(false);
        instructionsIndicatorEnglish.SetActive(false);
        instructionsIndicatorSpanish.SetActive(false);

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            initialInstructionsEnglish.SetActive(true);
            initialInstructionsSpanish.SetActive(false);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            initialInstructionsEnglish.SetActive(false);
            initialInstructionsSpanish.SetActive(true);
        }

        tutorial1English.SetActive(false);
        tutorial1Spanish.SetActive(false);
        tutorial2English.SetActive(false);
        tutorial2Spanish.SetActive(false);
        tutorial3English.SetActive(false);
        tutorial3Spanish.SetActive(false);
        finishPanelEnglish.SetActive(false);
        finishPanelSpanish.SetActive(false);
        MouseLook.canLook = false;
        ObserveObject.cantMove = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseMenuManager.canPause = false;
        pauseIndicatorEnglish.SetActive(false);
        pauseIndicatorSpanish.SetActive(false);

        if (audioSourceBackground != null)
        {
            audioSourceBackground.clip = soundClips[0];
            audioSourceBackground.Play();
        }
        
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.K))
        {
            phase = Phase.PHASE3;
            HandleMinigamePhase();
        }*/

        //HandleGlobalElectricityUI();
        SumGlobalElectricity();
        CheckGlobalElectricity();
        ShowInstructions();
    }

    #endregion Update

    #region Handle MiniGame Phases

    private void HandleMinigamePhase()
    {
        switch (phase)
        {
            case Phase.PHASE1:
                //Lógica de que si presionas la pieza se inicie la animación de ponerse en su sitio

                //Comprobar si pieza1, pieza2, pieza3, pieza4, pieza5, pieza6 y pieza7 están bien colocada
                    //Pasar a siguiente fase

                break;
            case Phase.PHASE2:
                //Lógica de sol y de porcentaje de luz natural solar según la luz que le da o el momento del día

                //Comprobar si pieza8, pieza9, pieza10 y piez11 están bien colocadas
                    //Pasar a siguiente fase

                break;
            case Phase.PHASE3:
                //Se muestra panel de cableado
                cablePanel.SetActive(true);
                cablePanelFader.Fade();

                //Lógica de cableado con drag and drop de los cables a su lugar correspondiente

                //Comprobar si todos los cables están bien puestos
                    //Mostrar panel de ahorro energético y como las casas empiezan a gastar menos energía
                    //Después mostrar el robot que agradece al jugador y le otorga uno de los sellos

                break;
            default:
                break;
        }
    }

    #endregion Handle MiniGame Phases

    private void HandleGlobalElectricityUI()
    {
        if (phase == Phase.PHASE3)
        {
            globalElectricityText.text = "Global Electricity: " + globalElectricity + " / 5";
        }
        else
        {
            globalElectricityText.text = string.Empty;
        }
    }

    private void CheckGlobalElectricity()
    {
        if (cable1.isPositioned && cable2.isPositioned && cable3.isPositioned && cable4.isPositioned && cable5.isPositioned && cable6.isPositioned)
        {
            Debug.Log("Congratulations");
            PlaySound(5);

            if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                finishPanelEnglish.SetActive(true);
                pauseIndicatorEnglish.SetActive(false);
                PauseMenuManager.canPause = false;
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                finishPanelSpanish.SetActive(true);
                pauseIndicatorSpanish.SetActive(false);
                PauseMenuManager.canPause = false;
            }

            /*if (globalElectricity == 5 && canCheckElectricity)
            {
                
                //congratulationsPhase3Panel.SetActive(true);
                //StartCoroutine(ShowStampPanel());

                canCheckElectricity = false;
                
            }
            else if (globalElectricity != 5 && canCheckElectricity)
            {
                StartCoroutine(slotScript.ShowElectrictyFail());

                

                cable1.ResetPosition();
                cable2.ResetPosition();
                cable3.ResetPosition();
                cable4.ResetPosition();
                cable5.ResetPosition();
                cable6.ResetPosition();
                ResetSlots();
                //canCheckElectricity = false;
            }*/
        }

        if (SlotScript.canReset)
        {
            cable1.ResetPosition();
            cable2.ResetPosition();
            cable3.ResetPosition();
            cable4.ResetPosition();
            cable5.ResetPosition();
            cable6.ResetPosition();
            ResetSlots();
            SlotScript.canReset = false;
        }
        
    }

    public void SumGlobalElectricity()
    {
        int temp = 0;

        for (int i = 0; i < slotCables.Length; i++)
        {
            temp += slotCables[i].GetComponent<SlotScript>().slotIndividualElectricity;
            
        }

        globalElectricity = temp;
    }

    public void PassToPhase2()
    {
        PlaySound(3);

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            tutorial2English.SetActive(true);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            tutorial2Spanish.SetActive(true);
        }

        SolarLight.randomCorroutinCanStart = true;
        solarLightPlace1.StartCoroutine(solarLightPlace1.RandomChangeSunPercent());
        solarLightPlace2.StartCoroutine(solarLightPlace2.RandomChangeSunPercent());
        solarLightPlace3.StartCoroutine(solarLightPlace3.RandomChangeSunPercent());
        solarLightPlace4.StartCoroutine(solarLightPlace4.RandomChangeSunPercent());
        phase = Phase.PHASE2;
    }

    public void PassToPhase3()
    {
        PlaySound(3);

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            tutorial3English.SetActive(true);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            tutorial3Spanish.SetActive(true);
        }

        instructionsPanelText.text = "Fase 3. Conecta los cables por medio de arrastrarlos y soltarlos, de forma que sumen la electricidad necesaria.";
        instructionsPanel.transform.localPosition = new Vector3(438, 452, 0);
        instructionsPanel.transform.localScale = new Vector3(0.64f, 0.64f, 0.64f);
        phase = Phase.PHASE3;
        HandleMinigamePhase();
    }

    private IEnumerator ShowStampPanel()
    {
        yield return new WaitForSeconds(2f);
        //stampPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        MinigamesCompleted.minigame5Finished = true;
        SceneManager.LoadScene("LevelSelector");
    }

    private void ResetSlots()
    {
        for (int i = 0; i < slotCables.Length; i++)
        {
            slotCables[i].GetComponent<SlotScript>().ResetSlot();
        }
    }

    public void ShowTutorial1()
    {
        PlaySound(3);

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            initialInstructionsEnglish.SetActive(false);
            tutorial1English.SetActive(true);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            initialInstructionsSpanish.SetActive(false);
            tutorial1Spanish.SetActive(true);
        }
    }

    public void HideTutorials1()
    {
        PlaySound(3);

        tutorial1English.SetActive(false);
        tutorial1Spanish.SetActive(false);
        MouseLook.canLook = true;
        ObserveObject.cantMove = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenuManager.canPause = true;
        robotIcon.SetActive(true);
        canShowInstructions = true;

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            pauseIndicatorEnglish.SetActive(true);
            instructionsIndicatorEnglish.SetActive(true);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            pauseIndicatorSpanish.SetActive(true);
            instructionsIndicatorSpanish.SetActive(true);
        }
    }

    public void FinishMinigame()
    {
        PlaySound(3);

        PauseMenuManager.canPause = true;
        MinigamesCompleted.minigame5Finished = true;
        SceneManager.LoadScene("LevelSelector");
    }

    public void HideTutorials2()
    {
        PlaySound(3);
        tutorial2English.SetActive(false);
        tutorial2Spanish.SetActive(false);
        robotIcon.SetActive(true);
        canShowInstructions = true;

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            instructionsIndicatorEnglish.SetActive(true);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            instructionsIndicatorSpanish.SetActive(true);
        }
    }

    public void HideTutorials3()
    {
        PlaySound(3);
        tutorial3English.SetActive(false);
        tutorial3Spanish.SetActive(false);
        robotIcon.SetActive(true);
        canShowInstructions = true;

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            instructionsIndicatorEnglish.SetActive(true);
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            instructionsIndicatorSpanish.SetActive(true);
        }
    }

    public void PlaySound(int sound)
    {
        if (audioSource != null)
        {
            audioSource.clip = soundClips[sound];
            audioSource.Play();
        }
        
    }

    private void ShowInstructions()
    {
        

        if (Input.GetKeyDown(KeyCode.E) && canShowInstructions)
        {
            switch (phase)
            {
                case Phase.PHASE1:
                    MouseLook.canLook = false;
                    ObserveObject.cantMove = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    PauseMenuManager.canPause = false;

                    if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                    {
                        initialInstructionsEnglish.SetActive(true);
                    }
                    else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                    {
                        initialInstructionsSpanish.SetActive(true);
                    }

                    break;
                case Phase.PHASE2:
                    if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                    {
                        tutorial2English.SetActive(true);
                    }
                    else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                    {
                        tutorial2Spanish.SetActive(true);
                    }
                    break;
                case Phase.PHASE3:
                    if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                    {
                        tutorial3English.SetActive(true);
                    }
                    else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                    {
                        tutorial3Spanish.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

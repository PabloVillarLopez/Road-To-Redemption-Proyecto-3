using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    #region Camera Variables
    //[Header("Camera Variables")]
    private GameObject playerCamera;
    private GameObject pipelineCamera;
    private bool pipelineCameraActive;
    [HideInInspector]
    public GameObject RotatePipelineUI;

    #endregion Camera Variables

    #region Type of Catastrophes and Catastrophes Variables

    public enum Catastrophes
    {
        NONE,
        WATERLEAK,
        BACTERIA,
        HIGHCONTAMINATION
    }

    [Header("Catastrophes")]
    public Catastrophes catastrophes;
    public float waterContamination;
    public float contaminationSpeed;
    public float decontaminationSpeed;
    public Slider contaminationSlider;
    public float bacteriaContamination;
    public float bacteriaContaminationSpeed;
    public float bacteriaDecontaminationSpeed;
    public float waterLeak;
    public float waterMoreLeakSpeed;
    public float waterLessLeakSpeed;
    public Slider bacteriaSlider;
    public Slider waterLeakSlider;

    public bool isWaterLeakCatastropheStarted;
    public bool isBacteriaCatastropheStarted;
    public bool isContaminationCatastropheStarted;

    public bool catastrophesCanStart;
    private bool catastropheTransitionStarted;

    private IEnumerator contaminationTransitionCoroutine;
    private bool contaminationCoroutineRunning;
    private IEnumerator bacteriaTransitionCoroutine;
    private bool bacteriaCoroutineRunning;

    public Button BacteriaContaminationButton;
    public Button DecontaminationButton;

    public GameObject waterLeakWarningMessage;
    public GameObject contaminationWarningMessage;
    public GameObject bacterianWarningMessage;
    public PanelFader waterLeakWarningPanelFader;
    public PanelFader contaminationWarningPanelFader;
    public PanelFader bacteriaWarningPanelFader;

    [HideInInspector]
    public IEnumerator waterLeakTransitionCoroutine;
    [HideInInspector]
    public bool waterLeakCoroutineRunning;

    #endregion Type of Catastrophes and Catastrophes Variables

    #region Minigame Phases

    public enum Phases
    {
        TREATMENTPLANT,
        TOWN
    }

    [Header("Minigame Phases")]
    public Phases phases;

    public int decontaminationCount;
    public int bacteriaCleanedCount;
    public int waterLeakedSolvedCount;

    #endregion Minigame Phases

    #region Resources

    [Header("Resources")]
    public int money = 1000;
    public TextMeshProUGUI moneyText;
    public int points;
    public bool pointsCanIncrease;
    public bool pointsCanDecrease;

    #endregion Resources

    #region Verify Pipelines Correct Position Variables

    [Header("Verify Pipelines Correct Position")]
    public GameObject[] pipelinesToRotate;
    //public GameObject[] pipelinesToRotateAndSelect;
    public GameObject[] pipelinesToDecontaminate;
    public GameObject[] pipelinesToChangePipeline1;
    public GameObject[] pipelinesToChangePipeline3;
    public GameObject[] pipelinesToChangePipeline5;
    public GameObject[] pipelinesIconsPipeline1;
    public GameObject[] pipelinesIconsPipeline3;
    public GameObject[] pipelinesIconsPipeline5;
    public int pipelinesInCorrectPlace;

    #endregion Verify Pipelines Correct Position Variables

    #region Select Pipelines Variables

    [Header("Select Pipelines")]
    public int selectedPipeline = 0;
    public int selectedPipelineIcon = 0;
    public int previousPipeline;

    #endregion Select Pipelines Variables

    #region Difficulty Levels Variables

    public enum DifficultyLevel
    {
        EASY,
        INTERMEDIATE,
        HARD
    }

    [Header("Difficulty Level")]
    public DifficultyLevel difficultyLevel;

    #endregion Difficulty Levels Variables

    #region Pipeline Reference

    public PipelineForeground pipelineActive;

    #endregion Pipeline Reference

    #region UI References

    [Header("UI References")]
    [Space]
    public GameObject RotateUI;
    public GameObject SelectUI;
    public GameObject waterOpenAndCloseUI;
    public GameObject decontaminationUI;
    public GameObject catastrophesPanel;

    #endregion UI References

    #region Rotate UI Buttons Variables
    [Header("Rotate Buttons")]
    [Space]
    public Button leftRotateButton;
    public Button rightRotateButton;
    public Button upRotateButton;
    public Button downRotateButton;
    public static bool canAddRotateToButton = true;

    #endregion Rotate UI Buttons Variables

    #region Click to decontaminate Variable

    public int clickToDecontaminateCount;
    public int clickToDecontaminateBacteriaCount;

    #endregion Click to decontaminate Variable

    #region Number of Movements Variables

    //[Header("Movements")]
    [HideInInspector]
    public int maxNumOfMovements;
    [HideInInspector]
    public int currentNumOfMovements;
    [HideInInspector]
    public int remainingMovements;
    private TextMeshProUGUI numOfMovementsText;

    #endregion Number of Movements Variables

    #region Text Variables for changing Language

    [Header("Texts for changing Language")]
    public TextMeshProUGUI waterLeakText;
    public TextMeshProUGUI contaminationText;
    public TextMeshProUGUI bacteriaText;
    public TextMeshProUGUI rotatePipelineText;
    //public TextMeshProUGUI moneyText;
    public TextMeshProUGUI selectPipelineTypeText;
    public TextMeshProUGUI bacteriaBlinkingWarningTitleText;
    public TextMeshProUGUI bacteriaBlinkingWarningSubtitleText;
    public TextMeshProUGUI bacteriaBlinkingWarningDescriptionText;
    public TextMeshProUGUI contaminationBlinkingWarningTitleText;
    public TextMeshProUGUI contaminationBlinkingWarningSubtitleText;
    public TextMeshProUGUI contaminationBlinkingWarningDescriptionText;
    public TextMeshProUGUI waterLeakBlinkingWarningTitleText;
    public TextMeshProUGUI waterLeakBlinkingWarningSubtitleText;
    public TextMeshProUGUI waterLeakBlinkingWarningDescriptionText;
    public TextMeshProUGUI instructionsText;
    public GameObject instructionsPanel;

    #endregion Text Variables for changing Language

    public static bool canInteractWithRotatePipelines;
    public IEnumerator blinkingWaterLeakCorroutine;
    public IEnumerator blinkingContaminationCorroutine;
    public IEnumerator blinkingContaminationBacteriaCorroutine;

    [Header("Congratulations Panel")]
    public GameObject congratulationsPanel;
    public TextMeshProUGUI congratulationsPanelText;

    [Header("Mistake Panel")]
    public GameObject mistakePanel;
    public TextMeshProUGUI mistakePanelText;

    [Header("Stamp Panel")]
    public GameObject stampPanel;

    [Header("Select buttons")]
    public GameObject selectUp1Button;
    public GameObject selectDonw1Button;
    public GameObject selectUp2Button;
    public GameObject selectDown2Button;
    public GameObject selectUp3Button;
    public GameObject selectDown3Button;
    public GameObject pipelineSelector1;
    public GameObject pipelineSelector2;
    public GameObject pipelineSelector3;

    [Header("Arrows")]
    public GameObject arrowContamination1;
    public GameObject arrowContamination2;
    public GameObject arrowContamination3;
    public GameObject arrowRedirection1;
    public GameObject arrowRedirection2;
    public GameObject arrowRedirection3;
    public GameObject arrowRedirection4;
    public GameObject arrowRedirection5;
    public GameObject arrowRedirection6;
    public GameObject arrowRedirection7;
    private bool canShowArrowsContamination = true;
    private bool canShowArrowsRedirection = false;
    private float arrowsContaminationCont = 0;
    private float arrowsRedirectionCont = 0;

    [Header("Interact Indicator")]
    public GameObject interactIndicator;

    #region Awake

    private void Awake()
    {
        playerCamera = GameObject.Find("Charapter");
        pipelineCamera = GameObject.Find("PipelineCamera");
        RotatePipelineUI = GameObject.Find("RotatePipelineUI");
        numOfMovementsText = GameObject.Find("RemainingMovementsText").GetComponent<TextMeshProUGUI>();
    }

    #endregion Awake

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        playerCamera.SetActive(true);
        pipelineCamera.SetActive(false);
        RotatePipelineUI.SetActive(false);
        bacterianWarningMessage.SetActive(false);
        contaminationWarningMessage.SetActive(false);
        waterLeakWarningMessage.SetActive(false);
        congratulationsPanel.SetActive(false);
        mistakePanel.SetActive(false);
        stampPanel.SetActive(false);

        maxNumOfMovements = 5;

        //SelectPipeline();

        contaminationTransitionCoroutine = ContaminationTransition();
        bacteriaTransitionCoroutine = BacteriaContaminationTransition();
        waterLeakTransitionCoroutine = WaterLeakMoreTransition();
        blinkingWaterLeakCorroutine = BlinkingWaterLeakWarning();

        arrowContamination1.SetActive(false);
        arrowContamination2.SetActive(false);
        arrowContamination3.SetActive(false);
        arrowRedirection1.SetActive(false);
        arrowRedirection2.SetActive(false);
        arrowRedirection3.SetActive(false);
        arrowRedirection4.SetActive(false);
        arrowRedirection5.SetActive(false);
        arrowRedirection6.SetActive(false);
        arrowRedirection7.SetActive(false);

    //StartCoroutine(ContaminationTransition());
}

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        //ComeBackToPlayerCamera();
        HandleMinigamePhases();
        ManageUIPipelineType();

        if (Input.GetKeyDown(KeyCode.E) && PlayerController.playerEnteredInRotatePipelineArea && canInteractWithRotatePipelines && !PlayerController.pipelineEnteredHasBeenAlreadyRotated)
        {
            PipelineRotateCamera();
        }

        if (Input.GetKeyDown(KeyCode.E) && PlayerController.playerEnteredInDecontaminatePipelineArea && !PlayerController.pipelineEnteredHasBeenAlreadyDecontaminated)
        {
            PipelineDecontaminateCamera();
        }

        //HandleRemaingingMovements();

        if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            moneyText.text = "Dinero: " + money;
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            moneyText.text = "Money: " + money;
        }
        

        //CheckSelectedPipeline();


        contaminationSlider.value = waterContamination;
        bacteriaSlider.value = bacteriaContamination;
        waterLeakSlider.value = waterLeak;

        if (Input.GetKeyDown(KeyCode.C))
        {
            LanguageManager.currentLanguage = LanguageManager.Language.Spanish;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            LanguageManager.currentLanguage = LanguageManager.Language.English;
        }

        HandleUIDependingOnLanguage();
    }

    #endregion Update

    //To select different types of pipelines
    #region Select Pipeline Types

    /*void SelectPipeline()
    {
        int i = 0;

        foreach (GameObject pipeline in pipelinesToRotateAndSelect)
        {
            if (i == selectedPipeline)
            {
                //pipeline.SetActive(true);
                pipelinesIcons[i].gameObject.SetActive(true);
                //pipeline.gameObject.transform.position = pipelines[0].transform.position;
                pipelineActive = pipeline.GetComponent<PipelineForeground>();

                switch (pipelineActive.pipelineId)
                {

                    case 1:
                        pipelineCamera.transform.position = pipelines[i].transform.position + new Vector3(0, 0, 2);
                        pipelineCamera.transform.eulerAngles = new Vector3(0, 180, 0);
                        break;
                    case 2:
                        pipelineCamera.transform.position = pipelines[i].transform.position + new Vector3(3, 1, 0);
                        pipelineCamera.transform.eulerAngles = new Vector3(0, -90, 25);
                        break;
                    case 3:
                        pipelineCamera.transform.position = pipelines[i].transform.position + new Vector3(1, 0, 0);
                        break;
                    case 4:
                        pipelineCamera.transform.position = pipelines[i].transform.position + new Vector3(0, 0, 5);
                        break;
                    case 5:
                        pipelineCamera.transform.position = pipelines[i].transform.position + new Vector3(0, 0, 5);
                        break;
                    case 6:
                        pipelineCamera.transform.position = pipelines[i].transform.position + new Vector3(0, 0, 5);
                        break;
                    case 7:
                        pipelineCamera.transform.position = pipelines[i].transform.position + new Vector3(0, 0, 5);
                        break;
                    case 0:
                        break;
                    default:
                        break;
                }

                
                

                ManageUIPipelineType();
            }
            else
            {
                //pipeline.SetActive(false);
                pipelinesIcons[i].gameObject.SetActive(false);
            }
            i++;
        }

    }

    public void SelectPipelineLeft()
    {
        if (money >= 100 && money >= 0)
        {
            if (selectedPipeline <= 0)
            {
                selectedPipeline = pipelines.Length - 1;
                selectedPipelineIcon = pipelinesIcons.Length - 1;
            }
            else
            {
                selectedPipeline--;
                selectedPipelineIcon--;
            }

            if (previousPipeline != selectedPipeline)
            {
                SelectPipeline();
                money -= 100;
            }
        }
        
    }

    public void SelectPipelineRight()
    {
        if (money >= 100 && money >= 0)
        {
            if (selectedPipeline >= pipelines.Length - 1)
            {
                selectedPipeline = 0;
                selectedPipelineIcon = 0;
            }
            else
            {
                selectedPipeline++;
                selectedPipelineIcon++;
            }

            if (previousPipeline != selectedPipeline)
            {
                SelectPipeline();
                money -= 100;
            }
        }
        
    }

    private void CheckSelectedPipeline()
    {
        previousPipeline = selectedPipeline;

        if (previousPipeline != selectedPipeline)
        {
            SelectPipeline();
        }
    }*/

    #endregion Select Pipeline Types

    //For the transition from no contamination to totally contaminated and inverse
    #region Contamination Transition

    public IEnumerator ContaminationTransition()
    {
        float elapsedTime = 0;
        contaminationCoroutineRunning = true;

        while (elapsedTime < contaminationSpeed && contaminationCoroutineRunning)
        {
            elapsedTime += Time.deltaTime;

            waterContamination = Mathf.Lerp(0f, 1f, elapsedTime / contaminationSpeed);
            yield return null;
        }

        if (waterContamination >= 1f)
        {
            StopAllCoroutines();
            RotatePipelineUI.SetActive(false);
            instructionsPanel.SetActive(false);
            mistakePanel.SetActive(true);
        }

        //StartCoroutine(ContaminationTransition());
    }

    public IEnumerator DecontaminationTransition()
    {
        float elapsedTime2 = 0;

        while (elapsedTime2 < contaminationSpeed && waterContamination >= 0)
        {
            elapsedTime2 += Time.deltaTime;
            float previousWaterContamination = waterContamination;

            waterContamination = Mathf.Lerp(previousWaterContamination, 0f, elapsedTime2 / decontaminationSpeed);
            
            yield return null;
        }

        //decontaminationCount++;
        //StartCoroutine(ContaminationTransition());
    }

    #endregion Contamination Transition

    //For the transition from no bacteria to bacteria contamination and inverse
    #region Bacteria Transition

    public IEnumerator BacteriaContaminationTransition()
    {
        float elapsedTime3 = 0;
        bacteriaCoroutineRunning = true;

        while (elapsedTime3 < bacteriaContaminationSpeed && bacteriaCoroutineRunning)
        {
            elapsedTime3 += Time.deltaTime;

            bacteriaContamination = Mathf.Lerp(0f, 1f, elapsedTime3 / bacteriaContaminationSpeed);
            yield return null;
        }

        if (bacteriaContamination >= 1f)
        {
            StopAllCoroutines();
            RotatePipelineUI.SetActive(false);
            instructionsPanel.SetActive(false);
            mistakePanel.SetActive(true);
        }

        //StartCoroutine(ContaminationTransition());
    }

    public IEnumerator BacteriaDecontaminationTransition()
    {
        float elapsedTime4 = 0;

        while (elapsedTime4 < bacteriaDecontaminationSpeed)
        {
            elapsedTime4 += Time.deltaTime;
            float previousBacteriaContamination = bacteriaContamination;

            bacteriaContamination = Mathf.Lerp(previousBacteriaContamination, 0f, elapsedTime4 / bacteriaDecontaminationSpeed);
            
            yield return null;
        }

        //bacteriaCleanedCount++;
        //StartCoroutine(ContaminationTransition());
    }

    #endregion Bacteria Transition

    //For the transition from no water leak to water leak and inverse
    #region WaterLeak Transition

    public IEnumerator WaterLeakMoreTransition()
    {
        float elapsedTime5 = 0;
        waterLeakCoroutineRunning = true;

        while (elapsedTime5 < waterMoreLeakSpeed && waterLeakCoroutineRunning)
        {
            elapsedTime5 += Time.deltaTime;

            waterLeak = Mathf.Lerp(0f, 1f, elapsedTime5 / waterMoreLeakSpeed);
            yield return null;
        }

        if (waterLeak >= 1f)
        {
            StopAllCoroutines();
            RotatePipelineUI.SetActive(false);
            instructionsPanel.SetActive(false);
            mistakePanel.SetActive(true);
        }

        //StartCoroutine(ContaminationTransition());
    }

    public IEnumerator WaterLeakLessTransition()
    {
        float elapsedTime6 = 0;

        while (elapsedTime6 < contaminationSpeed && waterContamination >= 0)
        {
            elapsedTime6 += Time.deltaTime;
            float previousWaterLeak = waterLeak;

            waterLeak = Mathf.Lerp(previousWaterLeak, 0f, elapsedTime6 / waterLessLeakSpeed);
            waterLeakedSolvedCount = 1;
            yield return null;
        }

        //StartCoroutine(ContaminationTransition());
    }

    #endregion WaterLeak Transition

    //Adjustments for Difficulty Levels
    #region Difficulty Levels

    public void EasyLevel()
    {
        maxNumOfMovements = 10;
        difficultyLevel = DifficultyLevel.EASY;
    }

    public void IntermediateLevel()
    {
        maxNumOfMovements = 7;
        difficultyLevel = DifficultyLevel.INTERMEDIATE;
    }

    public void HardLevel()
    {
        maxNumOfMovements = 5;
        difficultyLevel = DifficultyLevel.HARD;
    }

    #region Manage Points According to the DifficultyLevel

    public void ManagePointsDifficulty()
    {
        switch (difficultyLevel)
        {
            case DifficultyLevel.EASY:
                pointsCanIncrease = true;
                break;
            case DifficultyLevel.INTERMEDIATE:
                pointsCanIncrease = true;
                break;
            case DifficultyLevel.HARD:
                pointsCanDecrease = true;
                break;
            default:
                break;
        }
    }

    #endregion Manage Points According to the DifficultyLevel

    #endregion Difficulty Levels

    //To hide and show UI depending on the pipeline type
    #region Manage UI Depending On Pipeline Type

    private void ManageUIPipelineType()
    {
        if (PlayerController.pipelineEntered != null)
        {
            pipelineActive = PlayerController.pipelineEntered.GetComponent<PipelineForeground>();
        }
        
        if (pipelineActive != null)
        {
            switch (pipelineActive.pipelineType)
            {
                case PipelineForeground.PipelineType.DECONTAMINATING:
                    if (!pipelineActive.alreadyDecontaminated)
                    {
                        RotateUI.SetActive(false);
                        waterOpenAndCloseUI.SetActive(false);
                        SelectUI.SetActive(false);
                        decontaminationUI.SetActive(true);
                        catastrophesPanel.SetActive(true);
                        catastrophes = Catastrophes.HIGHCONTAMINATION;
                        HandleCatastrophes();
                        //SelectUI.transform.position = new Vector3(SelectUI.transform.position.x - 5, SelectUI.transform.position.y, SelectUI.transform.position.z);
                    }

                    break;
                case PipelineForeground.PipelineType.CONTROLFLOWOFWATER:
                    if (!pipelineActive.alreadyDecontaminated)
                    {
                        RotateUI.SetActive(false);
                        waterOpenAndCloseUI.SetActive(true);
                        decontaminationUI.SetActive(false);
                        SelectUI.SetActive(false);
                        catastrophesPanel.SetActive(true);
                        catastrophes = Catastrophes.WATERLEAK;
                        HandleCatastrophes();
                        //waterOpenAndCloseUI.transform.position = new Vector3(waterOpenAndCloseUI.transform.position.x - 5, SelectUI.transform.position.y, SelectUI.transform.position.z);
                    }


                    break;
                case PipelineForeground.PipelineType.DECONTAMINATINGBACTERIA:
                    if (!pipelineActive.alreadyDecontaminated)
                    {
                        RotateUI.SetActive(false);
                        waterOpenAndCloseUI.SetActive(false);
                        decontaminationUI.SetActive(true);
                        SelectUI.SetActive(false);
                        catastrophesPanel.SetActive(true);
                        catastrophes = Catastrophes.BACTERIA;
                        HandleCatastrophes();
                    }
                    
                    

                    break;
                case PipelineForeground.PipelineType.REDIRECTION:
                    RotateUI.SetActive(true);
                    waterOpenAndCloseUI.SetActive(false);
                    decontaminationUI.SetActive(false);
                    SelectUI.SetActive(false);
                    catastrophesPanel.SetActive(false);

                    catastrophes = Catastrophes.NONE;
                    HandleCatastrophes();

                    if (canAddRotateToButton)
                    {
                        leftRotateButton.onClick.RemoveAllListeners();
                        rightRotateButton.onClick.RemoveAllListeners();
                        upRotateButton.onClick.RemoveAllListeners();
                        downRotateButton.onClick.RemoveAllListeners();

                        leftRotateButton.onClick.AddListener(pipelineActive.RotateLeftX);
                        rightRotateButton.onClick.AddListener(pipelineActive.RotateRightX);
                        upRotateButton.onClick.AddListener(pipelineActive.RotateUpwards);
                        downRotateButton.onClick.AddListener(pipelineActive.RotateDownwards);
                        canAddRotateToButton = false;
                    }


                    //SelectUI.SetActive(false);
                    //RotateUI.transform.position = new Vector3(RotateUI.transform.position.x + 160, RotateUI.transform.position.y, RotateUI.transform.position.z);
                    break;
                case PipelineForeground.PipelineType.REDIRECTIONANDSELECT:
                    RotateUI.SetActive(true);
                    waterOpenAndCloseUI.SetActive(false);
                    decontaminationUI.SetActive(false);
                    SelectUI.SetActive(true);
                    catastrophesPanel.SetActive(false);
                    catastrophes = Catastrophes.NONE;
                    HandleCatastrophes();

                    if (canAddRotateToButton)
                    {
                        leftRotateButton.onClick.RemoveAllListeners();
                        rightRotateButton.onClick.RemoveAllListeners();
                        upRotateButton.onClick.RemoveAllListeners();
                        downRotateButton.onClick.RemoveAllListeners();

                        leftRotateButton.onClick.AddListener(pipelineActive.RotateLeftX);
                        rightRotateButton.onClick.AddListener(pipelineActive.RotateRightX);
                        upRotateButton.onClick.AddListener(pipelineActive.RotateUpwards);
                        downRotateButton.onClick.AddListener(pipelineActive.RotateDownwards);
                        canAddRotateToButton = false;
                    }
                    break;
                case PipelineForeground.PipelineType.DECONTAMINATINGANDSELECT:
                    RotateUI.SetActive(false);
                    waterOpenAndCloseUI.SetActive(false);
                    SelectUI.SetActive(true);
                    decontaminationUI.SetActive(true);
                    catastrophesPanel.SetActive(true);

                    break;
                case PipelineForeground.PipelineType.CONTROLFLOWOFWATERANDSELECT:
                    RotateUI.SetActive(false);
                    waterOpenAndCloseUI.SetActive(true);
                    decontaminationUI.SetActive(false);
                    SelectUI.SetActive(true);
                    catastrophesPanel.SetActive(true);

                    break;
                default:
                    break;
            }
        }
        
    }

    #endregion Manage UI Depending on Pipeline Type

    //To decontaminate the water
    #region Click Button 5 times to decontaminate

    public void ClickToDecontaminate()
    {
        clickToDecontaminateCount++;

        if (clickToDecontaminateCount >= 5)
        {
            contaminationCoroutineRunning = false;
            //StopCoroutine(contaminationTransitionCoroutine);
            //StopCoroutine(BlinkingContaminationWarning());
            StopAllCoroutines();
            
            StartCoroutine(DecontaminationTransition());
            StopCoroutine(blinkingContaminationCorroutine);
            contaminationWarningPanelFader.Fade();
            contaminationWarningPanelFader.canvGroup.gameObject.SetActive(false);
            congratulationsPanel.SetActive(true);
            //StartCoroutine(Wait());
            DecontaminationButton.gameObject.SetActive(false);
            congratulationsPanelText.text = "Congratulations on decontamining the water.";
            //PlayerController.pipelineEnteredHasBeenAlreadyDecontaminated = true;
            PlayerController.pipelineEntered.GetComponent<PipelineForeground>().alreadyDecontaminated = true;
            PlayerController.pipelineEntered.GetComponent<PipelineForeground>().arrowAsociated.SetActive(false);

            clickToDecontaminateCount = 0;

            decontaminationCount = 1;
        }
    }

    public void ClickToCleanBacteria()
    {
        clickToDecontaminateBacteriaCount++;

        if (clickToDecontaminateBacteriaCount >= 5)
        {
            bacteriaCoroutineRunning = false;
            //StopCoroutine(bacteriaTransitionCoroutine);
            //StopCoroutine(BlinkingBacteriaWarning());
            StopAllCoroutines();
            
            StartCoroutine(BacteriaDecontaminationTransition());
            StopCoroutine(blinkingContaminationBacteriaCorroutine);
            bacteriaWarningPanelFader.Fade();
            bacteriaWarningPanelFader.canvGroup.gameObject.SetActive(false);
            congratulationsPanel.SetActive(true);
            BacteriaContaminationButton.gameObject.SetActive(false);
            //StartCoroutine(Wait());
            congratulationsPanelText.text = "Congratulations on decontamining the water of bacteria.";
            PlayerController.pipelineEntered.GetComponent<PipelineForeground>().alreadyDecontaminated = true;
            PlayerController.pipelineEntered.GetComponent<PipelineForeground>().arrowAsociated.SetActive(false);
            //PlayerController.pipelineEnteredHasBeenAlreadyDecontaminated = true;

            clickToDecontaminateBacteriaCount = 0;

            bacteriaCleanedCount = 1;
        }
    }

    #endregion Click Button 5 times to decontaminate

    //To come back to the player camera
    #region Player Camera 

    private void ComeBackToPlayerCamera()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pipelineCameraActive)
        {
            playerCamera.SetActive(true);
            pipelineCamera.SetActive(false);
            pipelineCameraActive = false;
            RotatePipelineUI.SetActive(false);
            catastrophesCanStart = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    public void ComeBackToPlayerCameraAfterEvent()
    {
        playerCamera.SetActive(true);
        pipelineCamera.SetActive(false);
        pipelineCameraActive = false;
        RotatePipelineUI.SetActive(false);
        catastrophesCanStart = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    #endregion Player Camera

    //To place in the foreground the pipeline by activating the pipelinecamera and deactivating the player camera
    #region Pipeline Camera

    private void PipelineRotateCamera()
    {
        interactIndicator.SetActive(false);
        playerCamera.SetActive(false);

        switch (PlayerController.pipelineRotateEnteredID)
        {
            case 0:
                pipelineCamera.transform.position = pipelinesToRotate[PlayerController.pipelineRotateEnteredID].transform.position + new Vector3(0, 0, 4); //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
                pipelineCamera.transform.eulerAngles = new Vector3(0, -180, 0);

                selectDonw1Button.SetActive(true);
                selectUp1Button.SetActive(true);
                selectDown2Button.SetActive(false);
                selectUp2Button.SetActive(false);
                selectDown3Button.SetActive(false);
                selectUp3Button.SetActive(false);

                pipelineSelector1.SetActive(true);
                pipelineSelector1.GetComponent<PipelineSelector>().SelectPipeline();
                pipelineSelector2.GetComponent<PipelineSelector>().DeactivatePipelines();
                pipelineSelector3.GetComponent<PipelineSelector>().DeactivatePipelines();
                //pipelineSelector2.SetActive(false);
                //pipelineSelector3.SetActive(false);
                break;
            case 1:
                pipelineCamera.transform.position = pipelinesToRotate[PlayerController.pipelineRotateEnteredID].transform.position + new Vector3(4, 0, 0); //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
                pipelineCamera.transform.eulerAngles = new Vector3(0, -90, 0);

                selectDonw1Button.SetActive(false);
                selectUp1Button.SetActive(false);
                selectDown2Button.SetActive(false);
                selectUp2Button.SetActive(false);
                selectDown3Button.SetActive(false);
                selectUp3Button.SetActive(false);
                break;
            case 2:
                pipelineCamera.transform.position = pipelinesToRotate[PlayerController.pipelineRotateEnteredID].transform.position + new Vector3(4, 0, 1); //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
                pipelineCamera.transform.eulerAngles = new Vector3(0, -90, 0);

                selectDonw1Button.SetActive(false);
                selectUp1Button.SetActive(false);
                selectDown2Button.SetActive(true);
                selectUp2Button.SetActive(true);
                selectDown3Button.SetActive(false);
                selectUp3Button.SetActive(false);

                //pipelineSelector1.SetActive(false);
                pipelineSelector2.SetActive(true);
                pipelineSelector2.GetComponent<PipelineSelector>().SelectPipeline();
                pipelineSelector1.GetComponent<PipelineSelector>().DeactivatePipelines();
                pipelineSelector3.GetComponent<PipelineSelector>().DeactivatePipelines();
                //pipelineSelector3.SetActive(false);
                break;
            case 3:
                pipelineCamera.transform.position = pipelinesToRotate[PlayerController.pipelineRotateEnteredID].transform.position + new Vector3(4, 0, -1); //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
                pipelineCamera.transform.eulerAngles = new Vector3(0, -90, 0);

                selectDonw1Button.SetActive(false);
                selectUp1Button.SetActive(false);
                selectDown2Button.SetActive(false);
                selectUp2Button.SetActive(false);
                selectDown3Button.SetActive(false);
                selectUp3Button.SetActive(false);
                break;
            case 4:
                pipelineCamera.transform.position = pipelinesToRotate[PlayerController.pipelineRotateEnteredID].transform.position + new Vector3(4, 0, -1); //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
                pipelineCamera.transform.eulerAngles = new Vector3(0, -90, 0);

                selectDown2Button.SetActive(false);
                selectUp2Button.SetActive(false);
                selectDown2Button.SetActive(false);
                selectUp2Button.SetActive(false);
                selectDown2Button.SetActive(true);
                selectUp2Button.SetActive(true);

                //pipelineSelector1.SetActive(false);
                //pipelineSelector2.SetActive(false);
                pipelineSelector1.GetComponent<PipelineSelector>().DeactivatePipelines();
                pipelineSelector2.GetComponent<PipelineSelector>().DeactivatePipelines();

                pipelineSelector3.SetActive(true);
                pipelineSelector3.GetComponent<PipelineSelector>().SelectPipeline();
                break;
            case 5:
                pipelineCamera.transform.position = pipelinesToRotate[PlayerController.pipelineRotateEnteredID].transform.position + new Vector3(4, 0, 0); //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
                pipelineCamera.transform.eulerAngles = new Vector3(0, -90, 0);

                selectDonw1Button.SetActive(false);
                selectUp1Button.SetActive(false);
                selectDown2Button.SetActive(false);
                selectUp2Button.SetActive(false);
                selectDown3Button.SetActive(false);
                selectUp3Button.SetActive(false);
                break;
            case 6:
                pipelineCamera.transform.position = pipelinesToRotate[PlayerController.pipelineRotateEnteredID].transform.position + new Vector3(4, 0, 0); //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
                pipelineCamera.transform.eulerAngles = new Vector3(0, -90, 0);

                selectDonw1Button.SetActive(false);
                selectUp1Button.SetActive(false);
                selectDown2Button.SetActive(false);
                selectUp2Button.SetActive(false);
                selectDown3Button.SetActive(false);
                selectUp3Button.SetActive(false);
                break;
            default:
                break;
        }
        
        
        pipelineCamera.SetActive(true);
        pipelineCameraActive = true;
        RotatePipelineUI.SetActive(true);
        leftRotateButton.gameObject.SetActive(true);
        rightRotateButton.gameObject.SetActive(true);
        upRotateButton.gameObject.SetActive(true);
        downRotateButton.gameObject.SetActive(true);
        numOfMovementsText.gameObject.SetActive(false);

        ManageUIPipelineType();

        if (PlayerController.pipelineEnteredID > 1)
        {
            catastrophesCanStart = true;
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void PipelineDecontaminateCamera()
    {
        interactIndicator.SetActive(false);
        playerCamera.SetActive(false);

        if (PlayerController.pipelineEnteredID == 2)
        {
            pipelineCamera.transform.position = pipelinesToDecontaminate[0].transform.position - new Vector3(0, 0, 2) ; //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
            pipelineCamera.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (PlayerController.pipelineEnteredID == 3)
        {
            pipelineCamera.transform.position = pipelinesToDecontaminate[2].transform.position - new Vector3(0, 0, 2); //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
            pipelineCamera.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (PlayerController.pipelineEnteredID == 4)
        {
            pipelineCamera.transform.position = pipelinesToDecontaminate[1].transform.position + new Vector3(2, 0, -1); //+ new Vector3(0, 0, 2); //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
            pipelineCamera.transform.eulerAngles = new Vector3(0, -90, 0);
        }


        
        pipelineCamera.SetActive(true);
        pipelineCameraActive = true;
        RotatePipelineUI.SetActive(true);
        if (PlayerController.pipelineEnteredID > 1)
        {
            catastrophesCanStart = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /*private void PipelineDecontaminateCamera()
    {
        playerCamera.SetActive(false);
        //pipelineCamera.transform.position = pipelines[PlayerController.pipelineEnteredID - 1].transform.position + new Vector3(0, 0, 2); //pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
        pipelineCamera.SetActive(true);
        pipelineCameraActive = true;
        RotatePipelineUI.SetActive(true);
        if (PlayerController.pipelineEnteredID > 1)
        {
            catastrophesCanStart = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }*/

    #endregion Pipeline Camera

    //To handle the remainging movements text
    #region Handle Remaining Movements Text

    private void HandleRemaingingMovements()
    {
        remainingMovements = (maxNumOfMovements - currentNumOfMovements);

        if (RotatePipelineUI.activeInHierarchy)
        {
            if (remainingMovements > 1 && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                numOfMovementsText.text = "Movimientos restantes: " + remainingMovements;
            }
            else if (remainingMovements > 1 && LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                numOfMovementsText.text = "Remaining Movements: " + remainingMovements;
            }
            else if (remainingMovements == 1 && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                numOfMovementsText.text = "Movimientos restantes: " + remainingMovements;
            }
            else if (remainingMovements == 1 && LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                numOfMovementsText.text = "Remaining Movement: " + remainingMovements;
            }
            else if (remainingMovements <= 0 && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                numOfMovementsText.text = "Movimientos restantes: " + remainingMovements + " . Sin movimientos";
            }
            else if (remainingMovements <= 0 && LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                numOfMovementsText.text = "Remaining Movements: " + remainingMovements + " . No Remaining Movements";
            }
        }



    }

    #endregion Handle Remainging Movements Text

    //To handle the minigame phases
    #region Handle MiniGame Phases

    public void HandleMinigamePhases()
    {
        switch (phases)
        {
            case Phases.TREATMENTPLANT:

                /*for (int i = 0; i < pipelines.Length; i++)
                {
                    if (pipelines[i].GetComponent<PipelineForeground>().pipelineInCorrecRotation)
                    {
                        if (pipelinesInCorrectPlace >= 0 && pipelinesInCorrectPlace <= 3)
                        {
                            pipelinesInCorrectPlace++;
                        }
                        
                    }
                }*/

                canInteractWithRotatePipelines = false;

                if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                {
                    instructionsText.text = "Descontamina las tuberías rojas. Tuberías descontaminadas: " + (decontaminationCount + bacteriaCleanedCount + waterLeakedSolvedCount) + " / 3";
                }
                else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                {
                    instructionsText.text = "Decontaminate the red tubes. Decontaminated tubes: " + (decontaminationCount + bacteriaCleanedCount + waterLeakedSolvedCount) + " / 3";
                }
                else
                {
                    instructionsText.text = "Descontamina las tuberías rojas. Tuberías descontaminadas: " + (decontaminationCount + bacteriaCleanedCount + waterLeakedSolvedCount) + " / 3";
                }

                arrowsContaminationCont += Time.deltaTime;
                if (canShowArrowsContamination && arrowsContaminationCont >= 15f)
                {
                    arrowContamination1.SetActive(true);
                    arrowContamination2.SetActive(true);
                    arrowContamination3.SetActive(true);
                    canShowArrowsContamination = false;
                }

                if (decontaminationCount == 1 && bacteriaCleanedCount == 1 && waterLeakedSolvedCount == 1)
                {
                    phases = Phases.TOWN;
                    canShowArrowsRedirection = true;
                }

                break;
            case Phases.TOWN:
                arrowContamination1.SetActive(false);
                arrowContamination2.SetActive(false);
                arrowContamination3.SetActive(false);

                canInteractWithRotatePipelines = true;

                if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                {
                    instructionsText.text = "Rota las tuberías que están mal colocadas. Tuberías bien colocadas: " + (pipelinesInCorrectPlace) + " / 7";
                }
                else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                {
                    instructionsText.text = "Rotate the tubes that are in wrong position. Rotated tubes: " + (pipelinesInCorrectPlace) + " / 7";
                }
                else
                {
                    instructionsText.text = "Rota las tuberías que están mal colocadas. Tuberías bien colocadas: " + (pipelinesInCorrectPlace) + " / 7";
                }

                arrowsRedirectionCont += Time.deltaTime;
                if (canShowArrowsRedirection && arrowsRedirectionCont >= 15f)
                {
                    arrowRedirection1.SetActive(true);
                    arrowRedirection2.SetActive(true);
                    arrowRedirection3.SetActive(true);
                    arrowRedirection4.SetActive(true);
                    arrowRedirection5.SetActive(true);
                    arrowRedirection6.SetActive(true);
                    arrowRedirection7.SetActive(true);
                    canShowArrowsRedirection = false;
                }

                if (pipelinesInCorrectPlace >= 7)
                {
                    Debug.Log("Minigame Finished");

                    StartCoroutine(WaitAndFinish());
                }

                break;
            default:
                break;
        }
    }

    #endregion Handle MiniGame Phases

    //To handle different catastrophes
    #region Handle Catastrophes

    private void HandleCatastrophes()
    {
        switch (catastrophes)
        {
            case Catastrophes.NONE:
                BacteriaContaminationButton.gameObject.SetActive(false);
                break;

            case Catastrophes.WATERLEAK:
                BacteriaContaminationButton.gameObject.SetActive(false);

                if (!isWaterLeakCatastropheStarted && catastrophesCanStart)
                {
                    StartCoroutine(WaterLeakMoreTransition());
                    StartCoroutine(BlinkingWaterLeakWarning());

                    isWaterLeakCatastropheStarted = true;
                }

                if (!catastrophesCanStart)
                {
                    StopCoroutine(WaterLeakMoreTransition());
                    StopCoroutine(BlinkingWaterLeakWarning());

                    isWaterLeakCatastropheStarted = false;
                }

                break;
            case Catastrophes.BACTERIA:
                BacteriaContaminationButton.gameObject.SetActive(true);
                DecontaminationButton.gameObject.SetActive(false);

                if (!isBacteriaCatastropheStarted && catastrophesCanStart)
                {
                    StartCoroutine(BacteriaContaminationTransition());
                    StartCoroutine(BlinkingBacteriaWarning());

                    isBacteriaCatastropheStarted = true;
                }

                if (!catastrophesCanStart)
                {
                    StopCoroutine(BacteriaDecontaminationTransition());
                    StopCoroutine(BlinkingBacteriaWarning());

                    isBacteriaCatastropheStarted = false;
                }
                break;
            case Catastrophes.HIGHCONTAMINATION:
                BacteriaContaminationButton.gameObject.SetActive(false);
                DecontaminationButton.gameObject.SetActive(true);

                if (!isContaminationCatastropheStarted && catastrophesCanStart)
                {
                    StartCoroutine(ContaminationTransition());
                    StartCoroutine(BlinkingContaminationWarning());

                    isContaminationCatastropheStarted = true;
                }

                if (!catastrophesCanStart)
                {
                    StopCoroutine(ContaminationTransition());
                    StopCoroutine(BlinkingContaminationWarning());

                    isContaminationCatastropheStarted = false;
                }
                break;
            default:
                break;
        }
    }

    public IEnumerator CatastrophesTransition()
    {
        catastropheTransitionStarted = true;

        if (PlayerController.pipelineEnteredID == 0)
        {
            catastrophes = Catastrophes.NONE;
        }
        else if (PlayerController.pipelineEnteredID == 1)
        {
            catastrophes = Catastrophes.NONE;
        }
        else if (PlayerController.pipelineEnteredID == 2)
        {
            catastrophes = Catastrophes.HIGHCONTAMINATION;
            yield return new WaitForSeconds(120f);
        }
        else if (PlayerController.pipelineEnteredID == 3)
        {
            catastrophes = Catastrophes.WATERLEAK;
            yield return new WaitForSeconds(120f);
        }
        else if (PlayerController.pipelineEnteredID == 4)
        {
            catastrophes = Catastrophes.BACTERIA;
            yield return new WaitForSeconds(120f);
        }
    }

    #endregion Handle Catastrophes

    #region Blinking Warning Catastrophes Messages

    private IEnumerator BlinkingBacteriaWarning()
    {
        bacterianWarningMessage.SetActive(true);
        bacteriaWarningPanelFader.Fade();
        yield return new WaitForSeconds(0.3f);
        bacteriaWarningPanelFader.FadeOut();
        bacterianWarningMessage.SetActive(false);

        blinkingContaminationBacteriaCorroutine = BlinkingBacteriaWarning();
        StartCoroutine(blinkingContaminationBacteriaCorroutine);
    }

    private IEnumerator BlinkingContaminationWarning()
    {
        contaminationWarningMessage.SetActive(true);
        contaminationWarningPanelFader.Fade();
        yield return new WaitForSeconds(0.3f);
        contaminationWarningPanelFader.FadeOut();
        contaminationWarningMessage.SetActive(false);

        blinkingContaminationCorroutine = BlinkingContaminationWarning();
        StartCoroutine(blinkingContaminationCorroutine);
    }

    public IEnumerator BlinkingWaterLeakWarning()
    {
        waterLeakWarningMessage.SetActive(true);
        waterLeakWarningPanelFader.Fade();
        yield return new WaitForSeconds(0.3f);
        waterLeakWarningPanelFader.FadeOut();
        waterLeakWarningMessage.SetActive(false);

        blinkingWaterLeakCorroutine = BlinkingWaterLeakWarning();
        StartCoroutine(blinkingWaterLeakCorroutine);
    }

    #endregion Blinking Warning Catastrophes Messages

    #region Handle UI Depending On Language

    private void HandleUIDependingOnLanguage()
    {
        if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            waterLeakText.text = "Fuga de agua";
            contaminationText.text = "Contaminación";
            rotatePipelineText.text = "Rotar tubería";
            selectPipelineTypeText.text = "Selecciona tipo de tubería";
            bacteriaBlinkingWarningTitleText.text = "Peligro! Bacteria";
            bacteriaBlinkingWarningSubtitleText.text = "¡El agua está siendo contaminada por bacterias!";
            bacteriaBlinkingWarningDescriptionText.text = "Pulsa 5 veces el botón de descontaminar para descontaminar el agua y eliminar las bacterias";
            contaminationBlinkingWarningTitleText.text = "Peligro! Contaminación";
            contaminationBlinkingWarningSubtitleText.text = "¡El agua se está contaminando!";
            contaminationBlinkingWarningDescriptionText.text = "Pulsa 5 veces el botón de descontaminar para descontaminar el agua";
            waterLeakBlinkingWarningTitleText.text = "Peligro! Fuga de agua";
            waterLeakBlinkingWarningSubtitleText.text = "¡Está habiendo una fuga de agua!";
            waterLeakBlinkingWarningDescriptionText.text = "Pulsa el botón de abrir o cerrar el flujo de agua para reparar la tubería y la fuga de agua";
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            waterLeakText.text = "Water Leak";
            contaminationText.text = "Contamination";
            rotatePipelineText.text = "Rotate pipeline";
            selectPipelineTypeText.text = "Select Pipeline Type";
            bacteriaBlinkingWarningTitleText.text = "Warning! Bacteria";
            bacteriaBlinkingWarningSubtitleText.text = "The water is being contaminated by bacteria!";
            bacteriaBlinkingWarningDescriptionText.text = "Press 5 times the descontamination button to decontaminate the water and eliminate the bacteria";
            contaminationBlinkingWarningTitleText.text = "Warning! Contamination";
            contaminationBlinkingWarningSubtitleText.text = "The water is being contaminated!";
            contaminationBlinkingWarningDescriptionText.text = "Press 5 times the descontamination button to decontaminate the water";
            waterLeakBlinkingWarningTitleText.text = "Warning! Water Leak";
            waterLeakBlinkingWarningSubtitleText.text = "A water leak is happening!";
            waterLeakBlinkingWarningDescriptionText.text = "Press the open and close water button to repair the pipeline and water leak";
}
    }

    #endregion Handle UI Depending On Language

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        RotatePipelineUI.SetActive(false);
    }

    public IEnumerator WaitAndFinish()
    {
        yield return new WaitForSeconds(2f);
        RotatePipelineUI.SetActive(false);
        congratulationsPanel.SetActive(false);
        stampPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        MinigamesCompleted.minigame3Finished = true;
        SceneManager.LoadScene("LevelSelector");
    }
}


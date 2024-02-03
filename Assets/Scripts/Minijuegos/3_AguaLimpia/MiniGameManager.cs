using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    #region Camera Variables
    //[Header("Camera Variables")]
    private GameObject playerCamera;
    private GameObject pipelineCamera;
    private bool pipelineCameraActive;
    private GameObject RotatePipelineUI;

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
    public GameObject[] pipelines;
    public GameObject[] pipelinesIcons;
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

    #endregion UI References

    #region Rotate UI Buttons Variables
    [Header("Rotate Buttons")]
    [Space]
    public Button leftRotateButton;
    public Button rightRotateButton;
    public Button upRotateButton;
    public Button downRotateButton;
    private bool canAddRotateToButton = true;

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
        maxNumOfMovements = 5;

        SelectPipeline();

        contaminationTransitionCoroutine = ContaminationTransition();
        bacteriaTransitionCoroutine = BacteriaContaminationTransition();
        waterLeakTransitionCoroutine = WaterLeakMoreTransition();

        //StartCoroutine(ContaminationTransition());
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        ComeBackToPlayerCamera();

        if (Input.GetKeyDown(KeyCode.E) && PlayerController.playerEnteredInPipelineArea)
        {
            PipelineCamera();
        }

        HandleRemaingingMovements();

        /*if (pipelines[0].GetComponent<PipelineForeground>().pipelineInCorrecRotation transform.rotation.x == 90f)
        {
            pipeline1CorrectPlaced = true;
        }*/

        moneyText.text = "Money: " + money;

        CheckSelectedPipeline();

        //waterContamination = Mathf.Lerp(0.5f, 1f, contaminationSpeed);

        contaminationSlider.value = waterContamination;
        bacteriaSlider.value = bacteriaContamination;
        waterLeakSlider.value = waterLeak;

        if (!catastropheTransitionStarted)
        {
            StartCoroutine(CatastrophesTransition());
        }

        HandleCatastrophes();
    }

    #endregion Update

    //To select different types of pipelines
    #region Select Pipeline Types

    void SelectPipeline()
    {
        int i = 0;

        foreach (GameObject pipeline in pipelines)
        {
            if (i == selectedPipeline)
            {
                pipeline.SetActive(true);
                pipelinesIcons[i].gameObject.SetActive(true);
                pipeline.gameObject.transform.position = pipelines[0].transform.position;
                pipelineActive = pipeline.GetComponent<PipelineForeground>();

                ManageUIPipelineType();
            }
            else
            {
                pipeline.SetActive(false);
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
    }

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

        decontaminationCount++;
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

        bacteriaCleanedCount++;
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
            waterLeakedSolvedCount++;
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
        switch (pipelineActive.pipelineType)
        {
            case PipelineForeground.PipelineType.FILTER:
                RotateUI.SetActive(false);
                waterOpenAndCloseUI.SetActive(false);
                SelectUI.SetActive(true);
                decontaminationUI.SetActive(true);
                //SelectUI.transform.position = new Vector3(SelectUI.transform.position.x - 5, SelectUI.transform.position.y, SelectUI.transform.position.z);
                break;
            case PipelineForeground.PipelineType.HIGHSPEED:
                RotateUI.SetActive(false);
                waterOpenAndCloseUI.SetActive(true);
                decontaminationUI.SetActive(false);
                //SelectUI.SetActive(false);
                //waterOpenAndCloseUI.transform.position = new Vector3(waterOpenAndCloseUI.transform.position.x - 5, SelectUI.transform.position.y, SelectUI.transform.position.z);
                break;
            case PipelineForeground.PipelineType.REDIRECTION:
                RotateUI.SetActive(true);
                waterOpenAndCloseUI.SetActive(false);
                decontaminationUI.SetActive(false);

                if (canAddRotateToButton)
                {
                    leftRotateButton.onClick.AddListener(pipelineActive.RotateLeftX);
                    rightRotateButton.onClick.AddListener(pipelineActive.RotateRightX);
                    upRotateButton.onClick.AddListener(pipelineActive.RotateUpwards);
                    downRotateButton.onClick.AddListener(pipelineActive.RotateDownwards);
                    canAddRotateToButton = false;
                }
                

                //SelectUI.SetActive(false);
                //RotateUI.transform.position = new Vector3(RotateUI.transform.position.x + 160, RotateUI.transform.position.y, RotateUI.transform.position.z);
                break;
            default:
                break;
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
            StopCoroutine(contaminationTransitionCoroutine);
            StartCoroutine(DecontaminationTransition());
            clickToDecontaminateCount = 0;
        }
    }

    public void ClickToCleanBacteria()
    {
        clickToDecontaminateBacteriaCount++;

        if (clickToDecontaminateBacteriaCount >= 5)
        {
            bacteriaCoroutineRunning = false;
            StopCoroutine(bacteriaTransitionCoroutine);
            StartCoroutine(BacteriaDecontaminationTransition());
            clickToDecontaminateBacteriaCount = 0;
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

    #endregion Player Camera

    //To place in the foreground the pipeline by activating the pipelinecamera and deactivating the player camera
    #region Pipeline Camera

    private void PipelineCamera()
    {
        playerCamera.SetActive(false);
        pipelineCamera.transform.position = pipelines[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
        pipelineCamera.SetActive(true);
        pipelineCameraActive = true;
        RotatePipelineUI.SetActive(true);
        catastrophesCanStart = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        /*if (pipelineType == PipelineType.FILTER)
        {
            StartCoroutine(ContaminationTransition());
        }*/

        //TypeOfPipelineFunctionality();
    }

    #endregion Pipeline Camera

    //To handle the remainging movements text
    #region Handle Remaining Movements Text

    private void HandleRemaingingMovements()
    {
        remainingMovements = (maxNumOfMovements - currentNumOfMovements);

        if (RotatePipelineUI.activeInHierarchy)
        {
            if (remainingMovements > 1)
            {
                numOfMovementsText.text = "Remaining Movements: " + remainingMovements;
            }
            else if (remainingMovements == 1)
            {
                numOfMovementsText.text = "Remaining Movement: " + remainingMovements;
            }
            else if (remainingMovements <= 0)
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

                for (int i = 0; i < pipelines.Length; i++)
                {
                    if (pipelines[i].GetComponent<PipelineForeground>().pipelineInCorrecRotation)
                    {
                        if (pipelinesInCorrectPlace >= 0 && pipelinesInCorrectPlace <= 3)
                        {
                            pipelinesInCorrectPlace++;
                        }
                        
                    }
                }

                if (pipelinesInCorrectPlace >= 3 && decontaminationCount == 1 && bacteriaCleanedCount == 1)
                {
                    phases = Phases.TOWN;
                }

                break;
            case Phases.TOWN:
                for (int i = 0; i < pipelines.Length; i++)
                {
                    if (pipelines[i].GetComponent<PipelineForeground>().pipelineInCorrecRotation)
                    {
                        if (pipelinesInCorrectPlace >= 0 && pipelinesInCorrectPlace <= 7)
                        {
                            pipelinesInCorrectPlace++;
                        }

                    }
                }

                break;
            default:
                break;
        }
    }

    #endregion Handle MiniGame Phases

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

                    isWaterLeakCatastropheStarted = true;
                }

                if (!catastrophesCanStart)
                {
                    StopCoroutine(WaterLeakMoreTransition());

                    isWaterLeakCatastropheStarted = false;
                }

                break;
            case Catastrophes.BACTERIA:
                BacteriaContaminationButton.gameObject.SetActive(true);
                DecontaminationButton.gameObject.SetActive(false);

                if (!isBacteriaCatastropheStarted && catastrophesCanStart)
                {
                    StartCoroutine(BacteriaContaminationTransition());

                    isBacteriaCatastropheStarted = true;
                }

                if (!catastrophesCanStart)
                {
                    StopCoroutine(BacteriaDecontaminationTransition());

                    isBacteriaCatastropheStarted = false;
                }
                break;
            case Catastrophes.HIGHCONTAMINATION:
                BacteriaContaminationButton.gameObject.SetActive(false);
                DecontaminationButton.gameObject.SetActive(true);

                if (!isContaminationCatastropheStarted && catastrophesCanStart)
                {
                    StartCoroutine(ContaminationTransition());

                    isContaminationCatastropheStarted = true;
                }

                if (!catastrophesCanStart)
                {
                    StopCoroutine(ContaminationTransition());

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

        catastrophes = Catastrophes.NONE;
        yield return new WaitForSeconds(20f);
        catastrophes = Catastrophes.HIGHCONTAMINATION;
        yield return new WaitForSeconds(120f);
        catastrophes = Catastrophes.BACTERIA;
        yield return new WaitForSeconds(120f);
        catastrophes = Catastrophes.WATERLEAK;
        yield return new WaitForSeconds(120f);
    }

    #endregion Handle Catastrophes
}


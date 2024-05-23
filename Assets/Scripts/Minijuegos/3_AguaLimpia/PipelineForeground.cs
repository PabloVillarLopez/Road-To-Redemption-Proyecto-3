using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PipelineForeground : MonoBehaviour
{
    //Script for placing pipelines in the foreground, in the first plane

    #region Rotate Pipelines Variables

    //[Header("Rotate Pipeline Variables")]
    private float rotationAddedX;
    private float rotationAddedY;

    #endregion Rotate Pipelines Variables

    #region Open and Close Water Flow Variables

    [Header("Open and Close Water Flow Variables")]
    [Space]
    public bool isWaterFlowingInThisPipeline;
    private GameObject openWaterFlowButton;
    private GameObject closeWaterFlowButton;
    private TextMeshProUGUI waterFlowStatusText;

    #endregion Open and Close Water Flow Variables

    #region Catastrophes Events Reference Variable
    
    //[Header("Reference for Catastrophes")]
    private MiniGameManager gameManager;

    #endregion Catastrophes Events Reference Variable

    #region Types of Pipeline Variables

    public enum PipelineType
    {
        DECONTAMINATING,
        DECONTAMINATINGBACTERIA,
        CONTROLFLOWOFWATER,
        REDIRECTION,
        REDIRECTIONANDSELECT
    }

    [Header("Type of Pipeline")]
    public PipelineType pipelineType;
    public float decontaminationSpeed;
    public int pipelineId;

    #endregion Types of Pipeline Variables

    #region Verify Pipeline Correct Rotation

    [Header("Verify Pipeline Correct Rotation")]
    public Vector3 correctRotation;
    public bool pipelineInCorrecRotation;
    private Vector3 initialRotation;

    #endregion Verify Pipeline Correct Rotation

    #region Awake

    private void Awake()
    {
        openWaterFlowButton = GameObject.Find("OpenWaterFlowButton");
        closeWaterFlowButton = GameObject.Find("CloseWaterFlowButton");
        waterFlowStatusText = GameObject.Find("Water Flow Status Text").GetComponent<TextMeshProUGUI>();
    }

    #endregion Awake

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();
        
        rotationAddedX = transform.eulerAngles.x;
        rotationAddedY = transform.eulerAngles.y;
        initialRotation = transform.eulerAngles;
        ShowCorrectWaterFlowButton();
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        ShowWaterFlowStatus();
    }

    #endregion Update

    //To rotate the pipelines
    #region RotatePipeline

    public void RotateRightX()
    {
        /*if (gameManager.currentNumOfMovements < gameManager.maxNumOfMovements)
        {
            
        }*/

        rotationAddedY += 90f;
        transform.eulerAngles = new Vector3(initialRotation.x, rotationAddedY, rotationAddedY); //Quaternion.Euler(initialRotation.x, rotationAddedY, rotationAddedX);
        //gameManager.currentNumOfMovements++;

        DecreaseOrIncreasePointsOnWrongOrRightRotation();
    }

    public void RotateLeftX()
    {
        /*if (gameManager.currentNumOfMovements < gameManager.maxNumOfMovements)
        {
            
        }*/

        rotationAddedY -= 90f;
        transform.eulerAngles = new Vector3(initialRotation.x, rotationAddedY, rotationAddedX); //Quaternion.Euler(initialRotation.x, rotationAddedY, rotationAddedX);
        //gameManager.currentNumOfMovements++;

        DecreaseOrIncreasePointsOnWrongOrRightRotation();
    }

    public void RotateUpwards()
    {
        /*if (gameManager.currentNumOfMovements < gameManager.maxNumOfMovements)
        {
            
        }*/

        rotationAddedX += 90f;
        transform.eulerAngles = new Vector3(initialRotation.x, rotationAddedY, rotationAddedX); //Quaternion.Euler(initialRotation.x, rotationAddedY, rotationAddedX);
        //gameManager.currentNumOfMovements++;

        DecreaseOrIncreasePointsOnWrongOrRightRotation();
    }

    public void RotateDownwards()
    {
        /*if (gameManager.currentNumOfMovements < gameManager.maxNumOfMovements)
        {
            
        }*/

        rotationAddedX -= 90f;
        transform.eulerAngles = new Vector3(initialRotation.x, rotationAddedY, rotationAddedX); //Quaternion.Euler(initialRotation.x , transform.rotation.y, rotationAddedX);
        //gameManager.currentNumOfMovements++;

        DecreaseOrIncreasePointsOnWrongOrRightRotation();
    }

    #endregion RotatePipeline

    //To open and close the water flow, if open water flows through the pipeline
    #region Open and Close Water Flow

    public void OpenWaterFlow()
    {
        isWaterFlowingInThisPipeline = true;

        ShowCorrectWaterFlowButton();
    }

    public void CloseWaterFlow()
    {
        isWaterFlowingInThisPipeline = false;

        if (gameManager.catastrophes == MiniGameManager.Catastrophes.WATERLEAK)
        {
            gameManager.waterLeakCoroutineRunning = false;
            StopCoroutine(gameManager.waterLeakTransitionCoroutine);
            StartCoroutine(gameManager.WaterLeakLessTransition());
        }

        ShowCorrectWaterFlowButton();
    }

    #endregion Open and Close Water Flow

    //Pipeline UI
    #region Water Flow UI

    private void ShowWaterFlowStatus()
    {
        if (isWaterFlowingInThisPipeline && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            waterFlowStatusText.text = "Estado: Flujo de agua abierto";
        }
        else if (isWaterFlowingInThisPipeline && LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            waterFlowStatusText.text = "Status: Water Flowing";
        }
        else if (!isWaterFlowingInThisPipeline && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            waterFlowStatusText.text = "Estado: Flujo de agua cerrado";
        }
        else if(!isWaterFlowingInThisPipeline && LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            waterFlowStatusText.text = "Status: Water not Flowing";
        }
    }

    private void ShowCorrectWaterFlowButton()
    {
        if (isWaterFlowingInThisPipeline)
        {
            closeWaterFlowButton.SetActive(true);
            openWaterFlowButton.SetActive(false);
        }
        else
        {
            closeWaterFlowButton.SetActive(false);
            openWaterFlowButton.SetActive(true);
        }

        ShowWaterFlowStatus();
    }

    #endregion Water Flow UI

    //To punish decreasing points wrong movements and to reward good movements increasing points
    #region Decrease or Increase Points On Wrong Or Right Rotation

    public void DecreaseOrIncreasePointsOnWrongOrRightRotation()
    {
        switch (gameManager.difficultyLevel)
        {
            case MiniGameManager.DifficultyLevel.EASY:
                Debug.Log("Rotación tubería: " + rotationAddedX + ", " + rotationAddedY);
                Debug.Log("Rotación correcta: " + correctRotation);
                if (transform.eulerAngles == correctRotation )//rotationAddedX == correctRotation.z && rotationAddedY == correctRotation.y)
                {
                    Debug.Log("Aquí llego");
                    pipelineInCorrecRotation = true;
                    gameManager.points += 10;
                    //gameManager.pointsCanIncrease = false;
                }
                break;
            case MiniGameManager.DifficultyLevel.INTERMEDIATE:

                if (transform.eulerAngles != correctRotation)//transform.eulerAngles.x != correctRotation.x || rotationAddedY != correctRotation.y)
                {
                    gameManager.points -= 1;
                }
                else if (transform.eulerAngles == correctRotation)//rotationAddedX == correctRotation.z && rotationAddedY == correctRotation.y)
                {
                    pipelineInCorrecRotation = true;
                    gameManager.points += 5;
                    //gameManager.pointsCanIncrease = false;
                }
                break;
            case MiniGameManager.DifficultyLevel.HARD:
                gameManager.points = 1000;

                if (transform.eulerAngles == correctRotation)
                {
                    gameManager.points -= 10;
                }

                break;
            default:
                break;
        }
    }

    #endregion Decrease or Increase Points On Wrong Or Right Rotation

    /*#region Different Type of Pipeline Functionality

    private void TypeOfPipelineFunctionality()
    {
        switch (pipelineType)
        {
            case PipelineType.FILTER:
                StartCoroutine(ContaminationTransition());
                break;
            case PipelineType.HIGHSPEED:
                break;
            case PipelineType.REDIRECTION:
                break;
            default:
                break;
        }
    }

    public IEnumerator ContaminationTransition()
    {

        float elapsedTime = 0;

        while (elapsedTime < decontaminationSpeed)
        {
            elapsedTime += Time.deltaTime;

            gameManager.waterContamination = Mathf.Lerp(gameManager.waterContamination, 0f, elapsedTime / decontaminationSpeed);
            yield return null;
        }

        //StartCoroutine(ContaminationTransition());
    }

    #endregion Different Type of Pipeline Functionality*/
}

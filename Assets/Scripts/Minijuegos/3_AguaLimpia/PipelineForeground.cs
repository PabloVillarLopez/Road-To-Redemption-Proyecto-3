using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PipelineForeground : MonoBehaviour
{
    //Script for placing pipelines in the foreground, in the first plane

    #region Camera Variables
    //[Header("Camera Variables")]
    private GameObject playerCamera;
    private GameObject pipelineCamera;
    private bool pipelineCameraActive;
    private bool playerEnteredInPipelineArea;
    
    #endregion Camera Variables

    #region Rotate Pipelines Variables

    //[Header("Rotate Pipeline Variables")]
    private GameObject RotatePipelineUI;
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
        FILTER,
        HIGHSPEED,
        REDIRECTION
    }

    [Header("Type of Pipeline")]
    public PipelineType pipelineType;
    public float decontaminationSpeed;

    #endregion Types of Pipeline Variables

    #region Number of Movements Variables

    //[Header("Movements")]
    public static int maxNumOfMovements;
    private int currentNumOfMovements;
    private int remainingMovements;
    private TextMeshProUGUI numOfMovementsText;

    #endregion Number of Movements Variables

    #region Verify Pipeline Correct Rotation

    [Header("Verify Pipeline Correct Rotation")]
    public Quaternion correctRotation;
    public bool pipelineInCorrecRotation;

    #endregion Verify Pipeline Correct Rotation

    #region Awake

    private void Awake()
    {
        playerCamera = GameObject.Find("Charapter");
        pipelineCamera = GameObject.Find("PipelineCamera");
        RotatePipelineUI = GameObject.Find("RotatePipelineUI");
        openWaterFlowButton = GameObject.Find("OpenWaterFlowButton");
        closeWaterFlowButton = GameObject.Find("CloseWaterFlowButton");
        waterFlowStatusText = GameObject.Find("Water Flow Status Text").GetComponent<TextMeshProUGUI>();
        numOfMovementsText = GameObject.Find("RemainingMovementsText").GetComponent<TextMeshProUGUI>();
    }

    #endregion Awake

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        
        
        gameManager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();
        

        playerCamera.SetActive(true);
        pipelineCamera.SetActive(false);
        RotatePipelineUI.SetActive(false);

        rotationAddedX = transform.rotation.x;
        rotationAddedY = transform.rotation.y;

        ShowCorrectWaterFlowButton();

        maxNumOfMovements = 5;
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        ComeBackToPlayerCamera();

        if (Input.GetKeyDown(KeyCode.E) && playerEnteredInPipelineArea)
        {
            PipelineCamera();
        }


        HandleRemaingingMovements();
    }

    #endregion Update

    //To detect if the player is inside the pipeline range 
    #region Triggers


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            playerEnteredInPipelineArea = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerEnteredInPipelineArea = false;

        }
    }

    #endregion Triggers


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
        pipelineCamera.transform.position = gameObject.transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct pipeline
        pipelineCamera.SetActive(true);
        pipelineCameraActive = true;
        RotatePipelineUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(gameManager.ContaminationTransition());

        /*if (pipelineType == PipelineType.FILTER)
        {
            StartCoroutine(ContaminationTransition());
        }*/

        //TypeOfPipelineFunctionality();
    }

    #endregion Pipeline Camera

    //To rotate the pipelines
    #region RotatePipeline

    public void RotateRightX()
    {
        if (currentNumOfMovements < maxNumOfMovements)
        {
            rotationAddedY += 45f;
            transform.rotation = Quaternion.Euler(transform.rotation.x, rotationAddedY, transform.rotation.z);
            currentNumOfMovements++;

            DecreaseOrIncreasePointsOnWrongOrRightRotation();
        }
    }

    public void RotateLeftX()
    {
        if (currentNumOfMovements < maxNumOfMovements)
        {
            rotationAddedY += 45f;
            transform.rotation = Quaternion.Euler(transform.rotation.x, rotationAddedY, transform.rotation.z);
            currentNumOfMovements++;

            DecreaseOrIncreasePointsOnWrongOrRightRotation();
        }
    }

    public void RotateUpwards()
    {
        if (currentNumOfMovements < maxNumOfMovements)
        {
            rotationAddedX += 45f;
            transform.rotation = Quaternion.Euler(rotationAddedX, transform.rotation.y, transform.rotation.z);
            currentNumOfMovements++;

            DecreaseOrIncreasePointsOnWrongOrRightRotation();
        }
    }

    public void RotateDownwards()
    {
        if (currentNumOfMovements < maxNumOfMovements)
        {
            rotationAddedX -= 45f;
            transform.rotation = Quaternion.Euler(rotationAddedX, transform.rotation.y, transform.rotation.z);
            currentNumOfMovements++;

            DecreaseOrIncreasePointsOnWrongOrRightRotation();
        }
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

        ShowCorrectWaterFlowButton();
    }

    #endregion Open and Close Water Flow

    //Pipeline UI
    #region Water Flow UI

    private void ShowWaterFlowStatus()
    {
        if (isWaterFlowingInThisPipeline)
        {
            waterFlowStatusText.text = "Status: Water Flowing";
        }
        else
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

    //To handle the remainging movements text
    #region Handle Remaining Movements Text

    private void HandleRemaingingMovements()
    {
        if (!RotatePipelineUI.activeInHierarchy)
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

        remainingMovements = (maxNumOfMovements - currentNumOfMovements);
        
    }

    #endregion Handle Remainging Movements Text

    //To punish decreasing points wrong movements and to reward good movements increasing points
    #region Decrease or Increase Points On Wrong Or Right Rotation

    public void DecreaseOrIncreasePointsOnWrongOrRightRotation()
    {
        switch (gameManager.difficultyLevel)
        {
            case MiniGameManager.DifficultyLevel.EASY:
                if (transform.rotation == correctRotation)
                {
                    pipelineInCorrecRotation = true;
                    gameManager.points += 10;
                    //gameManager.pointsCanIncrease = false;
                }
                break;
            case MiniGameManager.DifficultyLevel.INTERMEDIATE:

                if (transform.rotation != correctRotation)
                {
                    gameManager.points -= 1;
                }
                else if (transform.rotation == correctRotation)
                {
                    pipelineInCorrecRotation = true;
                    gameManager.points += 5;
                    //gameManager.pointsCanIncrease = false;
                }
                break;
            case MiniGameManager.DifficultyLevel.HARD:
                gameManager.points = 1000;

                if (transform.rotation != correctRotation)
                {
                    gameManager.points -= 10;
                }

                break;
            default:
                break;
        }
    }

    #endregion Decrease or Increase Points On Wrong Or Right Rotation

    #region Different Type of Pipeline Functionality

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

    #endregion Different Type of Pipeline Functionality
}

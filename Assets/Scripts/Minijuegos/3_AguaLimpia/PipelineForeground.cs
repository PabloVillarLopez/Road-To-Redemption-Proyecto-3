using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PipelineForeground : MonoBehaviour
{
    //Script for placing pipelines in the foreground, in the first plane

    #region Camera Variables
    [Header("Camera Variables")]
    public GameObject playerCamera;
    public GameObject pipelineCamera;
    private bool pipelineCameraActive;
    private bool playerEnteredInPipelineArea;
    
    #endregion Camera Variables

    #region Rotate Pipelines Variables

    [Header("Rotate Pipeline Variables")]
    public GameObject RotatePipelineUI;
    private float rotationAddedX;
    private float rotationAddedY;

    #endregion Rotate Pipelines Variables

    #region Open and Close Water Flow Variables

    [Header("Open and Close Water Flow Variables")]
    public bool isWaterFlowingInThisPipeline;
    public GameObject openWaterFlowButton;
    public GameObject closeWaterFlowButton;
    public TextMeshProUGUI waterFlowStatusText;

    #endregion Open and Close Water Flow Variables

    #region Catastrophes Events Reference Variable
    
    [Header("Reference for Catastrophes")]
    public MiniGameManager gameManager;

    #endregion Catastrophes Events Reference Variable

    #region Types of Pipeline

    public enum PipelineType
    {
        FILTER,
        HIGHSPEED,
        REDIRECTION
    }

    [Header("Type of Pipeline")]
    public PipelineType pipelineType;

    #endregion Types of Pipeline

    #region Number of Movements

    [Header("Movements")]
    public static int maxNumOfMovements;
    public int currentNumOfMovements;
    public int remainingMovements;
    public TextMeshProUGUI numOfMovementsText;

    #endregion Number of Movements

    #region Awake

    private void Awake()
    {
        playerCamera.SetActive(true);
        pipelineCamera.SetActive(false);
        RotatePipelineUI.SetActive(false);
    }

    #endregion Awake

    #region Start

    // Start is called before the first frame update
    void Start()
    {
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

        remainingMovements = (maxNumOfMovements - currentNumOfMovements);
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
        }
        
    }

    public void RotateLeftX()
    {
        if (currentNumOfMovements < maxNumOfMovements)
        {
            rotationAddedY += 45f;
            transform.rotation = Quaternion.Euler(transform.rotation.x, rotationAddedY, transform.rotation.z);
            currentNumOfMovements++;
        }
        
    }

    public void RotateUpwards()
    {
        if (currentNumOfMovements < maxNumOfMovements)
        {
            rotationAddedX += 45f;
            transform.rotation = Quaternion.Euler(rotationAddedX, transform.rotation.y, transform.rotation.z);
            currentNumOfMovements++;
        }
        
    }

    public void RotateDownwards()
    {
        if (currentNumOfMovements < maxNumOfMovements)
        {
            rotationAddedX -= 45f;
            transform.rotation = Quaternion.Euler(rotationAddedX, transform.rotation.y, transform.rotation.z);
            currentNumOfMovements++;
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

}

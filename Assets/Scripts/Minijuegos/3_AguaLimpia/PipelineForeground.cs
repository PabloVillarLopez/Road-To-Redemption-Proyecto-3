using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PipelineForeground : MonoBehaviour
{
    //Script for placing pipelines in the foreground, in the first plane

    #region Rotate Pipelines Variables

    //[Header("Rotate Pipeline Variables")]
    [HideInInspector]
    public float rotationAddedX;
    [HideInInspector]
    public float rotationAddedY;

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
        REDIRECTIONANDSELECT,
        DECONTAMINATINGANDSELECT,
        CONTROLFLOWOFWATERANDSELECT
    }

    [Header("Type of Pipeline")]
    public PipelineType pipelineType;
    public float decontaminationSpeed;
    public int pipelineId;
    public int pipelineRotateId;

    #endregion Types of Pipeline Variables

    #region Verify Pipeline Correct Rotation

    [Header("Verify Pipeline Correct Rotation")]
    public Vector3 correctRotation;
    public Vector3 correctRotation2;
    public bool pipelineInCorrecRotation;
    private Vector3 initialRotation;

    #endregion Verify Pipeline Correct Rotation

    public bool alreadyDecontaminated;
    private int rotationCount = 0;
    public GameObject player;
    public GameObject arrowAsociated;
    public GameObject vfxAsociated;

    [Header("Waterfalls")]
    public GameObject waterfallContamination;
    public GameObject waterfallRedirection1;
    public GameObject waterfallRedirection2;
    public GameObject waterfallRedirection3;
    public GameObject waterfallRedirection4;
    public GameObject waterfallRedirection5;
    public GameObject waterfallRedirection6;
    public GameObject waterfallRedirection7;

    [Header("Material change contamination pipeline")]
    public Material straightPipelineMaterial;
    private Renderer pipelineRenderer;

    [Header("Minigame Manager")]
    public MiniGameManager minigameManager;

    [Header("Blocking Pipeline")]
    public bool canInteract;
    public GameObject[] pipelinesRotate;

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
        
        rotationAddedX = this.transform.eulerAngles.x;
        rotationAddedY = this.transform.eulerAngles.y;
        initialRotation = this.transform.eulerAngles;
        ShowCorrectWaterFlowButton();

        if (pipelineType == PipelineType.REDIRECTION || pipelineType == PipelineType.REDIRECTIONANDSELECT)
        {
            waterfallRedirection1.SetActive(true);
            waterfallRedirection2.SetActive(false);
            waterfallRedirection3.SetActive(false);
            waterfallRedirection4.SetActive(false);
            waterfallRedirection5.SetActive(false);
            waterfallRedirection6.SetActive(false);
            waterfallRedirection7.SetActive(true);
        }
        
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
        #region Not Used for Now
        /*if (gameManager.currentNumOfMovements < gameManager.maxNumOfMovements)
        {
            
        }*/

        /*if (rotationCount == 0)
        {
            rotationAddedX = PlayerController.pipeRotateX;
            rotationAddedY = PlayerController.pipeRotateY;
        }

        rotationCount++;

        GameObject pipe = PlayerController.pipelineEntered;
        PipelineForeground pipeScript = pipe.GetComponent<PipelineForeground>();


        rotationAddedY += 90f;
        if (rotationAddedY == 360)
        {
            rotationAddedY = 0;
        }
        else if (rotationAddedY < 0)
        {
            rotationAddedY += 360;
        }

        pipe.transform.eulerAngles = new Vector3(pipe.transform.eulerAngles.x, rotationAddedY, rotationAddedX); //Quaternion.Euler(initialRotation.x, rotationAddedY, rotationAddedX);
        //gameManager.currentNumOfMovements++;

        //DecreaseOrIncreasePointsOnWrongOrRightRotation();
        VerifyCorrectRotation();*/

        #endregion Not Used For Now

        RotatePipeline(90f, 0);
    }

    public void RotateLeftX()
    {
        #region Not Used For Now

        /*if (gameManager.currentNumOfMovements < gameManager.maxNumOfMovements)
        {
            
        }*/

        /*if (rotationCount == 0)
        {
            rotationAddedX = PlayerController.pipeRotateX;
            rotationAddedY = PlayerController.pipeRotateY;
        }

        rotationCount++;

        GameObject pipe = PlayerController.pipelineEntered;
        PipelineForeground pipeScript = pipe.GetComponent<PipelineForeground>();

        rotationAddedY -= 90f;
        if (rotationAddedY == 360)
        {
            rotationAddedY = 0;
        }
        else if (rotationAddedY < 0)
        {
            rotationAddedY += 360;
        }

        pipe.transform.eulerAngles = new Vector3(pipe.transform.eulerAngles.x, rotationAddedY, rotationAddedX); //Quaternion.Euler(initialRotation.x, rotationAddedY, rotationAddedX);
        //gameManager.currentNumOfMovements++;

        //DecreaseOrIncreasePointsOnWrongOrRightRotation();
        VerifyCorrectRotation();*/

        #endregion Not Used For Now

        RotatePipeline(-90f, 0);
    }

    public void RotateUpwards()
    {
        #region Not Used For Now

        /*if (gameManager.currentNumOfMovements < gameManager.maxNumOfMovements)
        {
            
        }*/

        /*if (rotationCount == 0)
        {
            rotationAddedX = PlayerController.pipeRotateX;
            rotationAddedY = PlayerController.pipeRotateY;
        }

        rotationCount++;

        GameObject pipe = PlayerController.pipelineEntered;
        PipelineForeground pipeScript = pipe.GetComponent<PipelineForeground>();

        rotationAddedX += 90f;
        if (rotationAddedX == 360)
        {
            rotationAddedX = 0;
        }
        else if (rotationAddedX < 0)
        {
            rotationAddedX += 360;
        }

        pipe.transform.eulerAngles = new Vector3(pipe.transform.eulerAngles.x, rotationAddedY, rotationAddedX); //Quaternion.Euler(initialRotation.x, rotationAddedY, rotationAddedX);
        //gameManager.currentNumOfMovements++;

        //DecreaseOrIncreasePointsOnWrongOrRightRotation();
        VerifyCorrectRotation();*/

        #endregion Not Used for Now

        RotatePipeline(0, 90f);
    }

    public void RotateDownwards()
    {
        #region Not Used for Now

        /*if (gameManager.currentNumOfMovements < gameManager.maxNumOfMovements)
        {
            
        }*/

        /*if (rotationCount == 0)
        {
            rotationAddedX = PlayerController.pipeRotateX;
            rotationAddedY = PlayerController.pipeRotateY;
        }

        rotationCount++;

        GameObject pipe = PlayerController.pipelineEntered;
        PipelineForeground pipeScript = pipe.GetComponent<PipelineForeground>();

        rotationAddedX -= 90f;
        if (rotationAddedX == 360)
        {
            rotationAddedX = 0;
        }
        else if (rotationAddedX < 0)
        {
            rotationAddedX += 360;
        }

        pipe.transform.eulerAngles = new Vector3(pipe.transform.eulerAngles.x, rotationAddedY, rotationAddedX); //Quaternion.Euler(initialRotation.x , transform.rotation.y, rotationAddedX);
        //gameManager.currentNumOfMovements++;

        //DecreaseOrIncreasePointsOnWrongOrRightRotation();
        VerifyCorrectRotation();*/

        #endregion Not Used for Now

        RotatePipeline(0, -90f);
    }

    private void RotatePipeline(float deltaX, float deltaY)
    {
        if (rotationCount == 0)
        {
            rotationAddedX = PlayerController.pipeRotateX;
            rotationAddedY = PlayerController.pipeRotateY;
        }

        rotationCount++;

        //GameObject pipe = PlayerController.pipelineEntered;
        //PipelineForeground pipeScript = pipe.GetComponent<PipelineForeground>();
        GameObject pipe = minigameManager.pipelineActiveGameObject;
        PipelineForeground pipeScript = minigameManager.pipelineActive;

        pipeScript.rotationAddedX += deltaX;
        pipeScript.rotationAddedY += deltaY;

        if (pipeScript.rotationAddedX >= 360) pipeScript.rotationAddedX -= 360;
        else if (pipeScript.rotationAddedX < 0) pipeScript.rotationAddedX += 360;

        if (pipeScript.rotationAddedY >= 360) pipeScript.rotationAddedY -= 360;
        else if (pipeScript.rotationAddedY < 0) pipeScript.rotationAddedY += 360;

        pipe.transform.eulerAngles = new Vector3(pipe.transform.eulerAngles.x, pipeScript.rotationAddedY, pipeScript.rotationAddedX);

        VerifyCorrectRotation();
    }

    #endregion RotatePipeline

    //To open and close the water flow, if open water flows through the pipeline
    #region Open and Close Water Flow

    public void OpenWaterFlow()
    {
        minigameManager.PlaySound(3);

        GameObject pipe = PlayerController.pipelineEntered;
        PipelineForeground pipeScript = pipe.GetComponent<PipelineForeground>();

        pipeScript.isWaterFlowingInThisPipeline = true;
        ShowCorrectWaterFlowButton();
    }

    public void CloseWaterFlow()
    {
        minigameManager.PlaySound(3);

        GameObject pipe = PlayerController.pipelineEntered;
        PipelineForeground pipeScript = pipe.GetComponent<PipelineForeground>();

        //GameObject pipe = minigameManager.pipelineActiveGameObject;
        //PipelineForeground pipeScript = minigameManager.pipelineActive;

        pipeScript.isWaterFlowingInThisPipeline = false;

        if (gameManager.catastrophes == MiniGameManager.Catastrophes.WATERLEAK)
        {
            waterfallContamination.SetActive(false);
            gameManager.waterLeakCoroutineRunning = false;
            StopCoroutine(gameManager.waterLeakTransitionCoroutine);
            StartCoroutine(gameManager.WaterLeakLessTransition());
            gameManager.StopCoroutine(gameManager.blinkingWaterLeakCorroutine);
            gameManager.waterLeakWarningPanelFader.FadeOut();
            gameManager.waterLeakWarningPanelFader.canvGroup.gameObject.SetActive(false);
            gameManager.congratulationsPanel.SetActive(true);
            
            gameManager.congratulationsPanelText.text = "Congratulations on controlling efficiently the water flow.";
            alreadyDecontaminated = true;
            PlayerController.pipelineEntered.GetComponent<PipelineForeground>().arrowAsociated.SetActive(false);
            PlayerController.pipelineEntered.GetComponent<PipelineForeground>().vfxAsociated.SetActive(false);
            pipelineRenderer = PlayerController.pipelineEntered.GetComponent<Renderer>();
            pipelineRenderer.sharedMaterial = straightPipelineMaterial;
            closeWaterFlowButton.SetActive(false);
            openWaterFlowButton.SetActive(false);
        }

        ShowCorrectWaterFlowButton();
    }

    #endregion Open and Close Water Flow

    //Pipeline UI
    #region Water Flow UI

    private void ShowWaterFlowStatus()
    {
        /*if (isWaterFlowingInThisPipeline && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
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
        }*/

        if (PlayerController.pipelineEntered != null)
        {
            GameObject pipe = PlayerController.pipelineEntered;
            PipelineForeground pipeScript = pipe.GetComponent<PipelineForeground>();

            //GameObject pipe = minigameManager.pipelineActiveGameObject;
            //PipelineForeground pipeScript = minigameManager.pipelineActive;

            if (pipeScript != null && pipeScript.isWaterFlowingInThisPipeline)
            {
                waterFlowStatusText.text = "Estado: Flujo de agua abierto";
            }
            else if (pipeScript != null && !pipeScript.isWaterFlowingInThisPipeline)
            {
                waterFlowStatusText.text = "Estado: Flujo de agua cerrado";
            }
        }
    }

    private void ShowCorrectWaterFlowButton()
    {
        if (isWaterFlowingInThisPipeline && !alreadyDecontaminated)
        {
            closeWaterFlowButton.SetActive(true);
            openWaterFlowButton.SetActive(false);
        }
        else if (!isWaterFlowingInThisPipeline && !alreadyDecontaminated)
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

    public void VerifyCorrectRotation()
    {
        //GameObject pipe = PlayerController.pipelineEntered;
        //PipelineForeground pipeScript = pipe.GetComponent<PipelineForeground>();

        GameObject pipe = minigameManager.pipelineActiveGameObject;
        PipelineForeground pipeScript = minigameManager.pipelineActive;

        Debug.Log("Rotación tubería: " + pipeScript.rotationAddedX + ", " + pipeScript.rotationAddedY);
        Debug.Log("Rotación tubería real: " + pipe.transform.eulerAngles.x + " , " + pipe.transform.eulerAngles.y + " , " + pipe.transform.eulerAngles.z);
        Debug.Log("Rotación correcta: " + pipeScript.correctRotation);

        if (IsRotationCorrect(pipe.transform.eulerAngles))
        {
            minigameManager.PlaySound(5); //good feedback sound
            Debug.Log("Aquí llego");
            pipelineInCorrecRotation = true;
            gameManager.points += 10;
            gameManager.congratulationsPanel.SetActive(true);

            gameManager.leftRotateButton.onClick.RemoveAllListeners();
            gameManager.rightRotateButton.onClick.RemoveAllListeners();
            gameManager.upRotateButton.onClick.RemoveAllListeners();
            gameManager.downRotateButton.onClick.RemoveAllListeners();

            gameManager.leftRotateButton.gameObject.SetActive(false);
            gameManager.rightRotateButton.gameObject.SetActive(false);
            gameManager.upRotateButton.gameObject.SetActive(false);
            gameManager.downRotateButton.gameObject.SetActive(false);
            rotationCount = 0;
            gameManager.pipelinesInCorrectPlace++;
            pipeScript.canInteract = false;

            /*if (pipeScript.pipelineRotateId == 1)
            {
                player.transform.position = new Vector3(-5.88999987f, 1.02022767f, 0.893214941f);
            }*/

            switch (pipeScript.pipelineRotateId)
            {
                case 0:
                    pipelinesRotate[1].GetComponent<PipelineForeground>().canInteract = true;
                    pipelinesRotate[1].GetComponent<PipelineForeground>().arrowAsociated.SetActive(true);
                    pipelinesRotate[1].GetComponent<PipelineForeground>().vfxAsociated.SetActive(true);
                    waterfallRedirection1.SetActive(false);
                    waterfallRedirection2.SetActive(true);
                    break;
                case 1:
                    pipelinesRotate[2].GetComponent<PipelineForeground>().canInteract = true;
                    pipelinesRotate[2].GetComponent<PipelineForeground>().arrowAsociated.SetActive(true);
                    pipelinesRotate[2].GetComponent<PipelineForeground>().vfxAsociated.SetActive(true);
                    waterfallRedirection2.SetActive(false);
                    waterfallRedirection3.SetActive(true);
                    break;
                case 2:
                    waterfallRedirection3.SetActive(false);
                    break;
                case 3:
                    waterfallRedirection4.SetActive(false);
                    break;
                case 4:
                    pipelinesRotate[3].GetComponent<PipelineForeground>().canInteract = true;
                    pipelinesRotate[3].GetComponent<PipelineForeground>().arrowAsociated.SetActive(true);
                    pipelinesRotate[3].GetComponent<PipelineForeground>().vfxAsociated.SetActive(true);
                    waterfallRedirection5.SetActive(false);
                    waterfallRedirection4.SetActive(true);
                    break;
                case 5:
                    pipelinesRotate[4].GetComponent<PipelineForeground>().canInteract = true;
                    pipelinesRotate[4].GetComponent<PipelineForeground>().arrowAsociated.SetActive(true);
                    pipelinesRotate[4].GetComponent<PipelineForeground>().vfxAsociated.SetActive(true);
                    waterfallRedirection6.SetActive(false);
                    waterfallRedirection5.SetActive(true);
                    break;
                case 6:
                    pipelinesRotate[5].GetComponent<PipelineForeground>().canInteract = true;
                    pipelinesRotate[5].GetComponent<PipelineForeground>().arrowAsociated.SetActive(true);
                    pipelinesRotate[5].GetComponent<PipelineForeground>().vfxAsociated.SetActive(true);
                    waterfallRedirection7.SetActive(false);
                    waterfallRedirection6.SetActive(true);
                    break;
                default:
                    break;
            }

            pipeScript.arrowAsociated.SetActive(false);
            pipeScript.vfxAsociated.SetActive(false);
        }
        else
        {
            minigameManager.PlaySound(6); //negative feedback
        }




        /*if (pipe.transform.eulerAngles == pipeScript.correctRotation || pipeScript.rotationAddedX == pipeScript.correctRotation.z && pipeScript.rotationAddedY == pipeScript.correctRotation.y || pipe.transform.eulerAngles == pipeScript.correctRotation2 || pipeScript.rotationAddedX == pipeScript.correctRotation2.z && pipeScript.rotationAddedY == pipeScript.correctRotation2.y || pipe.transform.eulerAngles.y == correctRotation.y && pipe.transform.eulerAngles.z == correctRotation.z)//rotationAddedX == correctRotation.z && rotationAddedY == correctRotation.y)
        {
            Debug.Log("Aquí llego");
            pipelineInCorrecRotation = true;
            gameManager.points += 10;
            gameManager.congratulationsPanel.SetActive(true);

            gameManager.leftRotateButton.onClick.RemoveAllListeners();
            gameManager.rightRotateButton.onClick.RemoveAllListeners();
            gameManager.upRotateButton.onClick.RemoveAllListeners();
            gameManager.downRotateButton.onClick.RemoveAllListeners();

            gameManager.leftRotateButton.gameObject.SetActive(false);
            gameManager.rightRotateButton.gameObject.SetActive(false);
            gameManager.upRotateButton.gameObject.SetActive(false);
            gameManager.downRotateButton.gameObject.SetActive(false);
            rotationCount = 0;
            //gameManager.pointsCanIncrease = false;
        }*/
    }

    private bool IsRotationCorrect(Vector3 currentRotation)
    {
        float tolerance = 0.1f;
        return (Approximately(currentRotation, correctRotation, tolerance) || Approximately(currentRotation, correctRotation2, tolerance));
    }

    private bool Approximately(Vector3 a, Vector3 b, float tolerance)
    {
        return Mathf.Abs(a.x - b.x) < tolerance && Mathf.Abs(a.y - b.y) < tolerance && Mathf.Abs(a.z - b.z) < tolerance;
    }

    #endregion Decrease or Increase Points On Wrong Or Right Rotation

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        gameManager.RotatePipelineUI.SetActive(false);
    }

    #region Not Used

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
    #endregion Not Used
}

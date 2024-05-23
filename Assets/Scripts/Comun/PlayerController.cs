using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Movement Variables
    [Header("Movement Variables")]
    public float movementSpeed;
    private Transform orientation;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rigy;

    #endregion Movement Variables

    #region Variable for camera changes
    public static bool playerEnteredInRotatePipelineArea;
    public static bool playerEnteredInDecontaminatePipelineArea;
    public static bool playerEnteredInObjectClue1Area;
    public static bool playerEnteredInObjectClue2Area;
    public static bool playerEnteredInObjectClue3Area;
    public static int pipelineEnteredID;

    #endregion Variable for camera changes

    #region Dialogue Reference Variable

    [Header("Dialogue Script Reference")]
    public DialogueScript dialogueScript;
    public bool canDialogue;
    public bool dialogueStarted;

    public GameObject interactIndicator;

    #endregion Dialogue Reference Variable

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        rigy = GetComponent<Rigidbody>();
        orientation = transform.GetChild(3).transform;

        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }
        
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();

        if (Input.GetKeyDown(KeyCode.E) && !canDialogue && !dialogueScript.dialogueStarted)
        {
            canDialogue = true;
        }
    }

    #endregion Update

    #region Fixed Update
    private void FixedUpdate()
    {
        MovePlayer();
    }

    #endregion Fixed Update

    #region Movement
        #region Get Movement Input

    private void MyInput() //Gets movement in horizontal and vertical axis with AWSD and arrows
    {
        if (!ObserveObject.cantMove)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        
    }

        #endregion Get Movement Input

        #region Move Player with Direction

    private void MovePlayer() //Moves player taking into account the direction the player is facing
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rigy.AddForce(moveDirection.normalized * movementSpeed * 10, ForceMode.Force);
    }

        #endregion Move Player with Direction

        #region Max Speed Control
    private void SpeedControl() //Limits the velocity to the max speed velocity and controls that max velocity
    {
        Vector3 flatVelocity = new Vector3(rigy.velocity.x, 0f, rigy.velocity.z);
        if (flatVelocity.magnitude > movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            rigy.velocity = new Vector3(limitedVelocity.x, rigy.velocity.y, limitedVelocity.z);
        }
    }

    #endregion Max Speed Control

    #endregion Movement

    //To detect if the player is inside the pipeline range 
    #region Triggers


    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.CompareTag("Pipeline"))
        {
            playerEnteredInPipelineArea = true;
            pipelineEnteredID = other.gameObject.GetComponent<PipelineForeground>().pipelineId;
        }*/

        if (other.gameObject.CompareTag("PipelineRotate") && MiniGameManager.canInteractWithRotatePipelines)
        {
            playerEnteredInRotatePipelineArea = true;
            pipelineEnteredID = other.gameObject.GetComponent<PipelineForeground>().pipelineId;
        }

        if (other.gameObject.CompareTag("PipelineDecontaminate"))
        {
            playerEnteredInDecontaminatePipelineArea = true;
            pipelineEnteredID = other.gameObject.GetComponent<PipelineForeground>().pipelineId;
        }

        if (other.gameObject.CompareTag("JusticeClue1"))
        {
            playerEnteredInObjectClue1Area = true;
        }

        if (other.gameObject.CompareTag("JusticeClue2"))
        {
            playerEnteredInObjectClue2Area = true;
        }

        if (other.gameObject.CompareTag("JusticeClue3"))
        {
            playerEnteredInObjectClue3Area = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CanDialogue") && LanguageManager.currentLanguage == LanguageManager.Language.Spanish && canDialogue)
        {
            dialogueScript.StartSpanishDialogue();
            canDialogue = false;
            //dialogueStarted = true;
        }

        if (other.gameObject.CompareTag("CanDialogue") && interactIndicator != null)
        {
            interactIndicator.SetActive(true);
        }

        if (other.gameObject.CompareTag("CanDialogue") && LanguageManager.currentLanguage == LanguageManager.Language.English && canDialogue)
        {
            dialogueScript.StartEnglishDialogue();
            canDialogue = false;
            //dialogueStarted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PipelineRotate") && MiniGameManager.canInteractWithRotatePipelines)
        {
            playerEnteredInRotatePipelineArea = false;
            pipelineEnteredID = 0;
        }

        if (other.gameObject.CompareTag("PipelineDecontaminate"))
        {
            playerEnteredInDecontaminatePipelineArea = false;
            pipelineEnteredID = 0;
        }

        if (other.gameObject.CompareTag("JusticeClue1"))
        {
            playerEnteredInObjectClue1Area = false;
        }

        if (other.gameObject.CompareTag("JusticeClue2"))
        {
            playerEnteredInObjectClue2Area = false;
        }

        if (other.gameObject.CompareTag("JusticeClue3"))
        {
            playerEnteredInObjectClue3Area = false;
        }

        if (other.gameObject.CompareTag("CanDialogue") && interactIndicator != null)
        {
            interactIndicator.SetActive(false);
            //dialogueStarted = false;
        }

        //if (other.gameObject.CompareTag("CanDialogue") && canDialogue)
        //{
        //    canDialogue = false;
        //}
    }

    #endregion Triggers
}

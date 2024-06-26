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
    public static int pipelineRotateEnteredID;
    public static GameObject pipelineEntered;

    #endregion Variable for camera changes

    #region Dialogue Reference Variable

    [Header("Dialogue Script Reference")]
    public DialogueScript dialogueScript;
    public bool canDialogue;
    public bool dialogueStarted;

    public GameObject interactIndicatorSpanish;
    public GameObject interactIndicatorEnglish;

    #endregion Dialogue Reference Variable

    public static bool pipelineEnteredHasBeenAlreadyDecontaminated;
    public static bool pipelineEnteredHasBeenAlreadyRotated;
    public static float pipeRotateX;
    public static float pipeRotateY;

    [Header("Animations / Animaciones")]
    public GameObject armsIdle;
    public GameObject armsRun;
    public Animator armsIdleAimController;
    public Animator armsRunAimController;

    [Header("Walking Sounds")]
    public List<AudioClip> walkingSoundsMetalOrSand = new List<AudioClip>(); // Lista de clips de sonido 
    public AudioSource walkingSound;
    public enum Minigames
    {
        Minigame1,
        Minigame3,
        Minigame5,
        Minigame6
    }

    public Minigames currentMinigame;

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        rigy = GetComponent<Rigidbody>();
        orientation = transform.GetChild(3).transform;

        if (interactIndicatorEnglish != null)
        {
            interactIndicatorEnglish.SetActive(false);
        }

        if (interactIndicatorSpanish != null)
        {
            interactIndicatorSpanish.SetActive(false);
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

        Debug.Log(pipelineEntered);
        //Debug.Log()
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

            if (verticalInput != 0 || horizontalInput != 0)
            {
                CheckWalkingSound();
            }
            else
            {
                if (walkingSound != null)
                {
                    walkingSound.Stop();
                }
            }

            if ((verticalInput != 0 && armsIdle != null && armsRun != null) || (horizontalInput != 0 && armsIdle != null && armsRun != null))
            {
                armsIdle.SetActive(false);
                armsRun.SetActive(true);
            }
            else if((verticalInput == 0 && armsIdle != null && armsRun != null) || (horizontalInput == 0 && armsIdle != null && armsRun != null))
            {
                armsRun.SetActive(false);
                armsIdle.SetActive(true);
            }
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
            pipelineRotateEnteredID = other.gameObject.GetComponent<PipelineForeground>().pipelineRotateId;
            pipelineEntered = other.gameObject;
            pipelineEnteredHasBeenAlreadyRotated = other.gameObject.GetComponent<PipelineForeground>().pipelineInCorrecRotation;
            MiniGameManager.canAddRotateToButton = true;
            pipeRotateX = other.gameObject.GetComponent<PipelineForeground>().rotationAddedX;
            pipeRotateY = other.gameObject.GetComponent<PipelineForeground>().rotationAddedY;

            if (!pipelineEnteredHasBeenAlreadyRotated && LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                interactIndicatorEnglish.SetActive(true);
            }

            if (!pipelineEnteredHasBeenAlreadyRotated && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                interactIndicatorSpanish.SetActive(true);
            }
        }

        if (other.gameObject.CompareTag("PipelineDecontaminate"))
        {
            if (!other.gameObject.GetComponent<PipelineForeground>().alreadyDecontaminated)
            {
                playerEnteredInDecontaminatePipelineArea = true;
                pipelineEnteredID = other.gameObject.GetComponent<PipelineForeground>().pipelineId;
                pipelineEntered = other.gameObject;
                pipelineEnteredHasBeenAlreadyDecontaminated = other.gameObject.GetComponent<PipelineForeground>().alreadyDecontaminated;

                if (!pipelineEnteredHasBeenAlreadyDecontaminated && LanguageManager.currentLanguage == LanguageManager.Language.English)
                {
                    interactIndicatorEnglish.SetActive(true);
                }

                if (!pipelineEnteredHasBeenAlreadyDecontaminated && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                {
                    interactIndicatorSpanish.SetActive(true);
                }
            }
            
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

        if (other.gameObject.CompareTag("CanDialogue") && LanguageManager.currentLanguage == LanguageManager.Language.English && interactIndicatorEnglish != null && !dialogueScript.dialoguePanel.activeInHierarchy)
        {
            interactIndicatorEnglish.SetActive(true);
        }

        if (other.gameObject.CompareTag("CanDialogue") && LanguageManager.currentLanguage == LanguageManager.Language.English && interactIndicatorEnglish != null && dialogueScript.dialoguePanel.activeInHierarchy)
        {
            interactIndicatorEnglish.SetActive(false);
        }

        if (other.gameObject.CompareTag("CanDialogue") && LanguageManager.currentLanguage == LanguageManager.Language.Spanish && interactIndicatorSpanish != null && !dialogueScript.dialoguePanel.activeInHierarchy)
        {
            interactIndicatorEnglish.SetActive(true);
        }

        if (other.gameObject.CompareTag("CanDialogue") && LanguageManager.currentLanguage == LanguageManager.Language.Spanish && interactIndicatorSpanish != null && dialogueScript.dialoguePanel.activeInHierarchy)
        {
            interactIndicatorEnglish.SetActive(false);
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
        if (other.gameObject.CompareTag("PipelineRotate") && MiniGameManager.canInteractWithRotatePipelines && this.gameObject.activeInHierarchy)
        {
            playerEnteredInRotatePipelineArea = false;
            pipelineEnteredID = 0;
            pipelineEntered = null;
            pipelineEnteredHasBeenAlreadyRotated = false;
            pipeRotateX = 0;
            pipeRotateY = 0;
        }

        if (other.gameObject.CompareTag("PipelineRotate") && MiniGameManager.canInteractWithRotatePipelines && LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            interactIndicatorEnglish.SetActive(false);
        }

        if (other.gameObject.CompareTag("PipelineRotate") && MiniGameManager.canInteractWithRotatePipelines && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            interactIndicatorSpanish.SetActive(false);
        }

        if (other.gameObject.CompareTag("PipelineDecontaminate"))
        {
            playerEnteredInDecontaminatePipelineArea = false;
            pipelineEnteredID = 0;
            pipelineEntered = null;
            pipelineEnteredHasBeenAlreadyDecontaminated = false;
        }

        if (other.gameObject.CompareTag("PipelineDecontaminate") && LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            interactIndicatorEnglish.SetActive(false);
        }

        if (other.gameObject.CompareTag("PipelineDecontaminate") && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            interactIndicatorSpanish.SetActive(false);
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

        if (other.gameObject.CompareTag("CanDialogue") && LanguageManager.currentLanguage == LanguageManager.Language.English && interactIndicatorEnglish != null)
        {
            interactIndicatorEnglish.SetActive(false);
            //dialogueStarted = false;
        }

        if (other.gameObject.CompareTag("CanDialogue") && LanguageManager.currentLanguage == LanguageManager.Language.Spanish && interactIndicatorSpanish != null)
        {
            interactIndicatorSpanish.SetActive(false);
            //dialogueStarted = false;
        }

        //if (other.gameObject.CompareTag("CanDialogue") && canDialogue)
        //{
        //    canDialogue = false;
        //}
    }

    #endregion Triggers

    private void CheckWalkingSound()
    {
        if (walkingSound != null)
        {
            if (!walkingSound.isPlaying)
            {
                switch (currentMinigame)
                {
                    case Minigames.Minigame1:
                        walkingSound.clip = walkingSoundsMetalOrSand[0];
                        walkingSound.Play();
                        break;
                    case Minigames.Minigame3:
                        walkingSound.clip = walkingSoundsMetalOrSand[0];
                        walkingSound.Play();
                        break;
                    case Minigames.Minigame5:
                        walkingSound.clip = walkingSoundsMetalOrSand[0];
                        walkingSound.Play();
                        break;
                    case Minigames.Minigame6:
                        walkingSound.clip = walkingSoundsMetalOrSand[1];
                        walkingSound.Play();
                        break;
                    default:
                        break;
                }
            }
            
        }
        
    }
}

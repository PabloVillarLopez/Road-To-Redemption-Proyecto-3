using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObserveObject : MonoBehaviour
{
    #region Cameras Variables
    [Header("Camera References")]
    public GameObject clueCamera;
    public GameObject playerCamera;

    #endregion Cameras Variables

    #region Clues References
    [Header("Clues References")]
    public GameObject[] clues;
    //public bool canAddRotateToButton = true;
    public RotateClue currentClue;
    public GameObject RotateUI;
    public Button leftRotateButton;
    public Button rightRotateButton;
    public int currentClueId;
    public Vector3 previousCluePosition;
    public int clueIndex;

    private bool canActivateClues;
    private bool canDeactivateClues;

    #endregion Clues References

    #region Take Clues Variables
    public static List<GameObject> TakenClues = new List<GameObject>();

    #endregion Take Clues Variables

    #region MiniGame Phases Variables
    public enum JusticePhases
    {
        INVESTIGATION,
        ANALYSIS,
        JUDGMENT
    }

    public JusticePhases phases;
    public GameObject lenseAimGlass;
    public GameObject judgementPanel;

    #endregion MiniGame Phases Variables

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        clueCamera.SetActive(false);
        playerCamera.SetActive(true);
        RotateUI.SetActive(false);
        judgementPanel.SetActive(false);
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.playerEnteredInObjectClue1Area)
        {
            Clue1CameraChange();
        }

        if (Input.GetKeyDown(KeyCode.E) && PlayerController.playerEnteredInObjectClue2Area)
        {
            Clue2CameraChange();
        }

        if (Input.GetKeyDown(KeyCode.E) && PlayerController.playerEnteredInObjectClue3Area)
        {
            Clue3CameraChange();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && clueCamera.activeInHierarchy)
        {
            CameraBackToPlayer();
        }

        HandleMinigamePhases();
    }

    #endregion Update

    #region Change from PlayerCamera to ClueCamera and from ClueCamera to PlayerCamera

    private void Clue1CameraChange()
    {
        clueCamera.SetActive(true);
        playerCamera.SetActive(false);
        clueCamera.transform.position = clues[0].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct clue

        clueIndex = 0;
        ManageUIClueType();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Clue2CameraChange()
    {
        clueCamera.SetActive(true);
        playerCamera.SetActive(false);
        clueCamera.transform.position = clues[1].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct clue
        clueIndex = 1;
        ManageUIClueType();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Clue3CameraChange()
    {
        clueCamera.SetActive(true);
        playerCamera.SetActive(false);
        clueCamera.transform.position = clues[2].transform.position + new Vector3(0, 0, -5); //Place the camera in front of the correct clue
        clueIndex = 2;
        ManageUIClueType();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CameraBackToPlayer()
    {
        clueCamera.SetActive(false);
        playerCamera.SetActive(true);
        RotateUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #endregion Change from PlayerCamera to ClueCamera and from ClueCamera to PlayerCamera

    #region Rotate Clue

    private void ManageUIClueType()
    {
        switch (clues[clueIndex].GetComponent<RotateClue>().clueType)
        {
            case RotateClue.ClueType.NOTROTATE:
                break;

            case RotateClue.ClueType.ROTATE:
                RotateUI.SetActive(true);

                if (clues[clueIndex].GetComponent<RotateClue>().canAddRotateToButton)
                {
                    leftRotateButton.onClick.RemoveAllListeners();
                    rightRotateButton.onClick.RemoveAllListeners();


                    leftRotateButton.onClick.AddListener(clues[clueIndex].GetComponent<RotateClue>().RotateLeftX);
                    rightRotateButton.onClick.AddListener(clues[clueIndex].GetComponent<RotateClue>().RotateRightX);
                    clues[clueIndex].GetComponent<RotateClue>().canAddRotateToButton = false;
                }
                break;

            default:
                break;
        }
    }

    #endregion Rotate Clue

    #region Handle Minigame Phases
    private void HandleMinigamePhases()
    {
        switch (phases)
        {
            case JusticePhases.INVESTIGATION:
                lenseAimGlass.SetActive(true);
                judgementPanel.SetActive(false);
                canDeactivateClues = true;
                if (canDeactivateClues)
                {
                    for (int i = 0; i < clues.Length; i++)
                    {
                        clues[i].SetActive(false);
                    }

                    canDeactivateClues = false;
                }

                if (AimLenseGlass.takenClues >= 3)
                {
                    phases = JusticePhases.ANALYSIS;
                }
                break;
            case JusticePhases.ANALYSIS:
                judgementPanel.SetActive(false);
                lenseAimGlass.SetActive(false);
                canActivateClues = true;
                if (canActivateClues)
                {
                    for (int i = 0; i < clues.Length; i++)
                    {
                        clues[i].SetActive(true);
                    }

                    canActivateClues = false;
                }

                break;
            case JusticePhases.JUDGMENT:
                judgementPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    #endregion Handle Minigame Phases
}

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

    #endregion Clues References

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        clueCamera.SetActive(false);
        playerCamera.SetActive(true);
        RotateUI.SetActive(false);
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
}

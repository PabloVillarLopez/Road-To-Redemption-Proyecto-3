using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserveObject : MonoBehaviour
{
    #region Cameras Variables
    [Header("Camera References")]
    public GameObject clueCamera;
    public GameObject playerCamera;

    #endregion Cameras Variables

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        clueCamera.SetActive(false);
        playerCamera.SetActive(true);
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.playerEnteredInObjectClueArea)
        {
            ClueCameraChange();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && clueCamera.activeInHierarchy)
        {
            CameraBackToPlayer();
        }
    }

    #endregion Update

    #region Change from PlayerCamera to ClueCamera and from ClueCamera to PlayerCamera

    private void ClueCameraChange()
    {
        clueCamera.SetActive(true);
        playerCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CameraBackToPlayer()
    {
        clueCamera.SetActive(false);
        playerCamera.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #endregion Change from PlayerCamera to ClueCamera and from ClueCamera to PlayerCamera

    #region Rotate Clue

    public void RotateLeft()
    {

    }

    public void RotateRight()
    {

    }

    #endregion Rotate Clue
}

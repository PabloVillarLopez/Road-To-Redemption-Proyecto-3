using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineForeground : MonoBehaviour
{
    //Script for placing pipelines in the foreground, in the first plane

    #region Camera Variables

    public GameObject playerCamera;
    public GameObject pipelineCamera;
    private bool pipelineCameraActive;
    private bool playerEnteredInPipelineArea;
    public GameObject RotatePipelineUI;

    #endregion Camera Variables

    private void Awake()
    {
        playerCamera.SetActive(true);
        pipelineCamera.SetActive(false);
        RotatePipelineUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ComeBackToPlayerCamera();

        if (Input.GetKeyDown(KeyCode.E) && playerEnteredInPipelineArea)
        {
            PipelineCamera();
        }
    }

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
    }

    #endregion Pipeline Camera

    #region RotatePipeline

    public void RotateRightX()
    {
        //gameObject.transform.localRotation += Quaternion.Euler(45f, 0, 0);
    }

    public void RotateLeftX()
    {

    }

    public void RotateUpwards()
    {

    }

    public void RotateDownwards()
    {

    }

    #endregion RotatePipeline
}

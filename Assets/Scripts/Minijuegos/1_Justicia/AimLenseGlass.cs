using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLenseGlass : MonoBehaviour
{
    #region Aim with Lense Variables
    [Header("Aim with Lense Variables")]
    public Vector3 normalPose, aimPose;
    public float aimSpeed;
    public Camera playerCam;

    #endregion Aim with Lense Variables

    #region Take clues Variable
    [Header("Take clues Variable")]
    public LayerMask Clues;
    public static int takenClues;

    #endregion Take clues Variable

    #region Show Clues Icons Variables
    [Header("Clue Icons")]
    [Space]
    public GameObject[] clueIcons;

    #endregion Show Clues Icons Variables

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < clueIcons.Length; i++)
        {
            clueIcons[i].SetActive(false);
        }    
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartLenseAim();
        }
        else
        {
            StopLenseAim();
        }


        if (transform.localPosition == aimPose)
        {
            TakeClues();
        }

    }

    #endregion Update

    #region Start Lense Aim
    private void StartLenseAim()
    {
        transform.localPosition = Vector3.Slerp(transform.localPosition, aimPose, aimSpeed * Time.deltaTime);
        playerCam.fieldOfView -= 40 * Time.deltaTime;
        playerCam.fieldOfView = Mathf.Clamp(playerCam.fieldOfView, 30, 60);
    }

    #endregion Start Lense Aim

    #region Stop Lense Aim
    private void StopLenseAim()
    {
        transform.localPosition = Vector3.Slerp(transform.localPosition, normalPose, aimSpeed * Time.deltaTime);
        playerCam.fieldOfView += 40 * Time.deltaTime;
        playerCam.fieldOfView = Mathf.Clamp(playerCam.fieldOfView, 30, 60);
    }

    #endregion Stop Lense Aim

    #region Take Clues
    private void TakeClues()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, Clues))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            ObserveObject.TakenClues.Add(hit.transform.gameObject);
            hit.transform.gameObject.SetActive(false);
            clueIcons[takenClues].SetActive(true);
            takenClues++;
        }
    }

    #endregion Take Clues
}

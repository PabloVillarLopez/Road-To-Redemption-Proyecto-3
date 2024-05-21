using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateClue : MonoBehaviour
{
    #region Angle to Rotate in Y axis

    private float angleAddedY;

    #endregion Angle to Rotate in Y axis

    public enum ClueType
    {
        NOTROTATE,
        ROTATE
    }

    public ClueType clueType;

    public int id;
    public bool canAddRotateToButton = true;

    public Vector3 correctRotation { get; private set; }
    public bool clueIsInCorrectRotation;

    public GameObject analyzeButton;
    public GameObject leftArrow;
    public GameObject rightArrow;

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        angleAddedY = transform.rotation.y;
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion Update

    #region Rotate Right

    public void RotateRightX()
    {
        angleAddedY += 45f;
        transform.rotation = Quaternion.Euler(transform.rotation.x, angleAddedY, transform.rotation.z);
    }

    #endregion Rotate Right

    #region Rotate Left

    public void RotateLeftX()
    {
        angleAddedY -= 45f;
        transform.rotation = Quaternion.Euler(transform.rotation.x, angleAddedY, transform.rotation.z);
    }

    #endregion Rotate Left

    private void CheckCorrectRotation()
    {
        if (transform.eulerAngles == correctRotation)
        {
            analyzeButton.SetActive(true);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
        }
    }
}

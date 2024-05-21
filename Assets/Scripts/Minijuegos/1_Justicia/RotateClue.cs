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

    public Vector3 correctRotation;
    public Vector3 correctRotation2;
    //public Vector3 CorrectRotation { get { return correctRotation; } set { correctRotation = value;} }

    public bool clueIsInCorrectRotation;

    public GameObject analyzeButton;
    public GameObject analyzeSlider;
    public GameObject leftArrow;
    public GameObject rightArrow;

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        angleAddedY = transform.eulerAngles.y;
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

        if (angleAddedY == 360)
        {
            angleAddedY = 0;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleAddedY, transform.eulerAngles.z);
        CheckCorrectRotation();
        Debug.Log(transform.eulerAngles);
        Debug.Log(angleAddedY);
    }

    #endregion Rotate Right

    #region Rotate Left

    public void RotateLeftX()
    {
        angleAddedY -= 45f;

        if (angleAddedY == 360)
        {
            angleAddedY = 0;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleAddedY, transform.eulerAngles.z);
        CheckCorrectRotation();
        Debug.Log(transform.eulerAngles);
        Debug.Log(angleAddedY);
    }

    #endregion Rotate Left

    private void CheckCorrectRotation()
    {
        if (transform.eulerAngles == correctRotation || angleAddedY == correctRotation.y)
        {
            clueIsInCorrectRotation = true;
            angleAddedY = 0;
            analyzeButton.SetActive(true);
            analyzeSlider.SetActive(true);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
        }
    }
}

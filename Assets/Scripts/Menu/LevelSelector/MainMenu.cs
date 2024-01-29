using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // List of predefined pivot points
    private Vector3[] pivotPoints = new Vector3[]
    {
        new Vector3(10f, 10f, 100f),
        new Vector3(1f, 10f, 1f),
        new Vector3(1f, 1f, 0f),
        new Vector3(1f, 1f, -1f),
        new Vector3(0f, 1f, -1f),
        new Vector3(-1f, 1f, -10f),
        new Vector3(-1f, 1f, 0f),
        new Vector3(-1f, 1f, 1f)
    };

    // Index of the current pivot point
    private int currentPivotPointIndex = 0;

    // Rotation speed
    public float rotationSpeed = 2f;

    // Reference to the text that will display the current pivot point
    public Text pivotPointText;

    void Update()
    {
        // Check if the 'D' key is being pressed
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Increment the index of the current pivot point
            currentPivotPointIndex = (currentPivotPointIndex + 1) % pivotPoints.Length;

            // Display the text with the current pivot point if the text is not null
            if (pivotPointText != null)
            {
                DisplayPivotPointText();
            }
        }

        // Check if the 'A' key is being pressed
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Decrement the index of the current pivot point
            currentPivotPointIndex = (currentPivotPointIndex - 1 + pivotPoints.Length) % pivotPoints.Length;

            // Display the text with the current pivot point if the text is not null
            if (pivotPointText != null)
            {
                DisplayPivotPointText();
            }
        }

        // Ensure the index stays within bounds
        if (currentPivotPointIndex < 0)
        {
            currentPivotPointIndex = pivotPoints.Length - 1;
        }
        else if (currentPivotPointIndex >= pivotPoints.Length)
        {
            currentPivotPointIndex = 0;
        }

        // Get the current pivot point
        Vector3 targetPivotPoint = pivotPoints[currentPivotPointIndex];

        // Smoothly rotate the object towards the new pivot point
        Quaternion targetRotation = Quaternion.LookRotation(targetPivotPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void DisplayPivotPointText()
    {
        // Display the text with the current pivot point
        pivotPointText.text = "Activity: " + pivotPoints[currentPivotPointIndex];
    }
}

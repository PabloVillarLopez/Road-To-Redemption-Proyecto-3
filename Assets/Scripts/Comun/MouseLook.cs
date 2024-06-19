using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Public variables
    public float Sensitivity = 700;
    public Transform playerBody;
    public Transform orientation;
    public Transform player;
    public float smoothing = 0.7f; // Smoothing factor for rotation

    // Private variables
    private float xRotation = 0f;
    private float yRotation = 0f;

    public static bool canLook = true;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start method, called at the beginning
    private void Start()
    {
        
    }

    // Update method, called every frame
    void Update()
    {
        if (canLook)
        {
            // Get mouse input on X and Y axes
            float mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;

            // Smoothing mouse movement on X and Y axes
            xRotation = Mathf.Lerp(xRotation, xRotation - mouseY, smoothing);
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            // Add rotation on Y axis
            yRotation += mouseX * smoothing;

            // Apply smoothed rotation on both axes to the player's body
            playerBody.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

            //Apply rotation also in orientation
            orientation.localRotation = Quaternion.Euler(0, yRotation, 0);

            //Rotate also the player body
            player.localRotation = Quaternion.Euler(0, yRotation, 0);
        }
        
    }
}

using UnityEngine;

public class BasicCharacterController : MonoBehaviour
{
    // Public variables
    public float speed = 5f; // Movement speed
    public Transform target;
    public Transform character;
    public GameObject player;

    // Private variables
    private CharacterController controller; // Reference to the CharacterController

    // Start method, called at the beginning
    void Start()
    {
        controller = GetComponent<CharacterController>(); // Get the CharacterController component
    }

    // Update method, called every frame
    void Update()
    {
        // Get horizontal and vertical movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction based on input and speed
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized * speed;

        // Apply movement to the CharacterController
        controller.Move(moveDirection * Time.deltaTime);
    }
}

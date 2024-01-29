using UnityEngine;

public class BasicCharacterController : MonoBehaviour
{
    // Public variables
    public float speed = 5f; // Movement speed
    public Transform character; // Personaje
    public GameObject player; // Jugador

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
        // Calculate movement direction based on mouse position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 moveDirection = mousePosition - character.position;
        moveDirection.y = 0f; // Ensure movement is only in the horizontal plane
        moveDirection.Normalize(); // Normalize the direction vector

        // Apply movement to the CharacterController
        controller.Move(moveDirection * speed * Time.deltaTime);
    }
}

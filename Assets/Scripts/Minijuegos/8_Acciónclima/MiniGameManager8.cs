using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager8 : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // Array of objects to spawn
    public float spawnInterval = 6f; // Time interval between spawns
    public Transform spawnPoint; // Spawn point
    public Transform spawnPointEmpty;

    public float moveObjSpeed = 5f; // Object movement speed
    public float velocity = 10f;
    public PlayerControllerCursor player; // Reference to the player script

    public Transform objectToMove; // Object to move
    public Vector3 pointA; // Point A
    public Vector3 pointB; // Point B
    public float moveSpeed = 5f; // Object movement speed
    private bool move = true;
    private Vector3 destination;

    public Text textPoints;
    private int points;

    public float totalTime = 5f; // 5 minutos en segundos
    private float timeRemaining;
    public Text timerText;
    public bool monitoring;

    void Start()
    {
        // Initialization goes here
    }

    private void Update()
    {
        // If player is monitoring
        if (player.monitoring)
        {
            MoveObject();
            UpdateTimer();
        }
    }

    void SpawnObject()
    {
        // Generate a random index based on the length of the objectsToSpawn array
        int randomIndex = Random.Range(0, objectsToSpawn.Length);

        // Instantiate the selected prefab object at the spawn point
        GameObject spawnedObject = Instantiate(objectsToSpawn[randomIndex], spawnPointEmpty.transform.position, Quaternion.identity);

    
        

        
        spawnedObject.GetComponent<TrashInfo>().AddForce();

        // Optionally: Set a random rotation for the spawned object
        spawnedObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }


    private void MoveObject()
    {
        // Calculate displacement on the X-axis based on keyboard input
        float movementX = player.horizontalInput * moveObjSpeed * Time.deltaTime;

        // Calculate the new position of the object on the X-axis applying minimum and maximum limits
        float newXPosition = Mathf.Clamp(objectToMove.transform.position.x + movementX, -9.42f, -0.57f);

        // Set the position of the object on the X-axis
        objectToMove.transform.position = new Vector3(newXPosition, objectToMove.transform.position.y, objectToMove.transform.position.z);
    }

    IEnumerator MoveObjectRoutine()
    {
        while (move) // Infinite loop for continuous movement
        {
            // Move the object towards the destination
            while (spawnPoint.transform.position != destination)
            {
                spawnPoint.transform.position = Vector3.MoveTowards(spawnPoint.transform.position, destination, velocity * Time.deltaTime);
                yield return null;
            }

            // Change the destination to the opposite point
            if (destination == pointA)
                destination = pointB;
            else
                destination = pointA;
        }
    }

    public void StartGame()
    {
        monitoring = true;
        if (monitoring==true)
        {
           InvokeRepeating("SpawnObject", 0f, spawnInterval);
            destination = pointB;
            StartCoroutine(MoveObjectRoutine());
            StartTimer();
        }
    }

    public void AddPoints()
    {
        points=points +5; 
        UpdatePointText(); 
    }

    public void SubtractPoints()
    {
        points = points - 1; 
        UpdatePointText(); 
    }

    void UpdatePointText()
    {
        if (textPoints != null)
        {
            textPoints.text = "Points: " + points.ToString(); 
        }
        
    }

    public void StartTimer()
    {
        timeRemaining = totalTime;
    }

    // Función para actualizar el temporizador
    public void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = "Time: " + timerString;
        }
        else
        {
            timerText.text = "Time's up!";
            player.RevertToPreviousCamera();
            player.monitoring = false;
            monitoring = false;
            StopAllCoroutines();
            CancelInvoke("SpawnObject");
            move = false;
        }
    }
}

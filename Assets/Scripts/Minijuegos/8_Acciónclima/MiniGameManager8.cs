using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Image overlay;
    public float totalTime = 5f; // 5 minutos en segundos
    private float timeRemaining;
    public Text timerText;
    public bool monitoring;
    private DialogueScript dialogue;

    public GameObject panel;
    public TMPro.TextMeshProUGUI text;
    public int lightmapIndexToUse; // Índice del baked lightmap que deseas que se utilice

    public Image interact;

    void Start()
    {
        // Initialization goes here
        overlay.gameObject.SetActive(false);
        dialogue = GetComponent<DialogueScript>();
        ApplyLightmapToScene();
        activeInteract(false);

    }

    private void Update()
    {
        // If player is monitoring
        if (player.monitoring)
        {
            MoveObject();
            overlay.gameObject.SetActive(true);
            UpdateTimer();
            textPoints.gameObject.SetActive(true);
        }
        else 
        {
            overlay.gameObject.SetActive(false);
            textPoints.gameObject.SetActive(false);
        }
    }

    void SpawnObject()
    {
        // Generate a random index based on the length of the objectsToSpawn array
        int randomIndex = Random.Range(0, objectsToSpawn.Length);

        // Generate a random rotation
        Quaternion randomRotation = Quaternion.Euler(
            Random.Range(0f, 360f), // Random rotation around the x-axis
            Random.Range(0f, 360f), // Random rotation around the y-axis
            Random.Range(0f, 360f)  // Random rotation around the z-axis
        );

        // Instantiate the selected prefab object at the spawn point with the random rotation
        GameObject spawnedObject = Instantiate(objectsToSpawn[randomIndex], spawnPointEmpty.transform.position, randomRotation);

        spawnedObject.GetComponent<TrashInfo>().AddForce();

        // Optionally: If you only want random rotation around the y-axis, use this instead
        // spawnedObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }



    private void MoveObject()
    {
        // Calculate displacement on the X-axis based on keyboard input
        float movementZ = player.horizontalInput * moveObjSpeed * Time.deltaTime;

        // Calculate the new position of the object on the X-axis applying minimum and maximum limits
        float newZPosition = Mathf.Clamp(objectToMove.transform.position.z + movementZ, -1.946f, 1.984f);

        // Set the position of the object on the X-axis
        objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y, newZPosition);
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
            dialogue.spanishLines = new string[] { "Excelente trabajo, después de toda esta basura que has reciclado mientras solucionabamos la averia no hemos contaminado la capa de ozono buen trabajo.\r\n" };
            dialogue.dialoguePanel = panel;
            dialogue.dialogueText = text;
            dialogue.StartSpanishDialogue();
            move = false;

            Invoke("ChangeSceneMain", 7f);
        }
    }


    private void ChangeSceneMain()
    {
        MinigamesCompleted.minigame8Finished = true;
        SceneManager.LoadScene("LevelSelector");
    }



    void ApplyLightmapToScene()
    {
        // Obtener el número total de lightmaps
        int totalLightmaps = LightmapSettings.lightmaps.Length;

        // Verificar si el índice del baked lightmap es válido
        if (lightmapIndexToUse >= 0 && lightmapIndexToUse < totalLightmaps)
        {
            // Crear un arreglo de LightmapData para asignar el lightmap a toda la escena
            LightmapData[] lightmaps = new LightmapData[totalLightmaps];

            // Asignar el baked lightmap a todas las entradas del arreglo de LightmapData
            for (int i = 0; i < totalLightmaps; i++)
            {
                lightmaps[i] = new LightmapData();
                lightmaps[i].lightmapColor = LightmapSettings.lightmaps[i].lightmapColor;
                lightmaps[i].lightmapDir = LightmapSettings.lightmaps[i].lightmapDir;
            }

            // Asignar el lightmap seleccionado a toda la escena
            LightmapSettings.lightmaps = lightmaps;

            Debug.Log("Baked lightmap asignado a toda la escena.");
        }
        else
        {
            Debug.LogWarning("El índice del baked lightmap no es válido.");
        }
    }


    public void activeInteract(bool active)
    {
        if (active)
        {
            interact.gameObject.SetActive(true);
        }
        else
        {
            interact.gameObject.SetActive(false);
        }



    }

}

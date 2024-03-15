using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class MiniGameManager7 : MonoBehaviour
{
    // GameObjects
    public GameObject[] objectsToSpawn;
    public GameObject[] ManageGameObjects;
    public GameObject wallToSpawn;

    // Vectors and Positions
    public Vector3 spawnArea;
    private Vector3 randomPosition;

    // Phase Management
    public int phasesProcess = 1;
    public int currentPhase = 1;

    // Counters and Progress
    private int counterGameObject = 0;
    private int countItems;
    public int countProgress = 30;
    public Text counterProgressText;
    private int counterRepare;
    // Dimensions and Sizes
    public float large;
    public float hight; // Note: Consider correcting the spelling to "height"

    // Zone Coordinates
    private float xMaxZone1;
    private float xMinZone1;
    private float zMaxZone1;
    private float zMinZone1;
    public float zone1Large;
    public float zone1Height;

    private float zMaxZone2;
    private float zMinZone2;
    private float xMaxZone2;
    private float xMinZone2;

    private float zMaxZone3;
    private float zMinZone3;
    private float xMaxZone3;
    private float xMinZone3;

    // Spawn Control
    private bool readyToSpawn;

    // Planting Process
    private float proceso = 0f;
    public bool isPlanting;
    public float plantingTime = 3f;
    public Slider progressBar;
    private float plantingTimer = 3f;

    #region MonoBehaviour Callbacks

    void Start()
    {
        GenerateArea();
        Phases(phasesProcess);
    }

    private void Update()
    {
        if (phasesProcess != currentPhase)
        {
            phasesProcess = currentPhase;
            Phases(phasesProcess);
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject character = GameObject.Find("Character");
            Vector3 vectorToCheck = character.transform.position; // Get character's position

            bool isInsideZones = IsVectorInsideZones(vectorToCheck); // Check if the position is inside the zones
            if (isInsideZones)
            {
                Debug.Log("The vector is inside the zones.");
            }
            else
            {
                Debug.Log(vectorToCheck);
                Debug.Log("The vector is not inside the zones.");
            }
        }

        if (currentPhase == 2)
        {
            if (isPlanting)
            {
                progressBar.gameObject.SetActive(true); // Activate progress bar
            }
            else
            {
                plantingTimer = 0f; // Reset planting timer
                progressBar.gameObject.SetActive(false);
            }

            UpdatePlantingProcess();
        }
    }

    #endregion

    #region Object Spawning

    void SpawnObject()
    {
        if (counterGameObject >= 0 && counterGameObject <= 10)
        {
            Vector3 areaToSpawn = GeneratePointToSpawn(1);
            GameObject objectToSpawn = objectsToSpawn[1];
            // Spawn the object at the generated position
            GameObject spawnedObject = Instantiate(objectToSpawn, areaToSpawn, Quaternion.identity);
            spawnedObject.SetActive(true); // Ensure the object is active
            ManageGameObjects[counterGameObject] = spawnedObject;
            ObjectToExplote objectToExploteComponent = spawnedObject.GetComponent<ObjectToExplote>();
            objectToExploteComponent.GetComponent<Renderer>().material = objectToExploteComponent.materials[0];
            counterGameObject++;
        }
        else if (counterGameObject > 10 && counterGameObject <= 20)
        {
            Vector3 areaToSpawn = GeneratePointToSpawn(2);
            GameObject objectToSpawn = objectsToSpawn[1];
            if (readyToSpawn)
            {
                // Spawn the object at the generated position
                GameObject spawnedObject = Instantiate(objectToSpawn, areaToSpawn, Quaternion.identity);
                spawnedObject.SetActive(true); // Ensure the object is active
                objectToSpawn.GetComponent<ObjectToExplote>().GetComponent<Renderer>().material = objectToSpawn.GetComponent<ObjectToExplote>().materials[1];
                ManageGameObjects[counterGameObject] = spawnedObject;
                counterGameObject++;
                readyToSpawn = false;
            }
            else
            {
                SpawnObject();
            }
        }
        else if (counterGameObject > 20 && counterGameObject <= 30)
        {
            readyToSpawn = true;
            if (readyToSpawn)
            {
                Vector3 areaToSpawn = GeneratePointToSpawn(3);
                GameObject objectToSpawn = objectsToSpawn[1];
                // Spawn the object at the generated position
                GameObject spawnedObject = Instantiate(objectToSpawn, areaToSpawn, Quaternion.identity);
                spawnedObject.SetActive(true); // Ensure the object is active
                objectToSpawn.GetComponent<ObjectToExplote>().GetComponent<Renderer>().material = objectToSpawn.GetComponent<ObjectToExplote>().materials[2];
                ManageGameObjects[counterGameObject] = spawnedObject;
                counterGameObject++;
                readyToSpawn = false;
            }
            else
            {
                SpawnObject();
            }
        }
    }

    #endregion

    #region Object Spawning Utilities

    private Vector3 GeneratePointToSpawn(float zone)
    {
        Vector3 randomPosition = Vector3.zero;
        switch (zone)
        {
            case 1:
                randomPosition = new Vector3(
                    Random.Range(xMinZone1, xMaxZone1),
                    0f,
                    Random.Range(zMinZone1, zMaxZone1)
                );
                break;
            case 2:
                for (int attemptsZone2 = 0; attemptsZone2 < 100; attemptsZone2++)
                {
                    randomPosition = new Vector3(
                        Random.Range(xMinZone2, xMaxZone2),
                        0f,
                        Random.Range(zMinZone2, zMaxZone2)
                    );
                    if (randomPosition.x >= xMinZone1 && randomPosition.x <= xMaxZone1 &&
                        randomPosition.z >= zMinZone1 && randomPosition.z <= zMaxZone1 && randomPosition.x >= xMinZone3)
                    {
                        continue;
                    }
                    else
                    {
                        readyToSpawn = true;
                        break;
                    }
                }
                break;
            case 3:
                for (int attemptsZone3 = 0; attemptsZone3 < 100; attemptsZone3++)
                {
                    randomPosition = new Vector3(
                        Random.Range(xMinZone3, xMaxZone3),
                        0f,
                        Random.Range(zMinZone3, zMaxZone3)
                    );
                    if (randomPosition.x >= xMinZone1 && randomPosition.x <= xMaxZone1 &&
                        randomPosition.z >= zMinZone1 && randomPosition.z <= zMaxZone1 && randomPosition.x <= xMaxZone2)
                    {
                        continue;
                    }
                    else
                    {
                        readyToSpawn = true;
                        break;
                    }
                }
                break;
        }
        return randomPosition;
    }

    private void GenerateArea()
    {
        GameObject character = GameObject.Find("Character");
        Vector3 posCharacter = character.transform.position;
        hight = posCharacter.z + hight;

        // Set valid points in zone 1
        xMinZone1 = posCharacter.x - zone1Large;
        xMaxZone1 = posCharacter.x + zone1Large;
        zMinZone1 = (large / 2f) - zone1Height;
        zMaxZone1 = (large / 2f) + zone1Height;

        xMinZone2 = posCharacter.x - large;
        xMaxZone2 = posCharacter.x;
        zMinZone2 = posCharacter.z;
        zMaxZone2 = posCharacter.z + hight;

        xMinZone3 = posCharacter.x;
        xMaxZone3 = posCharacter.x + large;
        zMinZone3 = posCharacter.z;
        zMaxZone3 = posCharacter.z + hight;
    }

    #endregion

    #region Phase Management

    private void Phases(int phasesProcess)
    {
        progressBar.gameObject.SetActive(false);
        switch (phasesProcess)
        {
            case 1:
                Debug.Log("Starting Phase 1");
                for (int i = 0; i < 30; i++)
                {
                    SpawnObject();
                }
                for (int x = 0; x < ManageGameObjects.Length; x++)
                {
                    ManageGameObjects[x].SetActive(true);
                }
                break;
            case 2:
                Debug.Log("Starting Phase 2");
                counterProgressText.text = 0.ToString();
                for (int x = 0; x < ManageGameObjects.Length; x++)
                {
                    Debug.Log(ManageGameObjects[x]);
                    ManageGameObjects[x].SetActive(false);
                }
                break;
            case 3:
                Debug.Log("Starting Phase 3");
                ManageGameObjects = null;
                counterProgressText.text = counterRepare.ToString();

                for (int x = 0; x < 5; x++)
                {

                    // Calculate spawn position by adding offset to spawner's position
                    Vector3 spawnPosition = transform.position + new Vector3(10f * x, 0, 0);
                    // Instantiate the object at the calculated position
                    GameObject newWall = Instantiate(wallToSpawn, spawnPosition, Quaternion.identity);
                    // Get a random number between 0 and 3
                    int randomNumber = Random.Range(0, 4);
                    // Access the first child of the instantiated object
                    GameObject childObject = newWall.transform.GetChild(randomNumber).gameObject;
                    float randomFloat = Random.Range(1f, 3f);
                    childObject.GetComponent<ApplyMaterial>().SetHeight(randomFloat);
                }
                break;
            default:
                break;
        }
    }

    #endregion

    #region Progress Update

    public void updateProgressText(int progress)
    {
        countProgress = countProgress + progress;
        counterProgressText.text = countProgress.ToString();
        if (countProgress <= 0)
        {
            Phases(2);
        }
    }


    public void updateProgressRepareText()
    {
        counterRepare++;
        counterProgressText.text = counterRepare.ToString();


    }

    #endregion

    #region Zone Check

    public bool IsVectorInsideZones(Vector3 vector)
    {
        // Check if the vector is inside zone 1
        if (vector.x >= xMinZone1 && vector.x <= xMaxZone1 &&
            vector.z >= zMinZone1 && vector.z <= zMaxZone1)
        {
            Debug.Log("Zone 1");
            return true; // Vector is inside zone 1
        }
        // Check if the vector is inside zone 2
        if (vector.x >= xMinZone2 && vector.x <= xMaxZone2 &&
            vector.z >= zMinZone2 && vector.z <= zMaxZone2)
        {
            Debug.Log("Zone 2");
            return true; // Vector is inside zone 2
        }
        // Check if the vector is inside zone 3
        if (vector.x >= xMinZone3 && vector.x <= xMaxZone3 &&
            vector.z >= zMinZone3 && vector.z <= zMaxZone3)
        {
            Debug.Log("Zone 3");
            return true; // Vector is inside zone 3
        }
        // Vector is not inside any zone
        return false;
    }

    #endregion

    #region Planting Progress

    public void UpdateProgressPlanting()
    {
        proceso += 0.2f;
    }

    void UpdateProgress(float progress)
    {
        progressBar.value = Mathf.Clamp01(progress);
    }

    void UpdatePlantingProcess()
    {
        if (isPlanting)
        {
            plantingTimer += Time.deltaTime;
            UpdateProgress(plantingTimer / plantingTime);
            if (plantingTimer >= plantingTime)
            {
                FinishPlanting();
            }
        }
    }

    void FinishPlanting()
    {
        isPlanting = false;
        progressBar.value = 0f;
        progressBar.gameObject.SetActive(false);
    }

    #endregion
}

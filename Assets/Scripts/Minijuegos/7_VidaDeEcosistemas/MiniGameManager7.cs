using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

using static UnityEditor.VersionControl.Asset;

public class MiniGameManager7 : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public Vector3 spawnArea;
    public int phasesProcess=1;
    public GameObject[] ManageGameObjects;
    private int counterGameObject = 0;
    private int countItems;
    public int countProgress = 30;
    public int currentPhase = 1;
    public Text counterProgressText;
    private Vector3 randomPosition;

    public float large;
    public float hight;

    private float xMaxZone1;
    private float xMinZone1;
    private float zMaxZone1;
    private float zMinZone1;
    public float zone1Large;
    public float zone1Hight;

    private float zMaxZone2;
    private float zMinZone2;
    private float xMaxZone2;
    private float xMinZone2;

    private float zMaxZone3;
    private float zMinZone3;
    private float xMaxZone3;
    private float xMinZone3;

    private bool readyToSpawn;



    private float proceso = 0f;
    public bool isPlanting;
    public float plantingTime = 3f;
    public Slider progressBar;
    private float plantingTimer = 3f;

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
            Vector3 vectorToCheck = character.transform.position; // Obtener la posición del personaje

            bool isInsideZones = IsVectorInsideZones(vectorToCheck); // Verificar si la posición está dentro de las zonas
            if (isInsideZones)
            {

                Debug.Log("El vector está dentro de las zonas.");
            }
            else
            {
                Debug.Log(vectorToCheck);
                Debug.Log("El vector no está dentro de las zonas.");
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


    void SpawnObject()
    {
        if (counterGameObject >= 0 && counterGameObject <= 10)
        {
            Vector3 areaToSpawn = GeneratePointToSpawn(1);

            GameObject objectToSpawn = objectsToSpawn[1];
            // Spawnear el objeto en la posición generada
            GameObject spawnedObject = Instantiate(objectToSpawn, areaToSpawn, Quaternion.identity);
            spawnedObject.SetActive(true); // Asegurarse de que el objeto esté activo

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
                // Spawnear el objeto en la posición generada
                GameObject spawnedObject = Instantiate(objectToSpawn, areaToSpawn, Quaternion.identity);
                spawnedObject.SetActive(true); // Asegurarse de que el objeto esté activo

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

                // Spawnear el objeto en la posición generada
                GameObject spawnedObject = Instantiate(objectToSpawn, areaToSpawn, Quaternion.identity);
                spawnedObject.SetActive(true); // Asegurarse de que el objeto esté activo

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
                        continue; // Continuar con el siguiente intento si la posici�n est� dentro de la zona 1
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

        // Set points valid in zone 1
        xMinZone1 = posCharacter.x - zone1Large;
        xMaxZone1 = posCharacter.x + zone1Large;

        zMinZone1 = (large / 2f) - zone1Hight;
        zMaxZone1 = (large / 2f) + zone1Hight;

        xMinZone2 = posCharacter.x - large;
        xMaxZone2 = posCharacter.x;

        zMinZone2 = posCharacter.z;
        zMaxZone2 = posCharacter.z + hight;

        xMinZone3 = posCharacter.x ;
        xMaxZone3 = posCharacter.x + large;

        zMinZone3 = posCharacter.z;
        zMaxZone3 = posCharacter.z + hight;
    }
    private void Phases(int phasesProcess)
    {
        progressBar.gameObject.SetActive(false);

        switch (phasesProcess)
        {

            case 1:
                Debug.Log("Iniciando Fase 1");
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

                Debug.Log("Iniciando Fase 2");
                counterProgressText.text = 0.ToString();
                
                for (int x = 0; x < ManageGameObjects.Length; x++)
                {
                    Debug.Log(ManageGameObjects[x]);
                    ManageGameObjects[x].SetActive(false);



                }
                break;
            default:
                break;
        }
    }

    public void updateProgressText(int progress)
    {
        countProgress = countProgress + progress;
        counterProgressText.text = countProgress.ToString();

        if(countProgress <=0) {

            Phases(2);

        }

    }


    public bool IsVectorInsideZones(Vector3 vector)
    {
        // Verificar si el vector está dentro de la zona 1
        if (vector.x >= xMinZone1 && vector.x <= xMaxZone1 &&
            vector.z >= zMinZone1 && vector.z <= zMaxZone1)
        {
            Debug.Log("Zona 1");
            return true; // El vector está dentro de la zona 1
        }

        // Verificar si el vector está dentro de la zona 2
        if (vector.x >= xMinZone2 && vector.x <= xMaxZone2 &&
            vector.z >= zMinZone2 && vector.z <= zMaxZone2)
        {
            Debug.Log("Zona 2");
            return true; // El vector está dentro de la zona 2
        }

        // Verificar si el vector está dentro de la zona 3
        if (vector.x >= xMinZone3 && vector.x <= xMaxZone3 &&
            vector.z >= zMinZone3 && vector.z <= zMaxZone3)
        {
            Debug.Log("Zona 3");
            return true; // El vector está dentro de la zona 3
        }

        // El vector no está dentro de ninguna zona
        return false;


    }

    public void UpdateProgressPlanting()
    {
        proceso += 0.2f;

    }


    void StartPlanting()
    {
        isPlanting = true;
        plantingTimer = 0f;
        progressBar.gameObject.SetActive(true);
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
}

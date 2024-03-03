using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager7 : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public Vector3 spawnArea;
    public int phasesProcess;
    public GameObject[] ManageGameObjects;
    private int counterGameObject = 0;
    private int countItems;
    public int countProgress = 15;
    public int currentPhase = 1;

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
    void Start()
    {
        GenerateArea();
        Phases(phasesProcess);
        Debug.Log("large: " + large);
        Debug.Log("hight: " + hight);

        Debug.Log("xMaxZone1: " + xMaxZone1);
        Debug.Log("xMinZone1: " + xMinZone1);
        Debug.Log("zMaxZone1: " + zMaxZone1);
        Debug.Log("zMinZone1: " + zMinZone1);
        Debug.Log("zone1Large: " + zone1Large);
        Debug.Log("zone1Hight: " + zone1Hight);

        Debug.Log("zMaxZone2: " + zMaxZone2);
        Debug.Log("zMinZone2: " + zMinZone2);
        Debug.Log("xMaxZone2: " + xMaxZone2);
        Debug.Log("xMinZone2: " + xMinZone2);

        Debug.Log("zMaxZone3: " + zMaxZone3);
        Debug.Log("zMinZone3: " + zMinZone3);
        Debug.Log("xMaxZone3: " + xMaxZone3);
        Debug.Log("xMinZone3: " + xMinZone3);
    }

    private void Update()
    {
        if (phasesProcess != currentPhase)
        {
            phasesProcess = currentPhase;
            Phases(2);
        }
    }

    void SpawnObject()
    {
        if (counterGameObject >= 0 && counterGameObject <= 10)
        {
            Vector3 areaToSpawn = GeneratePointToSpawn(1);

           
                GameObject objectToSpawn = objectsToSpawn[0];
                // Spawnear el objeto en la posición generada
                Instantiate(objectToSpawn, areaToSpawn, Quaternion.identity);



                ManageGameObjects[counterGameObject] = objectToSpawn;
                objectToSpawn.GetComponent<ObjectInfo>().renderer.material = objectToSpawn.GetComponent<ObjectInfo>().materials[0];
            objectToSpawn.GetComponent<ObjectInfo>().Tipo = "1";

            Debug.Log(areaToSpawn.ToString());  
            counterGameObject++;

          
        }
        else if (counterGameObject > 10 && counterGameObject <= 20)
        {
            Vector3 areaToSpawn = GeneratePointToSpawn(2);
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

            if (readyToSpawn)
            {


                // Spawnear el objeto en la posición generada
                Instantiate(objectToSpawn, areaToSpawn, Quaternion.identity);
            objectToSpawn.GetComponent<ObjectInfo>().renderer.material = objectToSpawn.GetComponent<ObjectInfo>().materials[1];
            ManageGameObjects[counterGameObject] = objectToSpawn;
                objectToSpawn.GetComponent<ObjectInfo>().Tipo = "2";
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
                GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

                // Spawnear el objeto en la posición generada
                Instantiate(objectToSpawn, areaToSpawn, Quaternion.identity);
                objectToSpawn.GetComponent<ObjectInfo>().renderer.material = objectToSpawn.GetComponent<ObjectInfo>().materials[2];
                ManageGameObjects[counterGameObject] = objectToSpawn;
                objectToSpawn.GetComponent<ObjectInfo>().Tipo = "3";
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
                        continue; // Continuar con el siguiente intento si la posición está dentro de la zona 1
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
        switch (phasesProcess)
        {
            case 1:
                for (int i = 0; i < 30; i++)
                {
                    
                   
                        SpawnObject();
                        

                    
                }
                break;
            case 2:
                for (int x = 0; x < ManageGameObjects.Length; x++)
                {
                    if (ManageGameObjects[x].activeSelf == true)
                    {
                        ManageGameObjects[x].SetActive(false);
                        Debug.Log(ManageGameObjects[x].name);
                    }
                }
                break;
            default:
                break;
        }
    }
}

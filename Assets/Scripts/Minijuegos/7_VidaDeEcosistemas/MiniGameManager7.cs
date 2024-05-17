using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.Rendering.DebugUI;
using Text = UnityEngine.UI.Text;

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
    public GameObject player;
    // Counters and Progress
    private int counterGameObject = 0;
    private int countItems;
    public int countProgress = 30;
    public Text counterProgressText;
    private int counterRepare;
    public bool haveSeeds;
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

    private DialogueScript dialogue;
    public TMPro.TextMeshProUGUI text;
    public GameObject panel;
    #region MonoBehaviour Callbacks


    public GameObject[] scrapt = new GameObject[5];
    private int maxScraps = 0;
    private int maxRefText = 0;
    public GameObject[] plantTargets = new GameObject[5];
    private int maxPlants = 0;
    public GameObject pistol;

    public GameObject[] wallsGood;
    public GameObject[] wallsBad;
    public Camera secondCamera;
    public Camera mainCam;
    public int lightmapIndexToUse; // Índice del baked lightmap que deseas que se utilice

    public GameObject[] tress;


    private void Awake()
    {
        
    }
    void Start()
    {
        maxScraps = scrapt.Length;
        maxPlants = plantTargets.Length;
        maxRefText = maxScraps;
        GenerateArea();
        Phases(phasesProcess);
        dialogue = GetComponent<DialogueScript>();
        ManagerPlant();
        manageWalls();
        secondCamera.enabled = false;
        mainCam = Camera.main;

        foreach (GameObject obj in wallsBad)
        {
            obj.SetActive(true);

        }
    }

    private void Update()
    {
        //if (phasesProcess != currentPhase)
        //{
        //    phasesProcess = currentPhase;
        //    Phases(phasesProcess);
        //}



        if (currentPhase == 2)
        {
            if (isPlanting)
            {
                progressBar.gameObject.SetActive(true); // Activate progress bar
                UpdateProgressPlanting();
            }
            else
            {
                plantingTimer = 0f; // Reset planting timer
                progressBar.gameObject.SetActive(false);
                
            }

            
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
                    UnityEngine.Random.Range(xMinZone1, xMaxZone1),
                    0f,
                    UnityEngine.Random.Range(zMinZone1, zMaxZone1)
                );
                break;
            case 2:
                for (int attemptsZone2 = 0; attemptsZone2 < 100; attemptsZone2++)
                {
                    randomPosition = new Vector3(
                        UnityEngine.Random.Range(xMinZone2, xMaxZone2),
                        0f,
                        UnityEngine.Random.Range(zMinZone2, zMaxZone2)
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
                        UnityEngine.Random.Range(xMinZone3, xMaxZone3),
                        0f,
                        UnityEngine.Random.Range(zMinZone3, zMaxZone3)
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
                
                countProgress = scrapt.Length;
                break;
            case 2:
                currentPhase = 2;
                reminders(0);
                pistol.SetActive(false);
                ManagerPlant();
                Debug.Log("Starting Phase 2");
                maxRefText = maxPlants;
                countProgress = maxPlants ;
               
                updateProgressText(-1);
                

                break;
            case 3:
                currentPhase = 3;
                pistol.SetActive(true);
                reminders(1);
                countProgress = 0;
                maxScraps = 3;
                countProgress = 3;
                updateProgressText(0);
                Debug.Log("Starting Phase 3");

                foreach (GameObject obj in wallsBad)
                {
                    obj.SetActive(true);

                }

                break;
                case 4:

                ChangeCameraFinishGame();

                



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
        counterProgressText.text = countProgress.ToString()+ " /" + maxScraps.ToString()  ;
        if (countProgress <= 0 && currentPhase==1)
        {
            Phases(2);
            

        }
        else if (countProgress <= 0 && currentPhase == 2)
        {
            Phases(3);
            
        }
        else if (countProgress <= 0 && currentPhase == 3)
        {
            Phases(4);
           
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
        plantingTimer += Time.deltaTime;
        progressBar.value  = plantingTimer;
        

        if (plantingTimer >= 1)
        {
            FinishPlanting();
        }
    }

   



    void FinishPlanting()
    {
        isPlanting = false;
        progressBar.value = 0f;
        progressBar.gameObject.SetActive(false);
        updateProgressText(-1);
    }


    private void reminders(int phaseReminder)
    {



        switch (phaseReminder)
        {
            case 0:

                dialogue.spanishLines = new string[] { "Excelente trabajo, después de toda esta basura que has reciclado mientras solucionabamos la averia no hemos contaminado la capa de ozono buen trabajo.En esta fase, plantarás árboles y plantas para restaurar la biodiversidad del santuario. Observa las diferentes cajas de almacenamiento que contienen las semillas y plántalas en las áreas designadas.\r\n\r\n" };
                dialogue.dialoguePanel = panel;

                dialogue.dialogueText = text;
                dialogue.StartSpanishDialogue();
                break;
            case 1:
                dialogue.spanishLines = new string[] { "Has hecho un trabajo excepcional en la fase de reforestación. Ahora, avanzamos hacia la fase final de nuestra misión.Para proteger nuestro santuario de intrusiones no deseadas, necesitamos reparar estas vallas dañadas. Toma el kit de reparación y disparales para arreglarlas. \r\n" };
                dialogue.dialoguePanel = panel;

                dialogue.dialogueText = text;
                dialogue.StartSpanishDialogue();
                break;
            case 2:
                dialogue.spanishLines = new string[] { "¡Increíble trabajo, guardián! Las vallas reparadas asegurarán que nuestro santuario esté protegido y sus habitantes estén seguros. Recuerda siempre la importancia de proteger nuestros hábitats naturales.\r\n" };
                dialogue.dialoguePanel = panel;
                dialogue.dialogueText = text;
                dialogue.StartSpanishDialogue();
                break;
                case 3:

                dialogue.spanishLines = new string[] { "Excelente trabajo, has logrado reparar la capa de ozono, ahora la naturaleza se recuperara.\r\n" };
                    dialogue.dialoguePanel = panel;
                    dialogue.dialogueText = text;
                    dialogue.StartSpanishDialogue();
                break;



        

         }

        

    }

    public void removeScrap(GameObject scrap)
    {
        
        updateProgressText(-1);

       
            Destroy(scrap);
        
    }

    private void ManagerPlant()
    {
        foreach (GameObject obj in plantTargets)
        {
            // Si el objeto está activo, desactívalo; de lo contrario, actívalo.
            obj.SetActive(!obj.activeSelf);
        }
    }

    private void manageWalls()
    {
        foreach (GameObject obj in wallsGood)
        {
            // Si el objeto está activo, desactívalo; de lo contrario, actívalo.
            obj.SetActive(!obj.activeSelf);
        }
        foreach (var item in tress)
        {
            item.SetActive(false);
        }
    }

    

    public void ChangeCameraFinishGame()
    {
       
            mainCam.enabled = false;
            secondCamera.enabled = true;
        Invoke("ChangeSceneMain", 15f);
        reminders(3);

        foreach (GameObject obj in scrapt)
        {
            obj.SetActive(false);
        }

        foreach (var item in plantTargets)
        {
            item.SetActive(false);
        }
        foreach (var item in tress)
        {
            item.SetActive(true);
        }


    }

    private void ChangeSceneMain()
    {
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
}
#endregion


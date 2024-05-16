using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static energyMinigameManager;


public class MiniGameManager4 : MonoBehaviour
{
    private int idObjectPhase;
    public int idMaterial1;
    public int idMaterial2;
    public int idMaterial3;
    private int HowManyMaterial1;
    private int HowManyMaterial2;
    private int HowManyMaterial3;
    private string description;
    private string nameMaterial1;
    private string nameMaterial2;
    private string nameMaterial3;
    private string nameObject;
    public Canvas CanvasMain;
    private Vector3[] ArrayController = new Vector3[3];
    public GameObject ojectToBuild;
    public int countBuilds;

    public int countMaterial1;
    public int countMaterial2;
    public int countMaterial3;

    public int phaseState;
    public int phaseBuild;
    public bool canBuildPart;

    public Text descriptionText;
    public Text material1Text;
    public Text material2Text;
    public Text material3Text;

    public float distanciaMaxima = 50f; // La distancia máxima desde el jugador para el spawn
    private DialogueScript dialogue;
    public GameObject[] objectsToSpawn; // Array de objetos que se van a spawnear
    public GameObject pointToSpawn; // Jugador
    public Vector3 spawnAreaCenter; // Centro del área de spawn
    public Vector3 spawnAreaSize; // Tamaño del área de spawn

    public Camera mainCam;
    public Camera secondCamera;
    public Camera thirdCamera;
    public Canvas thirdCanvas;
    public GameObject thirdPanel;
    public TMPro.TextMeshProUGUI thirdText;
    private bool conectingCable;

    // Start is called before the first frame update
    void Start()
    {
        PhaseMode(1);
        secondCamera.enabled = false;
        thirdCamera.enabled = false;
        thirdCanvas.enabled = false;
        dialogue = GetComponent<DialogueScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ChangeCameraMode();


        //}

        }
    #region Material Assignment

    private void SetMaterialsForObject()
    {
        idObjectPhase++;
        description = "Recoge materiales para " + nameObject;
        switch (idObjectPhase)
        {
            case 1: // Base
                nameObject = "Base molino";
                nameMaterial1 = "Botella azul";
                idMaterial1 = 0;
                HowManyMaterial1 = 1;

                nameMaterial2 = "Botella morada";
                idMaterial2 = 1;
                HowManyMaterial2 = 1;

                nameMaterial3 = "Botella verde";
                idMaterial3 = 2;
                HowManyMaterial3 = 1;
                break;

            case 2: // Blades
                nameObject = "Cuerpo";
                idMaterial1 = 1;
                nameMaterial1 = "Botella morada";
                HowManyMaterial1 = 1;

                idMaterial2 = 1;
                nameMaterial2 = "Botella verde";
                HowManyMaterial2 = 1;

                idMaterial3 = 0;
                nameMaterial3 = "Botella azul";
                HowManyMaterial3 = 1;
                break;

            case 3: // Gearbox
                nameObject = "Hélices";
                idMaterial1 = 2;
                nameMaterial1 = "Botella verde";
                HowManyMaterial1 =1;

                idMaterial2 = 0;
                nameMaterial2 = "Botella azul";
                HowManyMaterial2 = 1;

                idMaterial3 = 1;
                nameMaterial3 = "Botella morada";
                HowManyMaterial3 = 1;
                break;

            
        }
        updateTextsProgress();
    }

    #endregion


    public void  PhaseMode(int phase)
    
    {
        phaseState = phase;

        switch (phase)
        {
            case 1:
                SpawnObjects();
                updateTextsProgress();
                break;
            case 2:
                SetMaterialsForObject();

                break;

            case 3:
                description = "Mete los materiales en el reciclador";
                descriptionText.text = description;
                break;
            case 4:
                description = "Busca un sitio para plantar el molino";
                descriptionText.text = description;

                break;
            case 5:
                description = "Monta la pieza reciclada";
                descriptionText.text = description;
                break;
                case 6:
                
                countBuilds++;
                if (countBuilds == 3)
                {
                    ChangeCameraMode();                }

                break;


            default:
              
                break;
            }


        }
    public void PhaseBuild()
    {
        phaseBuild++;
        switch (phaseBuild)
        {
            case 0:
                // Pieza por colocar


                break;
            case 1:
                // Construye la base

                break;

            case 2:
                // Construye la parte central
                break;

            case 3:
                // Construye la cabeza

                CheckFinished();
                break;
            case 4:
                // Pieza completa
                

                break;


            default:
                
                break;
        }
    }

    private void invoke(string v1, float v2)
    {
        throw new NotImplementedException();
    }





    #region Object Spawning

    public void SpawnObjects()
    {
        Vector3 jugadorPosition = pointToSpawn.transform.position;

        for (int i = 0; i < 60; i++)
        {
            
            int id = UnityEngine.Random.Range(0, 3);
            // Calculate a random position within a circle around the player
            Vector2 offset = UnityEngine.Random.insideUnitCircle * distanciaMaxima;
            Vector3 spawnPosition = new Vector3(jugadorPosition.x + offset.x, 0.3f, jugadorPosition.z + offset.y);

            // Spawn the object at the calculated position
            GameObject prefab = Instantiate(objectsToSpawn[id], spawnPosition, Quaternion.identity);
            prefab.GetComponent<ObjectToCatch>().id = id;
        }
        PhaseMode(2);
    }

    #endregion



    public void ChooseAreaToSpawn(Vector3 spawnPosition)
    {

        if (ArrayController[0] == new Vector3(0, 0, 0))
        {
           if(phaseBuild == 0)
            ArrayController[0] = spawnPosition;
            Instantiate(ojectToBuild, spawnPosition, Quaternion.identity);
            onlyDescriptionMode();
            
            PhaseMode(5);
            if (phaseBuild == 0 && phaseBuild != 4)
            {
                PhaseBuild();
            }
        }

        if ( ArrayController[1] == new Vector3(0, 0, 0))
        {
            Vector3 difference = ArrayController[0] - spawnPosition;
            float distance = difference.magnitude;

           


            if (distance > 20f)
            {
                //Debug.Log("La distancia entre los puntos es mayor de 50 metros.");
                ArrayController[1]  = spawnPosition;
                Instantiate(ojectToBuild, spawnPosition, Quaternion.identity);


            }
            else
            {
                //Debug.Log("La distancia entre los puntos es menor o igual a 50 metros.");
                Debug.Log(ArrayController[0]);
                Debug.Log(spawnPosition);
            }

        }
        else if (ArrayController[0] != new Vector3(0, 0, 0) && ArrayController[1] != new Vector3(0, 0, 0) && ArrayController[2] == new Vector3(0, 0, 0))
        {
            Vector3 difference = ArrayController[1] - spawnPosition;
            float distance = difference.magnitude;

            if (distance > 50f)
            {
                Debug.Log("La distancia entre los puntos es mayor de 50 metros.");
                ArrayController[2] = spawnPosition;
            }
            else
            {
                Debug.Log("La distancia entre los puntos es menor o igual a 50 metros.");
            }

        }

    }



    #region UI Update Methods

    public void updateTextsProgress()
    {
        descriptionText.text = description + nameObject;

        material1Text.text = nameMaterial1 + ": (" + countMaterial1 + "/" + HowManyMaterial1 + ")";
        material2Text.text = nameMaterial2 + ": (" + countMaterial2 + "/" + HowManyMaterial2 + ")";
        material3Text.text = nameMaterial3 + ": (" + countMaterial3 + "/" + HowManyMaterial3 + ")";

        if (countMaterial1 >= HowManyMaterial1 && countMaterial2 >= HowManyMaterial2 && countMaterial3 >= HowManyMaterial3)
        {
            canBuildPart = true;
            PhaseMode(3);
        }

        if (countMaterial1 >= HowManyMaterial1)
        {
            material1Text.color = Color.green;
        }
        if (countMaterial2 >= HowManyMaterial2)
        {
            material2Text.color = Color.green;
        }
        if ((countMaterial3 >= HowManyMaterial3))
        {
            material3Text.color = Color.green;
        }
    }

    public void resetTextsProgress()
    {
        descriptionText.text = description + nameObject;
        countMaterial1 = 0;
        countMaterial2 = 0;
        countMaterial3 = 0;
        material1Text.text = nameMaterial1 + ": (" + countMaterial1 + "/" + HowManyMaterial1 + ")";
        material2Text.text = nameMaterial2 + ": (" + countMaterial2 + "/" + HowManyMaterial2 + ")";
        material3Text.text = nameMaterial3 + ": (" + countMaterial3 + "/" + HowManyMaterial3 + ")";

        material1Text.color = Color.black;
        material2Text.color = Color.black;
        material3Text.color = Color.black;
    }

    public void onlyDescriptionMode()
    {
        countMaterial1 = 0;
        countMaterial2 = 0;
        countMaterial3 = 0;
        material1Text.text = "";
        material2Text.text = "";
        material3Text.text = "";
    }

    #endregion





    public void ChangeCameraMode()
    {
        if (mainCam.enabled)
        {
            
            mainCam.enabled = false;
            secondCamera.enabled = true;
            CanvasMain.enabled = false;

        }
        else if (secondCamera.enabled)
        {
            secondCamera.enabled = false;
            mainCam.enabled = true;
            CanvasMain.enabled = true;
            PhaseMode(2);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            phaseBuild = 0;
            countBuilds++;
              
                ChangeCameraFinishGame();
            
        }


    }

    




    private void CheckFinished()
    {
        if(countBuilds == 3)
        {
            ChangeCameraFinishGame();
        }
    }

    public void ChangeCameraFinishGame()
    {
        if (mainCam.enabled)
        {
            mainCam.enabled = false;
            thirdCamera.enabled = true;
            CanvasMain.enabled = false;
            thirdCanvas.enabled = true;
            dialogue.spanishLines = new string[] { "Excelente trabajo, ahora que las farolas están en funcionamiento esta ciudad conseguirá luz sin tener que usar energía  renovable.\r\n" };
            dialogue.dialoguePanel = thirdPanel;
            dialogue.dialogueText = thirdText;
            dialogue.StartSpanishDialogue();

            Invoke("ChangeSceneMain", 15f);

        }

    }

    private void ChangeSceneMain()
    {
        SceneManager.LoadScene("LevelSelector");
    }







}

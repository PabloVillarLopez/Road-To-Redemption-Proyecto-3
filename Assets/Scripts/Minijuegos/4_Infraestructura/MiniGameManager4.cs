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
    public int lightmapIndexToUse; // Índice del baked lightmap que deseas que se utilice
                                   // Start is called before the first frame update

    public Image interact;
    public Image Ninteract;
    public Sprite interactEnglish;
    public Sprite NinteractEnglish;

    void Start()
    {
        PhaseMode(1);
        secondCamera.enabled = false;
        thirdCamera.enabled = false;
        thirdCanvas.enabled = false;
        dialogue = GetComponent<DialogueScript>();
        ApplyLightmapToScene();
        activeInteract(false);
          if (LanguageManager.currentLanguage == LanguageManager.Language.English)

        {
            interact.sprite = interactEnglish;
            Ninteract.sprite = NinteractEnglish;
        }
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

        // Descripción inicial de recolección de materiales, sin el nombre específico del objeto
        if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            description = "Recoge materiales para " + nameObject;
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            description = "Collect materials for " + nameObject;
        }

        switch (idObjectPhase)
        {
            case 1: // Base
                if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                {
                    nameObject = "Base del molino";
                    nameMaterial1 = "Botella de plástico";
                    nameMaterial2 = "Vidrio";
                    nameMaterial3 = "Pila";
                }
                else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                {
                    nameObject = "Windmill base";
                    nameMaterial1 = "Plastic bottle";
                    nameMaterial2 = "Glass";
                    nameMaterial3 = "Battery";
                }

                idMaterial1 = 0;
                HowManyMaterial1 = 1;
                idMaterial2 = 1;
                HowManyMaterial2 = 1;
                idMaterial3 = 2;
                HowManyMaterial3 = 1;
                break;

            case 2: // Blades
                if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                {
                    nameObject = "Cuerpo";
                    nameMaterial1 = "Vidrio";
                    nameMaterial2 = "Pila";
                    nameMaterial3 = "Botella de plástico";
                }
                else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                {
                    nameObject = "Body";
                    nameMaterial1 = "Glass";
                    nameMaterial2 = "Battery";
                    nameMaterial3 = "Plastic bottle";
                }

                idMaterial1 = 1;
                HowManyMaterial1 = 1;
                idMaterial2 = 1;
                HowManyMaterial2 = 1;
                idMaterial3 = 0;
                HowManyMaterial3 = 1;
                break;

            case 3: // Gearbox
                if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                {
                    nameObject = "Hélices";
                    nameMaterial1 = "Pila";
                    nameMaterial2 = "Botella de plástico";
                    nameMaterial3 = "Vidrio";
                }
                else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                {
                    nameObject = "Blades";
                    nameMaterial1 = "Battery";
                    nameMaterial2 = "Plastic bottle";
                    nameMaterial3 = "Glass";
                }

                idMaterial1 = 2;
                HowManyMaterial1 = 1;
                idMaterial2 = 0;
                HowManyMaterial2 = 1;
                idMaterial3 = 1;
                HowManyMaterial3 = 1;
                break;
        }

        // Actualizar el texto de progreso
        updateTextsProgress();
    }

    #endregion


    public void PhaseMode(int phase)
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
                // Configurar descripción basada en el idioma
                if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                {
                    description = "Mete los materiales en el reciclador";
                }
                else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                {
                    description = "Put the materials into the recycler";
                }
                descriptionText.text = description;
                break;
            case 4:
                // Configurar descripción basada en el idioma
                if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                {
                    description = "Busca un sitio para plantar el molino";
                }
                else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                {
                    description = "Find a place to plant the windmill";
                }
                descriptionText.text = description;
                break;
            case 5:
                // Configurar descripción basada en el idioma
                if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                {
                    description = "Monta la pieza reciclada";
                }
                else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                {
                    description = "Assemble the recycled piece";
                }
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

        material1Text.color = Color.white;
        material2Text.color = Color.white;
        material3Text.color = Color.white;
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
    public void ChangeCameraFinishGame()
    {
        if (mainCam.enabled)
        {
            // Desactivar la cámara principal y activar la tercera cámara
            mainCam.enabled = false;
            thirdCamera.enabled = true;

            // Desactivar el Canvas principal y activar el tercer Canvas
            CanvasMain.enabled = false;
            thirdCanvas.enabled = true;

            // Verificar el idioma actual y asignar las líneas de diálogo en consecuencia
            if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                dialogue.spanishLines = new string[] {
            "Excelente trabajo, ahora que las farolas están en funcionamiento esta ciudad conseguirá luz sin tener que usar energía renovable.\r\n"
        };
                dialogue.dialoguePanel = thirdPanel;
                dialogue.dialogueText = thirdText;
                dialogue.StartSpanishDialogue();
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                dialogue.englishLines = new string[] {
            "Great job! Now that the streetlights are working, this city will get light without needing to use renewable energy.\r\n"
        };
                dialogue.dialoguePanel = thirdPanel;
                dialogue.dialogueText = thirdText;
                dialogue.StartEnglishDialogue();
            }

            // Invocar el cambio de escena después de 7 segundos
            Invoke("ChangeSceneMain", 7f);
        }


    }

    private void ChangeSceneMain()
    {
        MinigamesCompleted.minigame4Finished = true;
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

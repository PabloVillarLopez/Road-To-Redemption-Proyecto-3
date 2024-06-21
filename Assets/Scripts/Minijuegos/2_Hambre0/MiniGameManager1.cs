using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManager1 : MonoBehaviour
{
    #region Variables

    // Variables de tipo GameObject
    [SerializeField] GameObject[] objectsToSpawn = new GameObject[3];
    public GameObject[] seeds = new GameObject[3];
    [SerializeField] GameObject objectSpawner;
    [SerializeField] GameObject cultiveZone;
    GameObject greenHouse;

    // Variables de tipo float
    public float offSetYSpawn;
    public float offSetZSpawn;
    public float dayTimeInSeconds = 150f;
    private float elapsedTime = 0f;
    public float temperatureChangePerSecond = 0.001f;
    public float temperature = 20;
    float duration = 300f;
    float cutoffThreshold = 12.5f;
    public int level;
    float totalTime = 900f;
    float elapsedTimeBuild = 0f;
    public float fullNightDurationInSeconds = 100f;
    // Variables de tipo Light
    public Light sunLight;
    Vector3 spawnPosition;

    // Variables de tipo int
    private int badFood = -1;
    private int goodFood = -1 ;

    // Variables de tipo Text
    public Text TextBadFood;
    public Text TextGoodFood;
    public Text temperatureText;

    // Variables de tipo Material
    public Material temporaryMaterial;
    private Material originalMat;

    // Variables de tipo Skybox
    public Skybox skybox;

    // Variables de tipo DialogueScript
    public DialogueScript dialog;

    // Variables de tipo bool
    public bool isDialogueFinished = false;
    public bool dayTime = true;
    // Variables de tipo array
    object[][] fruit = new object[3][];

    public Image interact;
    public Image Ninteract;
    public Sprite interactEnglish;
    public Sprite NinteractEnglish;
    public GameObject dispenser;
 
    public GameObject panel;
    public TMPro.TextMeshProUGUI text;
    public GameObject markGuide;
    public Image DialogueByImage;

    public GameObject Objective;


    public List<AudioClip> soundClips = new List<AudioClip>(); // Lista de clips de sonido
    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioSource audioSourceBackground;
    #endregion

    #region Unity Methods

    void Start()
    {
        CheckDialogue();
        greenHouse = GameObject.Find("GreenHouse");
        Level(1);
        Objective.SetActive(false);

        audioSourceBackground.clip = soundClips[0];
        audioSourceBackground .Play();


        SpawnObjectsOnSpawner();
        SpawnOchards();
        activeInteract(false);
        StartCoroutine(CheckDialogue());
        StartCoroutine(AdjustMaterialsOverTime());

        if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {

        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.English)

        {
            interact.sprite = interactEnglish;
            Ninteract.sprite = NinteractEnglish;
        }
    }

    IEnumerator CheckDialogue()
    {

        if (dialog.dialogueFinished && dialog.dialoguePanel.activeSelf == false)
        {
            isDialogueFinished = true;
            if (greenHouse != null)
            {
                StartCoroutine(AdjustMaterialsOverTime());
                if(markGuide != null)
                {
                    Instantiate(markGuide, dispenser.transform.position + new Vector3(-3, 3, 3), Quaternion.identity);
                }
                List<int> index = new List<int> { 0, 1 }; 
                DialogueByImage.GetComponent<DialogueByImage>().ShowCustomSequence(index);
                activeObjective();
            }
        }
        while (!isDialogueFinished)
        {
            // Llama a la corrutina original
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(CheckDialogue());


        }



    }
    void Update()
    {


        UpdateCycleDays();




    }

    #endregion

    #region Object Spawning

    public void SpawnObjectsOnSpawner()
    {
        spawnPosition = new Vector3(objectSpawner.transform.position.x, (objectSpawner.transform.position.y), objectSpawner.transform.position.z);
        spawnPosition += new Vector3(0, -0.05F, -2);

        for (int i = 0; i < 3; i++)
        {
            spawnPosition += new Vector3(0, 0, offSetZSpawn);
            GameObject spawnedObject = Instantiate(objectsToSpawn[0], spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetId(1);
            }

        }
        spawnPosition += new Vector3(offSetZSpawn, 0, 0);
        for (int i = 0; i < 2; i++)
        {
            GameObject spawnedObject = Instantiate(objectsToSpawn[0], spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetId(1);
            }
            spawnPosition += new Vector3(0, 0, -offSetZSpawn);
        }

        spawnPosition += new Vector3(0, 0, 2);
        for (int i = 0; i < 3; i++)
        {
            spawnPosition += new Vector3(0, 0, offSetZSpawn);
            GameObject spawnedObject = Instantiate(objectsToSpawn[1], spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetId(2);
            }

        }
        spawnPosition += new Vector3(offSetZSpawn, 0, 0);
        for (int i = 0; i < 2; i++)
        {
            GameObject spawnedObject = Instantiate(objectsToSpawn[1], spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetId(2);
            }
            spawnPosition += new Vector3(0, 0, -offSetZSpawn);
        }

        spawnPosition += new Vector3(0, 0, 1);
        for (int i = 0; i < 3; i++)
        {
            spawnPosition += new Vector3(0, 0, offSetZSpawn);
            GameObject spawnedObject = Instantiate(objectsToSpawn[2], spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetId(3);
            }

        }
        spawnPosition += new Vector3(offSetZSpawn, 0, 0);
        for (int i = 0; i < 2; i++)
        {
            GameObject spawnedObject = Instantiate(objectsToSpawn[2], spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetId(3);
            }
            spawnPosition += new Vector3(0, 0, -offSetZSpawn);
        }


    }

    void SpawnOchards()
    {
        // Usar la posición de objectSpawner como base para la posición inicial de los huertos.
        Vector3 basePosition = objectSpawner.transform.position;
        Vector3 spawnPosition = new Vector3(basePosition.x, basePosition.y, basePosition.z);

        // Definir un desplazamiento inicial en Z, por ejemplo, para empezar al lado del spawner
        float initialOffsetZ = 5.0f;  // Ajusta este valor según sea necesario para el desplazamiento inicial
        spawnPosition.z += initialOffsetZ;

        for (int i = 0; i < 3; i++)
        {
            // Añadir un desplazamiento en Z para cada huerto nuevo
            int offSetZOchard = 5; // Este es el desplazamiento entre cada huerto

            // Actualizar la posición z para el nuevo huerto
            spawnPosition.z += offSetZOchard;

            // Spawnear el prefab en la posición calculada
            GameObject CultiveObject = Instantiate(cultiveZone, spawnPosition, Quaternion.identity);
            CultiveObject.GetComponent<CultiveZone>().SetFruitStats((string)fruit[i][0], (int)fruit[i][1], (int)fruit[i][2]);
        }
    }

    public void PlaySound(int sound)
    {
        audioSource.clip = soundClips[sound];
        audioSource.Play();
    }
    public bool PlantSound(bool planting) { 
    
    if (planting)
        {
            audioSource.clip = soundClips[1];
            audioSource.Play();
        }
    else
        {
            audioSource.Stop();
        }
    return planting;
    
    
    }

    public void reminderNotCatch()
    {
        if (dialog.dialogueFinished && dialog.dialoguePanel.activeSelf == false)
        {

            dialog.spanishLines = new string[] { "Primero planta las semillas que has cogido.\r\n" };
            dialog.dialoguePanel = panel;
            dialog.dialogueText = text;
            dialog.StartSpanishDialogue();
        }
    }

    public void reminderNotRecollect()
    {
        if (dialog.dialogueFinished && dialog.dialoguePanel.activeSelf == false)
        {

            dialog.spanishLines = new string[] { "Esta planta todavia no está lista para ser recogida. Además de tener una mayor biodisponibilidad de antioxidantes y nutrientes clave, lo que mejora su valor nutricional\r\n" };
            dialog.dialoguePanel = panel;
            dialog.dialogueText = text;
            dialog.StartSpanishDialogue();
        }
    }
    #endregion

    #region Day-Night Cycle

    void UpdateCycleDays()
    {
        elapsedTime += Time.deltaTime;
        float totalDayCycleInSeconds = dayTimeInSeconds + fullNightDurationInSeconds;
        float percentageOfDay = elapsedTime / totalDayCycleInSeconds;

        if (percentageOfDay >= 1f)
        {
            elapsedTime = 0f;
        }

        float rotationAngle = (elapsedTime / dayTimeInSeconds) * 360f;
        sunLight.transform.rotation = Quaternion.Euler(rotationAngle - 90f, 170f, 0f);

        if (percentageOfDay <= dayTimeInSeconds / totalDayCycleInSeconds)
        {
            Camera.main.backgroundColor = Color.Lerp(Color.black, Color.blue, percentageOfDay / (dayTimeInSeconds / totalDayCycleInSeconds));
            AdjustTemperatureDuringDay();
            cultiveZone.GetComponent<CultiveZone>().day = true;
            Debug.Log("Es de día");
        }
        else if (percentageOfDay > dayTimeInSeconds / totalDayCycleInSeconds && percentageOfDay <= (dayTimeInSeconds / totalDayCycleInSeconds) + (0.25f * fullNightDurationInSeconds / totalDayCycleInSeconds))
        {
            Camera.main.backgroundColor = Color.Lerp(Color.blue, Color.black, (percentageOfDay - (dayTimeInSeconds / totalDayCycleInSeconds)) / (0.25f * fullNightDurationInSeconds / totalDayCycleInSeconds));
            AdjustTemperatureDuringNight();
            cultiveZone.GetComponent<CultiveZone>().day = false;
            Debug.Log("Es de noche (transición a noche completa)");
        }
        else
        {
            Camera.main.backgroundColor = Color.black; // Hacer la noche más oscura
            AdjustTemperatureDuringNight();
            cultiveZone.GetComponent<CultiveZone>().day = false;
            Debug.Log("Es de noche (noche completa)");
        }
    }


    void AdjustTemperatureDuringDay()
    {
        temperature += temperatureChangePerSecond * Time.deltaTime;
        dayTime = true;

        float exposure = Mathf.Lerp(0.2f, 1.5f, elapsedTime / dayTimeInSeconds);
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
        DynamicGI.UpdateEnvironment();
    }

    void AdjustTemperatureDuringNight()
    {
        temperature -= temperatureChangePerSecond * Time.deltaTime;
        dayTime = false;

        float nightElapsedTime = elapsedTime - dayTimeInSeconds;
        float exposure = Mathf.Lerp(1.5f, 0.2f, nightElapsedTime / fullNightDurationInSeconds);
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
        DynamicGI.UpdateEnvironment();
    }



    #endregion

    #region Inventory Management

    private void activeObjective() 
    
    {
        Objective.SetActive(true);

        checkBadFood();
        checkGoodFood();

    }


    public void checkBadFood()
    {
        audioSource.clip = soundClips[2];
        audioSource.Play();
        badFood++;
        // Actualizamos el texto según el idioma actual
        if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            TextBadFood.text = "Comida Desperdiciada: " + badFood;
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            TextBadFood.text = "Wasted Food: " + badFood;
        }
    }

    public void checkGoodFood()
    {
        audioSource.clip = soundClips[1];
        audioSource.Play();
        goodFood++;
        // Actualizamos el texto según el idioma actual
        if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            TextGoodFood.text = "Comida Recolectada: " + goodFood;
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            TextGoodFood.text = "Collected Food: " + goodFood;
        }
    }



    void Level(int level)
    {
        // Switch para manejar los diferentes niveles
        switch (level)
        {
            case 1:
                temperatureChangePerSecond = 0.2f;
                // Definir las frutas con sus nombres y rangos de temperatura
                if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
                {
                    fruit[0] = new object[] { "Fresa", 20, 30 };
                    fruit[1] = new object[] { "Tomate", 15, 25 };
                    fruit[2] = new object[] { "Pimiento", 22, 31 };
                }
                else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
                {
                    fruit[0] = new object[] { "Strawberry", 20, 30 };
                    fruit[1] = new object[] { "Tomato", 15, 25 };
                    fruit[2] = new object[] { "Bell Pepper", 22, 31 };
                }
                break;
            case 2:
                temperatureChangePerSecond = 0.3f;
                // Otros niveles pueden definirse de manera similar
                break;
            case 3:
                temperatureChangePerSecond = 0.4f;
                // Otros niveles pueden definirse de manera similar
                break;
        }
    }

    #endregion
    IEnumerator AdjustMaterialsOverTime()
    {
        float startTime = Time.time;
        MeshRenderer renderer = greenHouse.GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            // Guarda el material original solo una vez
            if (originalMat == null)
            {
                originalMat = renderer.material;
            }
            renderer.material = temporaryMaterial; // Asigna el material temporal desde el Inspector
        }

        // Asegúrate de que el diálogo ha terminado antes de continuar
        if (isDialogueFinished)
        {
            while (Time.time - startTime < duration)
            {
                if (renderer != null)
                {
                    float currentCutoffHeight = renderer.material.GetFloat("_CutoffHeight");
                    float incremento = 0.1f * Time.deltaTime;
                    float newCutoffHeight = currentCutoffHeight + incremento;
                    renderer.material.SetFloat("_CutoffHeight", newCutoffHeight);

                    // Revisa si el valor de _CutoffHeight supera el umbral
                    if (newCutoffHeight >= cutoffThreshold)
                    {
                        renderer.material = originalMat; // Reasigna el material original
                        break; // Detiene la corutina
                    }

                    // Actualiza otros valores de material si se sigue utilizando el material temporal
                    if (renderer.material == temporaryMaterial)
                    {
                        renderer.material.SetFloat("_NoiseStrength", 8.04f);
                        renderer.material.SetFloat("_NoiseScale", 46.34f);
                        renderer.material.SetFloat("_EdgeWidth", 0.36f);
                    }
                }
                yield return null;
            }

            // Asegura que el material original se reestablezca al finalizar el tiempo total
            if (renderer != null && renderer.material != originalMat)
            {
                renderer.material = originalMat;
            }

            if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                if (goodFood == 0)
                {
                    dialog.spanishLines = new string[] {
            "Vaya, parece que no has superado esta prueba, lamentablemente van a pasar hambre muchas familias por no gestionar bien los alimentos.\r\n"
        };
                }
                else if (goodFood >= 2 && goodFood < 5)
                {
                    dialog.spanishLines = new string[] {
            "¡Increíble! Tu generosidad ha marcado la diferencia. Has contribuido a la meta de Hambre Cero, ayudando a crear un mundo donde nadie pase hambre.\r\n"
        };
                }
                else if (goodFood >= 5 && goodFood < 10)
                {
                    dialog.spanishLines = new string[] {
            "¿Sabías que cada comida salvada ayuda a reducir el desperdicio y a alimentar a quienes más lo necesitan? Gracias a tu esfuerzo, estamos un paso más cerca de alcanzar el Hambre Cero.\r\n"
        };
                }
                else if (goodFood >= 10 && goodFood < 15)
                {
                    dialog.spanishLines = new string[] {
            "¡Fantástico! Gracias a ti, 50 familias tendrán algo en sus mesas esta noche. Continuemos trabajando juntos para acabar con el hambre en el mundo.\r\n"
        };
                }
                else if (goodFood >= 15 && goodFood < 20)
                {
                    dialog.spanishLines = new string[] {
            "¡Impresionante! Ahora, gracias a ti, 75 familias tendrán algo que comer. Tu ayuda es crucial en la lucha contra el hambre.\r\n"
        };
                }
                else if (goodFood >= 20)
                {
                    dialog.spanishLines = new string[] {
            "Increíble. ¡Has salvado suficiente comida para alimentar a 100 familias! Cada acción cuenta en nuestra misión de alcanzar el Hambre Cero. ¡Gracias por tu compromiso!\r\n"
        };
                }
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                if (goodFood == 0)
                {
                    dialog.englishLines = new string[] {
            "Oh no, it seems you didn't pass this test. Unfortunately, many families will go hungry due to poor food management.\r\n"
        };
                }
                else if (goodFood >= 2 && goodFood < 5)
                {
                    dialog.englishLines = new string[] {
            "Amazing! Your generosity has made a difference. You've contributed to the Zero Hunger goal, helping to create a world where no one goes hungry.\r\n"
        };
                }
                else if (goodFood >= 5 && goodFood < 10)
                {
                    dialog.englishLines = new string[] {
            "Did you know that every meal saved helps reduce waste and feed those in need? Thanks to your effort, we are one step closer to achieving Zero Hunger.\r\n"
        };
                }
                else if (goodFood >= 10 && goodFood < 15)
                {
                    dialog.englishLines = new string[] {
            "Fantastic! Thanks to you, 50 families will have something on their tables tonight. Let's continue working together to end world hunger.\r\n"
        };
                }
                else if (goodFood >= 15 && goodFood < 20)
                {
                    dialog.englishLines = new string[] {
            "Impressive! Now, thanks to you, 75 families will have something to eat. Your help is crucial in the fight against hunger.\r\n"
        };
                }
                else if (goodFood >= 20)
                {
                    dialog.englishLines = new string[] {
            "Incredible. You have saved enough food to feed 100 families! Every action counts in our mission to achieve Zero Hunger. Thank you for your commitment!\r\n"
        };
                }
            }


            dialog.dialoguePanel = panel;
                dialog.dialogueText = text;

            //if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            //{
            //    dialog.StartSpanishDialogue();
            //}
            //else if (LanguageManager.currentLanguage != LanguageManager.Language.English)
            //{
            //    dialog.StartSpanishDialogue();

            //}


            if (goodFood > 0)
            {
                MinigamesCompleted.minigame2Finished = true;
                DialogueByImage.GetComponent<DialogueByImage>().ShowImageByIndex(2);

            }
            // Invocar el cambio de escena después de 15 segundos
            Invoke("ChangeSceneMain", 15f);
        }
    }


    public void Reminders()
    {
        if (dialog.dialogueFinished && dialog.dialoguePanel.activeSelf == false)
        {
            if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                if (goodFood == 5)
                {
                    dialog.spanishLines = new string[] {
                "Vaya, recluso no esperaba que fueras tan bueno, ¿te han dicho alguna vez que se echa a perder mucha comida a lo largo del año?\r\n"
            };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartSpanishDialogue();
                }
                else if (goodFood == 10)
                {
                    dialog.spanishLines = new string[] {
                "Excelente trabajo, con la comida que has salvado se van a poder alimentar 50 familias\r\n"
            };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartSpanishDialogue();
                }
                else if (badFood == 2)
                {
                    dialog.spanishLines = new string[] {
                "¿Cuidado muchacho, quieres volver a tener problemas? ¿Sabes cuánta gente podría alimentarse con esa comida que se ha echado a perder?\r\n"
            };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartSpanishDialogue();
                }
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                if (goodFood == 5)
                {
                    dialog.englishLines = new string[] {
                "Wow, inmate, I didn't expect you to be so good. Have you ever been told that a lot of food goes to waste throughout the year?\r\n"
            };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartEnglishDialogue();
                }
                else if (goodFood == 10)
                {
                    dialog.englishLines = new string[] {
                "Excellent work, with the food you've saved, 50 families will be able to eat.\r\n"
            };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartEnglishDialogue();
                }
                else if (badFood == 2)
                {
                    dialog.englishLines = new string[] {
                "Watch out, boy, do you want to get in trouble again? Do you know how many people could be fed with the food that's been wasted?\r\n"
            };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartEnglishDialogue();
                }
            }
        }

    }

    public void badReminders()
    {
        if (badFood == 1)
        {
            if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                dialog.spanishLines = new string[] {
            "¡Cuidado! Has dejado que se eche a perder una comida, recuerda que hay muchas personas que no tienen la oportunidad de tener algo en su mesa.\r\n"
        };
                dialog.dialoguePanel = panel;
                dialog.dialogueText = text;
                dialog.StartSpanishDialogue();
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                dialog.englishLines = new string[] {
            "Watch out! You let one meal go to waste. Remember, there are many people who don't have the chance to have something on their table.\r\n"
        };
                dialog.dialoguePanel = panel;
                dialog.dialogueText = text;
                dialog.StartEnglishDialogue();
            }
        }
        else if (badFood == 5)
        {
            if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                dialog.spanishLines = new string[] {
            "Has permitido que se desperdicien cinco comidas. Este desperdicio podría haber alimentado a varias personas necesitadas. ¡Recuerda la importancia de no desperdiciar comida!\r\n"
        };
                dialog.dialoguePanel = panel;
                dialog.dialogueText = text;
                dialog.StartSpanishDialogue();
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                dialog.englishLines = new string[] {
            "You've allowed five meals to go to waste. This waste could have fed several people in need. Remember the importance of not wasting food!\r\n"
        };
                dialog.dialoguePanel = panel;
                dialog.dialogueText = text;
                dialog.StartEnglishDialogue();
            }
        }
        else if (badFood == 10)
        {
            if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                dialog.spanishLines = new string[] {
            "¡Preocupante! Diez comidas se han echado a perder. Cada desperdicio es una oportunidad perdida para ayudar a quienes pasan hambre. Reflexiona sobre el impacto de tus acciones.\r\n"
        };
                dialog.dialoguePanel = panel;
                dialog.dialogueText = text;
                dialog.StartSpanishDialogue();
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                dialog.englishLines = new string[] {
            "Worrisome! Ten meals have gone to waste. Each waste is a missed opportunity to help those who are hungry. Reflect on the impact of your actions.\r\n"
        };
                dialog.dialoguePanel = panel;
                dialog.dialogueText = text;
                dialog.StartEnglishDialogue();
            }
        }

    }


    public void activeInteract (bool active)
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


    private void ChangeSceneMain()
    {
        
        SceneManager.LoadScene("LevelSelector");

        
    }

}
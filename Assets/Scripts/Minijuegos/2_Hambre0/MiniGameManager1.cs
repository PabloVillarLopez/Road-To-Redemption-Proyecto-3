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
    private int badFood;
    private int goodFood;

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
    public GameObject dispenser;
 
    public GameObject panel;
    public TMPro.TextMeshProUGUI text;
    public GameObject markGuide;

    #endregion

    #region Unity Methods

    void Start()
    {
        CheckDialogue();
        greenHouse = GameObject.Find("GreenHouse");
        
        Level(1);
        
        SpawnObjectsOnSpawner();
        SpawnOchards();
        activeInteract(false);
        StartCoroutine(CheckDialogue());
        StartCoroutine(AdjustMaterialsOverTime());
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



    public void reminderNotCatch()
    {
        dialog.spanishLines = new string[] { "Primero planta las semillas que has cogido.\r\n" };
        dialog.dialoguePanel = panel;
        dialog.dialogueText = text;
        dialog.StartSpanishDialogue();
    }

    public void reminderNotRecollect()
    {
        dialog.spanishLines = new string[] { "Esta planta todavia no está lista para ser recogida. Además de tener una mayor biodisponibilidad de antioxidantes y nutrientes clave, lo que mejora su valor nutricional\r\n" };
        dialog.dialoguePanel = panel;
        dialog.dialogueText = text;
        dialog.StartSpanishDialogue();
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




    public void checkBadFood()
    {
        badFood++;
        TextBadFood.text = "Wasted Food: " + badFood;

    }

    public void checkGoodFood()
    {
        goodFood++;
        TextGoodFood.text = "Recolected Food: " + goodFood;

    }


    void Level(int level)
    {
        // Switch para manejar los diferentes niveles
        switch (level)
        {
            case 1:
                temperatureChangePerSecond = 0.2f;
                fruit[0] = new object[] { "Fresa", 20, 30 };
                fruit[1] = new object[] { "Tomate", 15, 25 };
                fruit[2] = new object[] { "Pimiento", 22, 31 };

                break;
            case 2:
                temperatureChangePerSecond = 0.3f;
                break;
            case 3:
                temperatureChangePerSecond = 0.4f;
                break;

        }
        #endregion
    }
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

            if (goodFood == 0)
            {
                dialog.spanishLines = new string[] {
               "Vaya, parece que no has superado esta prueba, lamentablemente van a pasar hambre muchas familias por no gestionar bien los alimientos.\r\n"
    };
            }

            if (goodFood >= 2 && goodFood < 5)
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

            // Inicia el diálogo
            dialog.dialoguePanel = panel;
            dialog.dialogueText = text;
            dialog.StartSpanishDialogue();

            if (goodFood > 0)
            {
                MinigamesCompleted.minigame2Finished = true;
            }
            // Invocar el cambio de escena después de 15 segundos
            Invoke("ChangeSceneMain", 15f);
        }
    }


    public void Reminders()
    {
        if (goodFood == 5)
        {
            dialog.spanishLines = new string[] { "Vaya, recluso no esperaba que fueras tan bueno, te han dicho alguna vez que se echa a perder mucha comida a lo largo del año?\r\n" };
            dialog.dialoguePanel = panel;
            dialog.dialogueText = text;
            dialog.StartSpanishDialogue();
        }
        else if (goodFood == 10)
        
        {
            dialog.spanishLines = new string[] { "Excelente trabajo, con la comida que has salvado se van a poder alimentar 50 familias\r\n" };
            dialog.dialoguePanel = panel;
            dialog.dialogueText = text;
            dialog.StartSpanishDialogue();
        }
        else if (badFood == 2)
        {
            dialog.spanishLines = new string[] { "¿Cuidado muchacho quieres volver a tener problemas? ¿Sabes cuanta gente podría alimentarse con esa comida que se ha echado a perder?\r\n" };
            dialog.dialoguePanel = panel;
            dialog.dialogueText = text;
            dialog.StartSpanishDialogue();
        }
    }

    public void badReminders()
    {
        if (badFood == 1)
        {
            dialog.spanishLines = new string[] { "¡Cuidado! Has dejado que se eche a perder una comida, recuerda que hay muchas personas que no tienen la oportunidad de tener algo en su mesa.\r\n" };
            dialog.dialoguePanel = panel;
            dialog.dialogueText = text;
            dialog.StartSpanishDialogue();
        }
        else if (badFood == 5)
        {
            dialog.spanishLines = new string[] { "Has permitido que se desperdicien cinco comidas. Este desperdicio podría haber alimentado a varias personas necesitadas. ¡Recuerda la importancia de no desperdiciar comida!\r\n" };
            dialog.dialoguePanel = panel;
            dialog.dialogueText = text;
            dialog.StartSpanishDialogue();
        }
        else if (badFood == 10)
        {
            dialog.spanishLines = new string[] { "¡Preocupante! Diez comidas se han echado a perder. Cada desperdicio es una oportunidad perdida para ayudar a quienes pasan hambre. Reflexiona sobre el impacto de tus acciones.\r\n"}; 
            dialog.dialoguePanel = panel;
            dialog.dialogueText = text;
            dialog.StartSpanishDialogue();
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
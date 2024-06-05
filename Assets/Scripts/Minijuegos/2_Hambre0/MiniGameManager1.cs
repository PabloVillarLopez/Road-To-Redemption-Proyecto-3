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
    public float dayTimeInSeconds = 60f;
    private float elapsedTime = 0f;
    public float temperatureChangePerSecond = 0.001f;
    public float temperature = 20;
    float duration = 300f;
    float cutoffThreshold = 12.5f;
    public int level;
    float totalTime = 900f;
    float elapsedTimeBuild = 0f;

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

 
    public GameObject panel;
    public TMPro.TextMeshProUGUI text;

    #endregion

    #region Unity Methods

    void Start()
    {
        CheckDialogue();
        greenHouse = GameObject.Find("GreenHouse");
        
        Level(1);
        spawnPosition = new Vector3(objectSpawner.transform.position.x, (objectSpawner.transform.position.y), objectSpawner.transform.position.z);
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
                StartCoroutine(UpdateCycleDaysCoroutine());

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
        

  



    }

    #endregion

    #region Object Spawning

    void SpawnObjectsOnSpawner()
    {

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

    private IEnumerator UpdateCycleDaysCoroutine()
    {
        while (true)
        {
            UpdateCycleDays();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void UpdateCycleDays()
    {
        elapsedTime += Time.deltaTime;
        float percentageOfDay = elapsedTime / dayTimeInSeconds;
        print (percentageOfDay);
        if (percentageOfDay >= 1f)
        {
            elapsedTime = 0f;
        }

        float rotationAngle = percentageOfDay * 360f;
        sunLight.transform.rotation = Quaternion.Euler(rotationAngle, 0f, 0f);

        if (percentageOfDay <= 0.5f)
        {
            Camera.main.backgroundColor = Color.Lerp(Color.blue, Color.black, percentageOfDay / 0.5f);
            AdjustTemperatureDuringDay();
            cultiveZone.GetComponent<CultiveZone>().day = true;
            Debug.Log("Es de dia");
            
        }
        else
        {
            Camera.main.backgroundColor = Color.Lerp(Color.black, Color.blue, (percentageOfDay - 0.5f) / 0.5f);
            AdjustTemperatureDuringNight();
            cultiveZone.GetComponent<CultiveZone>().day = false;
            Debug.Log("Es de noche");
        }


    }
    

    void AdjustTemperatureDuringDay()
    {
        temperature += temperatureChangePerSecond * Time.deltaTime;
        dayTime = true;
        // Aumentar la exposición del skybox durante el día
        float exposure = Mathf.Lerp(0.9f, 4f, elapsedTime / (dayTimeInSeconds / 4));
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
        DynamicGI.UpdateEnvironment(); // Actualiza la iluminación global basada en los cambios del skybox
    }

    void AdjustTemperatureDuringNight()
    {
        temperature -= temperatureChangePerSecond * Time.deltaTime;
        dayTime = false;
        // Disminuir la exposición del skybox durante la noche
        float exposure = Mathf.Lerp(0.9f, 4f, (elapsedTime - dayTimeInSeconds / 4) / (dayTimeInSeconds / 2));
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
        DynamicGI.UpdateEnvironment(); // Actualiza la iluminación global basada en los cambios del skybox
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
                fruit[0] = new object[] { "Fresa", 10, 30 };
                fruit[1] = new object[] { "Tomate", 12, 28 };
                fruit[2] = new object[] { "Pimiento", 5, 25 };

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
            originalMat = renderer.material; // Guarda el material original solo una vez
            renderer.material = temporaryMaterial; // Asigna el material temporal desde el Inspector
        }
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
                    break; // Opcional: detiene la corutina si no necesitas más cambios después de revertir el material
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

        // Opcional: Asegura que el material original se reestablezca al finalizar el tiempo total
        if (renderer != null && renderer.material != originalMat)
        {
            renderer.material = originalMat;
                Invoke("ChangeSceneMain", 15f);


                if (goodFood == 2)
                {
                    dialog.spanishLines = new string[] { "¡Increíble! Tu generosidad ha marcado la diferencia. Has contribuido a la meta de Hambre Cero, ayudando a crear un mundo donde nadie pase hambre.\r\n" };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartSpanishDialogue();
                }
                else if (goodFood == 5)
                {
                    dialog.spanishLines = new string[] { " ¿Sabías que cada comida salvada ayuda a reducir el desperdicio y a alimentar a quienes más lo necesitan?\r\n" };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartSpanishDialogue();
                }
                else if (goodFood == 10)
                {
                    dialog.spanishLines = new string[] { "¡Fantástico! Gracias a ti, 50 familias tendrán algo en sus mesas esta noche.\r\n" };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartSpanishDialogue();
                }
                else if (goodFood == 15)
                {
                    dialog.spanishLines = new string[] { "¡Impresionante! Ahora, gracias a ti, 75 familias tendrán algo que comer.\r\n" };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartSpanishDialogue();
                }
                else if (goodFood == 20)
                {
                    dialog.spanishLines = new string[] { "Increíble. ¡Has salvado suficiente comida para alimentar a 100 familias! \r\n" };
                    dialog.dialoguePanel = panel;
                    dialog.dialogueText = text;
                    dialog.StartSpanishDialogue();
                }

            }
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
        MinigamesCompleted.minigame2Finished = true;
    }

}
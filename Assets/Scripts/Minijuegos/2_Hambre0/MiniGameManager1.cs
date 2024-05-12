using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager1 : MonoBehaviour
{
    #region Variables

    [SerializeField] GameObject objectToSpawn;
    [SerializeField] GameObject objectSpawner;
    [SerializeField] GameObject cultiveZone;
    public float offSetYSpawn;
    public float offSetZSpawn;
    Vector3 spawnPosition;

    public float dayTimeInSeconds = 60f;
    public Light sunLight;

    private float elapsedTime = 0f;

    public float temperatureChangePerSecond = 0.1f;
    public float temperature = 20;
    private int badFood;
    private int goodFood;
    public Text TextBadFood;
    public Text TextGoodFood;
    public Text temperatureText;
    public Material temporaryMaterial;
    private Material originalMat; // Para almacenar el material original
    float duration = 300f; // Duración en segundos (5 minutos)
    float cutoffThreshold = 12.5f; // Umbral para revertir al material original
    public Skybox skybox;
    public int level;

    GameObject greenHouse;
    float totalTime = 300f; // Tiempo total de 5 minutos expresado en segundos
    float elapsedTimeBuild = 0f; // Tiempo transcurrido desde el inicio del proceso




    object[][] fruit = new object[3][];

    #endregion

    #region Unity Methods

    void Start()
    {

        greenHouse = GameObject.Find("GreenHouse");
        if (greenHouse != null)
        {
            StartCoroutine(AdjustMaterialsOverTime());
            Debug.Log("Encuentro");
        }
        Level(1);
        spawnPosition = new Vector3(objectSpawner.transform.position.x, (objectSpawner.transform.position.y), objectSpawner.transform.position.z);
        SpawnObjectsOnSpawner(objectToSpawn, 8, 0);
        SpawnOchards();
        

    }

    void Update()
    {
        UpdateCycleDays();


    }

    #endregion

    #region Object Spawning

    void SpawnObjectsOnSpawner(GameObject objectToSpawn, int amount, int typesToSpawn)
    {

        spawnPosition += new Vector3(0, -0.05F, -2);

        for (int i = 0; i < 3; i++)
        {
            spawnPosition += new Vector3(0, 0, offSetZSpawn);
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetId(1);
            }

        }
        spawnPosition += new Vector3(offSetZSpawn, 0, 0);
        for (int i = 0; i < 2; i++)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetId(1);
            }
            spawnPosition += new Vector3(0, 0, -offSetZSpawn);
        }

        spawnPosition += new Vector3(0, 0, 1);
        for (int i = 0; i < 3; i++)
        {
            spawnPosition += new Vector3(0, 0, offSetZSpawn);
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetId(2);
            }

        }
        spawnPosition += new Vector3(offSetZSpawn, 0, 0);
        for (int i = 0; i < 2; i++)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
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
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetId(3);
            }

        }
        spawnPosition += new Vector3(offSetZSpawn, 0, 0);
        for (int i = 0; i < 2; i++)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
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




    #endregion

    #region Day-Night Cycle

    void UpdateCycleDays()
    {
        elapsedTime += Time.deltaTime;
        float percentageOfDay = elapsedTime / dayTimeInSeconds;

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

        temperatureText.text = temperature.ToString("F1") + "°C";

    }
    void UpdateDayNightCycle()
    {
        elapsedTime += Time.deltaTime;

        // Asumimos que el día tiene dos fases iguales: día y noche
        if (elapsedTime <= dayTimeInSeconds / 2)
        {
            AdjustTemperatureDuringDay();
        }
        else if (elapsedTime <= dayTimeInSeconds)
        {
            AdjustTemperatureDuringNight();
        }
        else
        {
            elapsedTime = 0f; // Reiniciar el ciclo día-noche
        }
    }

    void AdjustTemperatureDuringDay()
    {
        temperature += temperatureChangePerSecond * Time.deltaTime;

        // Aumentar la exposición del skybox durante el día
        float exposure = Mathf.Lerp(0.33f, 1.30f, elapsedTime / (dayTimeInSeconds / 4));
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
        DynamicGI.UpdateEnvironment(); // Actualiza la iluminación global basada en los cambios del skybox
    }

    void AdjustTemperatureDuringNight()
    {
        temperature -= temperatureChangePerSecond * Time.deltaTime;

        // Disminuir la exposición del skybox durante la noche
        float exposure = Mathf.Lerp(1.30f, 0.33f, (elapsedTime - dayTimeInSeconds / 4) / (dayTimeInSeconds / 2));
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
                fruit[0] = new object[] { "Tomato", 10, 30 };
                fruit[1] = new object[] { "Pepper", 12, 28 };
                fruit[2] = new object[] { "Lettuce", 5, 25 };

                break;
            case 2:
                temperatureChangePerSecond = 0.3f;
                // Lógica específica para el Nivel 2
                break;
            case 3:
                temperatureChangePerSecond = 0.4f;
                // Lógica específica para el Nivel 3
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
        }
    }
}
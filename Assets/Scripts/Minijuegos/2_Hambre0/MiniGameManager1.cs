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


    public int level;





    object[][] fruit = new object[3][];

    #endregion

    #region Unity Methods

    void Start()
    {


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
        Vector3 spawnPosition = new Vector3(5.0f, 0.24f, -3f);
        for (int i = 0; i < 3; i++)
        {


            // Asegurarse de que la posición de spawn esté en el suelo (opcional)
            int offSetZOchard = 5;

            spawnPosition += new Vector3(0, 0, offSetZOchard);

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

    void AdjustTemperatureDuringDay()
    {
        temperature += temperatureChangePerSecond * Time.deltaTime;
        
    }

    void AdjustTemperatureDuringNight()
    {
        temperature -= temperatureChangePerSecond * Time.deltaTime;
        
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

}
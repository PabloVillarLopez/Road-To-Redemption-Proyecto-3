using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager1 : MonoBehaviour
{
    #region Variables

    [SerializeField] GameObject objectToSpawn;
    [SerializeField] GameObject objectSpawner;

    public float offSetYSpawn;
    public float offSetZSpawn;
    Vector3 spawnPosition;

    public float dayTimeInSeconds = 60f;
    public Light sunLight;

    private float elapsedTime = 0f;

    public float temperatureChangePerSecond = 0.1f;
    private float temperature = 20;

    public Text Text1;
    public Text Text2;
    public Text Text3;

    public int level;

    private int Tomato;
    private int Lettuce;
    private int Carrot;

    #endregion

    #region Unity Methods

    void Start()
    {
        spawnPosition = new Vector3(objectSpawner.transform.position.x, (objectSpawner.transform.position.y + offSetYSpawn), objectSpawner.transform.position.z);
        SpawnObjectsOnSpawner(objectToSpawn, 3, 0);
        level = 0;
        UpdateAttributesByLevel();
    }

    void Update()
    {
        UpdateCycleDays();
        Debug.Log("Temperature " + temperature);
    }

    #endregion

    #region Object Spawning

    void SpawnObjectsOnSpawner(GameObject objectToSpawn, int amount, int typesToSpawn)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetInfo(2);
            }
            spawnPosition += new Vector3(0, 0, offSetZSpawn);
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            ObjectInfo info = spawnedObject.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.SetInfo(0);
            }
            spawnPosition += new Vector3(offSetZSpawn, 0, offSetZSpawn);
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
        }
        else
        {
            Camera.main.backgroundColor = Color.Lerp(Color.black, Color.blue, (percentageOfDay - 0.5f) / 0.5f);
            AdjustTemperatureDuringNight();
        }
    }

    void AdjustTemperatureDuringDay()
    {
        temperature += temperatureChangePerSecond * Time.deltaTime;
        Debug.Log("Temperature during day: " + temperature);
    }

    void AdjustTemperatureDuringNight()
    {
        temperature -= temperatureChangePerSecond * Time.deltaTime;
        Debug.Log("Temperature during night: " + temperature);
    }

    #endregion

    #region Inventory Management

    public void AddTomatoToInventory()
    {
        Tomato += 1;
        Debug.Log("Tomato added to inventory");
        UpdateAttributesByLevel();
    }

    public void AddLettuceToInventory()
    {
        Lettuce += 1;
        Debug.Log("Lettuce added to inventory");
        UpdateAttributesByLevel();
    }

    public void AddCarrotToInventory()
    {
        Carrot += 1;
        Debug.Log("Carrot added to inventory");
        UpdateAttributesByLevel();
    }

    void UpdateAttributesByLevel()
    {
        switch (level)
        {
            case 0:
                Text1.text = "Tomato: " + Tomato;
                Text2.text = "Lettuce: " + Lettuce;
                Text3.text = "Carrot: " + Carrot;
                break;
            case 1:
                // Additional cases for more levels
                break;
            case 2:
                // Additional cases for more levels
                break;
            default:
                break;
        }
    }

    #endregion
}

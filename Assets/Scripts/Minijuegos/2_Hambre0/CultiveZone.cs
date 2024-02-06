using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CultiveZone : MonoBehaviour
{
    public Text nameFruitT;
    public Text tempFruitT;
    public GameObject[] fruitObjects = new GameObject[5];
    public Text[] tempTextArray;

    public GameObject[] spawnPoints;


    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateTextValues();
    }

    public void SetFruitStats(string newName, int newTempMin, int newTempMax)
    {
        nameFruitT.text = newName;
        tempFruitT.text = newTempMin + "-" + newTempMax;
    }

    public void AddFruit(GameObject newFruit)
    {
        if (newFruit != null)
        {
            for (int i = 0; i < fruitObjects.Length; i++)
            {
                if (fruitObjects[i] == null)
                {
                    fruitObjects[i] = newFruit;
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning("Trying to add a null GameObject as fruit.");
        }
    }

    void UpdateTextValues()
    {
        for (int i = 0; i < fruitObjects.Length; i++)
        {
            if (fruitObjects[i] != null && tempTextArray[i] != null)
            {
                ObjectInfo objectInfo = fruitObjects[i].GetComponent<ObjectInfo>();
                if (objectInfo != null)
                {
                    if (float.TryParse(tempTextArray[i].text, out float tempValue))
                    {
                        objectInfo.temp = tempValue;
                    }
                    else
                    {
                        Debug.LogWarning("Failed to parse text to float for GameObject: " + fruitObjects[i].name);
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CultiveZone : MonoBehaviour
{
    public Text nameFruitT;
    public Text tempFruitT;
    
    public GameObject[] fruitObject = new GameObject[5];
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
            for (int i = 0; i < fruitObject.Length; i++)
            {
                if (fruitObject[i] == null)
                {
                    fruitObject[i] = newFruit;
                    
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
        // Itera a través de los objetos fruta y los textos de temperatura asociados
        for (int i = 0; i < fruitObject.Length; i++)
        {
            // Verifica si el objeto fruta y el texto de temperatura no son nulos
            if (fruitObject[i] != null && tempTextArray[i] != null)
            {
                // Obtiene el componente ObjectInfo del objeto fruta
                ObjectInfo objectInfo = fruitObject[i].GetComponent<ObjectInfo>();
                // Verifica si se encontró el componente ObjectInfo
                if (objectInfo != null)
                {
                    // Intenta convertir el texto a un valor flotante y asignarlo a la temperatura del objeto fruta
                    if (float.TryParse(tempTextArray[i].text, out float tempValue))
                    {
                        objectInfo.temp = tempValue;
                    }
                    else
                    {
                        // Advierte si no se pudo convertir el texto a un valor flotante
                        Debug.LogWarning("Failed to parse text to float for GameObject: " + fruitObject[i].name);
                    }
                }
            }
        }
    }

}

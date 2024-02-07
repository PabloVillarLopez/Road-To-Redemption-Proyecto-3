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
    public string state = "Neutral";
    public bool day;
    public MiniGameManager1 manager;

    public GameObject hot;
    public GameObject neutral;
    public GameObject cold;
    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<MiniGameManager1>();

    }

    private void Update()
    {
        UpdateTextValues();
        ApplyTemp(state);
    }

    public void SetFruitStats(string newName, int newTempMin, int newTempMax)
    {
        nameFruitT.text = newName;
        tempFruitT.text = newTempMin + "-" + newTempMax;
    }

    public void AddFruit()
    {
        
            for (int i = 0; i < 5; i++)
            {
                if (spawnPoints[i].transform.childCount > 0)
                {
                    fruitObject[i] = spawnPoints[i].transform.GetComponentInChildren<GameObject>();

                    break;

                }
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

    public void AddChild()
    {
        //spawnPoints[i] != null
        for (int i = 0; i < 5; i++)
        {
            if (spawnPoints[i].transform.childCount > 0)
            {
                fruitObject[i] = spawnPoints[i].transform.GetChild(i).gameObject;

            }


        }



    }

    public void ApplyTemp(string state)
    {
       

        
        bool active = true;

        // Mientras el proceso esté activado, actualiza la temperatura del objeto gradualmente
        if(active)
        {
            
            switch (state)
            {
                case "Hot":
                    for (int i = 0; i < fruitObject.Length; i++)
                    {
                        if(fruitObject[i] != null)
                        {
                            fruitObject[i].GetComponent<ObjectInfo>().temp += Time.deltaTime;

                        }
                    }
                    break;
                case "Neutral":
                    
                    
                    
                    break;
                case "Cold":
                    
                    for (int i = 0; i < fruitObject.Length; i++)
                    {
                        if (fruitObject[i] != null)
                        {
                            fruitObject[i].GetComponent<ObjectInfo>().temp -= Time.deltaTime;

                        }
                    }
                    break;
                default:
                    break;
            }
  
        }

        if (day)
        {
            float temperatureGlobal = manager.temperatureChangePerSecond;
            for (int i = 0; i < fruitObject.Length; i++)
            {
                if (fruitObject[i] != null)
                {
                    fruitObject[i].GetComponent<ObjectInfo>().temp += temperatureGlobal;
                }
            }
        }
        else
        {
            float temperatureGlobal = manager.temperatureChangePerSecond;

            for (int i = 0; i < fruitObject.Length; i++)
            {
                if (fruitObject[i] != null)
                {
                    fruitObject[i].GetComponent<ObjectInfo>().temp -= temperatureGlobal;
                }
            }
        }



    }
}

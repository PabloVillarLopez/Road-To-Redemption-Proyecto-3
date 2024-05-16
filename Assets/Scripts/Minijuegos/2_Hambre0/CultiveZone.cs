using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.VersionControl.Asset;

public class CultiveZone : MonoBehaviour
{
    private float applyTempMan=1f;
    private float speedTempGlobal=0.05f;

    public Text nameFruitT;
    public Text tempFruitT;
    public List<GameObject> fruitObjects = new List<GameObject>();
    private int counterList;
  
    public Text[] tempTextArray;
    public Slider[] health;
    public GameObject[] spawnPoints;
 
    public bool day;
    public MiniGameManager1 manager;
    public string state ="";
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
        ApplyTemp();

     }

    public void SetFruitStats(string newName, int newTempMin, int newTempMax)
    {
        nameFruitT.text = newName;
        tempFruitT.text = newTempMin + "-" + newTempMax;
    }


    void UpdateTextValues()
    {

        // Itera a través de los objetos fruta y los textos de temperatura asociados
        for (int i = 0; i < fruitObjects.Count; i++)
        {
            // Verifica si el objeto fruta y el texto de temperatura no son nulos
            if (fruitObjects[i] != null && tempTextArray[i] != null && health[i] != null)
            {
                
                // Obtiene el componente ObjectInfo del objeto fruta
                ObjectInfo objectInfo = fruitObjects[i].GetComponent<ObjectInfo>();
                // Verifica si se encontró el componente ObjectInfo
                if (objectInfo != null)
                {
                    // Intenta convertir el texto a un valor flotante y asignarlo a la temperatura del objeto fruta

                    tempTextArray[i].text = objectInfo.temp.ToString("0") + "ºC";
                    float porcentaje = (objectInfo.timeLife/20);
                    health[i].GetComponent<Slider>().value = porcentaje;
                    // Advierte si no se pudo convertir el texto a un valor flotante


                }
            }
            
        }
        
    }

    public void AddChild(GameObject newFruit)
    {
       
        if (newFruit != null)
        {
            fruitObjects.Add(newFruit);

            counterList++;
            newFruit.GetComponent<ObjectInfo>().ready = true;
            newFruit.GetComponent<ObjectInfo>().temp= Random.Range(20, 25);
        }
       
        

    }


    public void RemoveChild (GameObject oldFruit)
    {
        if (oldFruit != null)
        {
            fruitObjects.Remove(oldFruit);
            counterList--;
        }
    }

    public void ApplyTemp()
    {

        if(manager.dayTime == true)
        {
            day = true;     
         }
        else
        {
            day = false;
        }

        switch (state)
        {
            case "Hot":
                for (int i = 0; i < fruitObjects.Count; i++)
                {
                    if (fruitObjects[i] != null)
                    {
                        fruitObjects[i].GetComponent<ObjectInfo>().temp += (applyTempMan * Time.deltaTime);
                        print (applyTempMan * Time.deltaTime);
                        fruitObjects[i].GetComponent<ObjectInfo>().temp = Mathf.Clamp(fruitObjects[i].GetComponent<ObjectInfo>().temp, -3, 35);
                    }
                }
                Debug.Log("Hot");
                break;
            case "Neutral":
                Debug.Log("Neutral");
                Debug.Log(day);
                break;
            case "Cold":
                for (int i = 0; i < fruitObjects.Count; i++)
                {
                    if (fruitObjects[i] != null)
                    {
                        fruitObjects[i].GetComponent<ObjectInfo>().temp -= (applyTempMan * Time.deltaTime);
                        fruitObjects[i].GetComponent<ObjectInfo>().temp = Mathf.Clamp(fruitObjects[i].GetComponent<ObjectInfo>().temp, -3, 35);
                        print(applyTempMan * Time.deltaTime);
                        Debug.Log(fruitObjects[i].GetComponent<ObjectInfo>().temp);
                    }
                }
                Debug.Log("Cold");
                break;
            default:
                break;
        }

        if (day)
        {
            float temperatureGlobal = manager.temperature;
            for (int i = 0; i < fruitObjects.Count; i++)
            {
                if (fruitObjects[i] != null)
                {
                    fruitObjects[i].GetComponent<ObjectInfo>().temp += speedTempGlobal *  + Time.deltaTime;
                    fruitObjects[i].GetComponent<ObjectInfo>().temp = Mathf.Clamp(fruitObjects[i].GetComponent<ObjectInfo>().temp, -3, 35);
                    print(fruitObjects[i].GetComponent<ObjectInfo>().temp);

                }
                else if (fruitObjects[i] == null)
                {
                    fruitObjects.Remove(fruitObjects[i]);
                }
            }
        }
        else
        {
            float temperatureGlobal = manager.temperature;

            for (int i = 0; i < fruitObjects.Count; i++)
            {
                if (fruitObjects[i] != null)
                {
                    fruitObjects[i].GetComponent<ObjectInfo>().temp -= speedTempGlobal * +Time.deltaTime;
                    fruitObjects[i].GetComponent<ObjectInfo>().temp = Mathf.Clamp(fruitObjects[i].GetComponent<ObjectInfo>().temp, -3, 35);
                    

                }

            }
        }


    }

   
    
}

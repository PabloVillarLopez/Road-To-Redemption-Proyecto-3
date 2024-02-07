using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.VersionControl.Asset;

public class CultiveZone : MonoBehaviour
{
    [SerializeField] float applyTempMan=4;
    [SerializeField] float speedTempGlobal=0.05f;

    public Text nameFruitT;
    public Text tempFruitT;
    public List<GameObject> fruitObjects = new List<GameObject>();
    private int counterList;
  
    public Text[] tempTextArray;
    
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

    public void AddFruit()
    {
        
            for (int i = 0; i < 5; i++)
            {
                if (spawnPoints[i].transform.childCount > 0)
                {
                    fruitObjects[i] = spawnPoints[i].transform.GetComponentInChildren<GameObject>();

                    break;

                }
            }
        
    }

    void UpdateTextValues()
    {
        // Itera a través de los objetos fruta y los textos de temperatura asociados
        for (int i = 0; i < fruitObjects.Count; i++)
        {
            // Verifica si el objeto fruta y el texto de temperatura no son nulos
            if (fruitObjects[i] != null && tempTextArray[i] != null)
            {
                
                // Obtiene el componente ObjectInfo del objeto fruta
                ObjectInfo objectInfo = fruitObjects[i].GetComponent<ObjectInfo>();
                // Verifica si se encontró el componente ObjectInfo
                if (objectInfo != null)
                {
                    // Intenta convertir el texto a un valor flotante y asignarlo a la temperatura del objeto fruta

                    tempTextArray[i].text = objectInfo.temp.ToString("0") + "ºC";

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
        }
       
        

    }

    public void ApplyTemp()
    {

      

        switch (state)
            {
                case "Hot":
                    for (int i = 0; i < fruitObjects.Count; i++)
                    {
                        if(fruitObjects[i] != null)
                        {
                            fruitObjects[i].GetComponent<ObjectInfo>().temp += (applyTempMan * Time.deltaTime);

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
            float temperatureGlobal = manager.temperatureChangePerSecond;
            for (int i = 0; i < fruitObjects.Count; i++)
            {
                if (fruitObjects[i] != null)
                {
                    fruitObjects[i].GetComponent<ObjectInfo>().temp += speedTempGlobal * temperatureGlobal;
                }
            }
        }
        else
        {
            float temperatureGlobal = manager.temperatureChangePerSecond;

            for (int i = 0; i < fruitObjects.Count; i++)
            {
                if (fruitObjects[i] != null)
                {
                    fruitObjects[i].GetComponent<ObjectInfo>().temp -= speedTempGlobal * temperatureGlobal;
                }
            }
        }

        
    }

   
    
}

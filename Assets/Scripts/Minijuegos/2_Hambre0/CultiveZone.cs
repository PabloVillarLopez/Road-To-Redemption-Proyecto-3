using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CultiveZone : MonoBehaviour
{
    private float applyTempMan=80f;
    private float speedTempGlobal=30f;

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
        StartCoroutine(ApplyTempCoroutine());
        StartCoroutine(UpdateTempCoroutine());

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


    void UpdateTextValues()
    {
        // Itera a través de los objetos fruta y los textos de temperatura asociados
        for (int i = 0; i < fruitObjects.Count; i++)
        {
            // Verifica si todos los objetos necesarios no son nulos
            if (fruitObjects[i] != null && tempTextArray[i] != null && health[i] != null)
            {
                // Obtiene el componente ObjectInfo del objeto fruta
                ObjectInfo objectInfo = fruitObjects[i].GetComponent<ObjectInfo>();

                // Verifica si se encontró el componente ObjectInfo
                if (objectInfo != null)
                {
                    // Actualiza el texto de temperatura y el valor del slider de salud
                    tempTextArray[i].text = objectInfo.temp.ToString("0") + "ºC";
                    float porcentaje = objectInfo.timeLife / 20f;
                    health[i].GetComponent<Slider>().value = porcentaje;
                }
                else
                {
                    tempTextArray[i].text = objectInfo.temp.ToString("");
                    health[i].GetComponent<Slider>().value = 0;


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
       if (newFruit != null && newFruit.transform.childCount > 0)
        {
            bool firstChild = true;

            foreach (Transform child in newFruit.transform)
            {
                if (firstChild)
                {
                    firstChild = false;
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        

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



    private IEnumerator ApplyTempCoroutine()
    {
        while (true)
        {
            ApplyTemp();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator UpdateTempCoroutine()
    {
        while (true)
        {
            UpdateTextValues();
            yield return new WaitForSeconds(0.5f);
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
                        fruitObjects[i].GetComponent<ObjectInfo>().temp = Mathf.Clamp(fruitObjects[i].GetComponent<ObjectInfo>().temp, -3, 35);
                    }
                }
              
                break;
            case "Neutral":
            
                break;
            case "Cold":
                for (int i = 0; i < fruitObjects.Count; i++)
                {
                    if (fruitObjects[i] != null)
                    {
                        fruitObjects[i].GetComponent<ObjectInfo>().temp -= (applyTempMan * Time.deltaTime);
                        fruitObjects[i].GetComponent<ObjectInfo>().temp = Mathf.Clamp(fruitObjects[i].GetComponent<ObjectInfo>().temp, -3, 35);
                        Debug.Log(fruitObjects[i].GetComponent<ObjectInfo>().temp);
                    }
                }
              
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

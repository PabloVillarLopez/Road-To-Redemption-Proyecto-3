using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    public MiniGameManager1 miniGameManager;

    public bool ready;
    private int id;
    public float timeLife;
    private float minTemp;
    private float maxTemp;
    public float speedLoseLife;
    public float temp = 20f;
    public float timeToCollect;
    public Material[] materials = new Material[3]; // Array of materials
    public GameObject Cultive;
    

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        miniGameManager = GameObject.Find("GameManager").GetComponent<MiniGameManager1>();

        switch (id)
        {
            case 1:
                SetFruitStats("Tomato", 10, 30);
                timeLife = 10f;
                renderer.material = materials[0];
                break;
            case 2:
                SetFruitStats("Pepper", 12, 28);
                timeLife = 10f;
                renderer.material = materials[1];
                break;
            case 3:
                SetFruitStats("Lettuce", 5, 25);
                timeLife = 10f;
                renderer.material = materials[2];
                break;
            case 4:
                SetFruitStats("Carrot", 8, 25);
                timeLife = 7f;
                break;
            case 5:
                SetFruitStats("Spinach", 8, 25);
                timeLife = 7f;
                break;
            case 6:
                SetFruitStats("Broccoli", 10, 25);
                timeLife = 7f;
                break;
            case 7:
                SetFruitStats("Onion", 10, 25);
                timeLife = 5f;
                break;
            case 8:
                SetFruitStats("Pumpkin", 12, 30);
                timeLife = 5f;
                break;
            case 9:
                SetFruitStats("Cucumber", 12, 30);
                timeLife = 5f;
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (ready)
        {
            VerifyTemp();
        }
    }

    void SetFruitStats(string newName, int newTempMin, int newTempMax)
    {
        name = newName;
        minTemp = newTempMin;
        maxTemp = newTempMax;
    }

    public void SetId(int newId)
    {
        id = newId;
    }

    // Function that returns an integer
    public int GetobjectInfo()
    {
        return id;
    }

    void VerifyTemp()
    {
        if (miniGameManager != null)
        {
            float temperature = miniGameManager.temperature;

            // Verificar si temp está fuera del rango
            if (temp < minTemp || temp > maxTemp)
            {

                // Reducir el tiempo de vida y verificar si llega a cero
                timeLife -= speedLoseLife * Time.deltaTime;

                if (timeLife <= 0)
                {
                    miniGameManager.checkBadFood();
                    gameObject.SetActive(false);
                    Cultive.GetComponent<CultiveZone>().RemoveChild(gameObject);
                }
            }
            else {
                timeToCollect += speedLoseLife * Time.deltaTime;
                    
                 }

            if (timeToCollect >= 10)
            {
                miniGameManager.checkGoodFood();

                gameObject.SetActive(false);
                
            }
        }
    }

}

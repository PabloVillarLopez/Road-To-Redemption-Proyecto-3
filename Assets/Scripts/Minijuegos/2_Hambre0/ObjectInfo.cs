using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    public MiniGameManager1 miniGameManager;

    private bool ready;
    private int id;
    private float timeLife;
    private float minTemp;
    private float maxTemp;
    private float speedLoseLife;
    public float temp = 20f;
    public Material[] materials = new Material[3]; // Array of materials
    public GameObject Cultive;
    private GameObject self;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        self = gameObject;

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

    void VerificarTemperatura()
    {
        float temperature = miniGameManager.temperature;
        if (temperature < minTemp || temperature > maxTemp)
        {
            float loseLife = speedLoseLife * Time.deltaTime;
            timeLife -= speedLoseLife;
            if (timeLife <= 0)
            {
                miniGameManager.checkBadFood();
                gameObject.SetActive(false);
            }
        }
    }

    public void AssignToCultive()
    {
        if (Cultive != null)
        {
            self.SetActive(true);
            CultiveZone cultiveZone = Cultive.GetComponent<CultiveZone>();
            if (cultiveZone != null)
            {
                cultiveZone.AddFruit(self);
            }
            else
            {
                Debug.LogWarning("CultiveZone component not found on Cultive GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Cultive GameObject is not assigned.");
        }
    }
}

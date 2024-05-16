using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    public MiniGameManager1 miniGameManager;

    public bool ready;
    public int id;
    public float timeLife;
    private float minTemp;
    private float maxTemp;
    private float speedLoseLife=0.1f;
    public float temp = 20f;
    public float timeToCollect;
    public Material[] materials = new Material[3]; // Array of materials
    public GameObject Cultive;
    public MeshRenderer renderer;
    public MeshFilter mesh;

    public Mesh[] meshStrawBerry = new Mesh[2];
    public Mesh[] meshTomatoe = new Mesh[2];
    public Mesh[] meshPepper = new Mesh[2];


    public Material[] materialStrawBerry = new Material[2];
    public Material[] materialTomatoe = new Material[2];
    public Material[] materialPepper = new Material[2];


    public string Tipo;
    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>();
    }

    void Start()
    {
         
        miniGameManager = GameObject.Find("GameManager").GetComponent<MiniGameManager1>();

        switch (id)
        {
            case 1:
                SetFruitStats("Pimiento", 10, 30);
                timeLife = 20f;
                break;
            case 2:
                SetFruitStats("Tomate", 12, 28);
                timeLife = 20f;
                break;
            case 3:
                SetFruitStats("Fresa", 5, 25);
                timeLife = 20f;
               
                break;
            case 4:
                SetFruitStats("Carrot", 8, 25);
                timeLife = 7f;
                break;
            case 5:
                Debug.Log("Heloude");
                break;
            case 6:
                break;
            case 7:
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

            if (timeToCollect >= 5)
            {
                switch (id)
                {
                    case 3:
                        mesh.mesh = meshStrawBerry[0];
                        renderer.material = materialStrawBerry[0];  
                        break;

                    case 2: 
                        mesh.mesh = meshTomatoe[0];
                        renderer.material = materialTomatoe[0];
                        break;

                case 1:     
                        mesh.mesh = meshPepper[0];
                        renderer.material = materialPepper[0];
                        break;

                }

            }
            if (timeToCollect >= 10)
            { 
            
                 switch (id)
                    {
                        case 3:
                            mesh.mesh = meshStrawBerry[1];
                            renderer.material = materialStrawBerry[1];
                            break;

                        case 2:
                            mesh.mesh = meshTomatoe[1];
                            renderer.material = materialTomatoe[1];
                            break;

                        case 1:
                            mesh.mesh = meshPepper[1];
                            renderer.material = materialPepper[1];
                            break;

                    }
      
            
            }

        }
    }

    public void Recollect()
    {
        miniGameManager.checkGoodFood();

        gameObject.SetActive(false);
    }
    
}

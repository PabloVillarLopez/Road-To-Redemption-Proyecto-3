using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    public MiniGameManager1 miniGameManager;

    public bool ready;
    public int id;
    public float timeLife;
    private float minTemp;
    private float maxTemp;
    private float speedLoseLife=10f;
    public float temp = 20f;
    public float timeToCollect;
    public Material[] materials = new Material[3]; // Array of materials
    public GameObject Cultive;
    public new MeshRenderer renderer;
    public MeshFilter mesh;


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

            case 0:
                SetFruitStats("Fresa", 10, 30);
                timeLife = 20f;
                break;
            case 1:
                SetFruitStats("Pimiento", 10, 30);
                timeLife = 20f;
                break;
            case 2:
                SetFruitStats("Tomate", 12, 28);
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

        ActiveChild(0);

        StartCoroutine(ApplyTempCoroutine());

    }
    private void Update()
    {
       
           
      
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

    private System.Collections.IEnumerator ApplyTempCoroutine()
    {
        while (true)
        {
            if (ready) { VerifyTemp(); }
            
            yield return new WaitForSeconds(0.5f);
        }
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

                ActiveChild(1);

            }
            if (timeToCollect >= 10)
            {

               

                ActiveChild(2);
            }

        }
    }

    public void Recollect()
    {
        miniGameManager.checkGoodFood();

        gameObject.SetActive(false);
    }
 
    private void ActiveChild(int childnumber)
    {
        int childcount = 0;
        foreach (Transform child in gameObject.transform)
        {
            if (childcount == childnumber)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
            childcount++;
        }
    }


}

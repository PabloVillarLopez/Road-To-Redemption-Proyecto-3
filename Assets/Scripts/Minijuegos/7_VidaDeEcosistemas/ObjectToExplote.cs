using Unity.Burst.CompilerServices;
using UnityEngine;

public class ObjectToExplote : MonoBehaviour
{
    public Material[] materials = new Material[3];
    public MeshRenderer rendererMat;

    private float maxHealth = 15f;
    private float currentHealth = 200f;

    public GameObject GameManager;
    private MiniGameManager7 miniGameManager;

    private void Start()
    {
        currentHealth = maxHealth;
        GameManager = GameObject.Find("GameManager");

        if (GameManager != null)
        {
            miniGameManager = GameManager.GetComponent<MiniGameManager7>();
            if (miniGameManager == null)
            {
                Debug.LogWarning("El componente MiniGameManager7 no está presente en el GameManager.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el GameManager en la escena.");
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(currentHealth.ToString());
        if (currentHealth <= 0)
        {

            Explode();
        }
    }

    private void Explode()
    {
        if (miniGameManager != null)
        {
           
            miniGameManager.removeScrap(gameObject);
        }
        else
        {
            Debug.LogWarning("No se puede acceder al componente MiniGameManager7.");
        }

        Destroy(gameObject);
    }


}

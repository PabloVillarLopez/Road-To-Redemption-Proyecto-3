using UnityEngine;

public class Container : MonoBehaviour
{
    public int containerID; // ID del contenedor
    public GameObject miniManager;
    private void Awake()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject miniManager = GameObject.Find("GameManager");
        // Obtener el componente TrashInfo del objeto que colisionó
        TrashInfo trashInfo = collision.gameObject.GetComponent<TrashInfo>();

        // Verificar si el objeto tiene un TrashInfo y si su ID coincide con el ID del contenedor
        if (trashInfo != null && trashInfo.GetId() == containerID)
        {
            miniManager.GetComponent<MiniGameManager8>().AddPoints(); // Suponiendo que AddPoints requiere un parámetro de cantidad

            // Aquí puedes agregar la lógica adicional que desees cuando la ID coincida
        }
        else
        {
            miniManager.GetComponent<MiniGameManager8>().SubtractPoints();
        }

        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("MainCamera"))
        {
            Destroy(collision.gameObject);
        }

        
    }
}

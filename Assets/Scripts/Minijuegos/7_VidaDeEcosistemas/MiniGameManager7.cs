using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager7 : MonoBehaviour
{

    public GameObject[] objectsToSpawn; 
    public Vector3 spawnArea; 
    public int phasesProcess;
    public GameObject[] ManageGameObjects ;
    private int counterGameObject = 0;
    private int countItems;
    public int countProgress=15;
    void Start()
    {

        Phases(phasesProcess);


    }

    private void Update()
    {
        
    }

    void SpawnObject()
    {
        // Genera una posición aleatoria dentro del área definida
        Vector3 randomPosition = new Vector3(
            transform.position.x + Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            transform.position.y + 1,
            transform.position.z + Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
        );

        // Selecciona un objeto aleatorio del array
        GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        // Spawnear el objeto en la posición generada
        Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        ManageGameObjects[counterGameObject] = objectToSpawn;
        counterGameObject++;
    }




    private void Phases(int phasesProcess)
    {
        switch (phasesProcess)
        {
            case 1:
                for (int i = 0; i < 30; i++)
                {
                    SpawnObject();

                }

                break;
            case 2:

                foreach (GameObject obj in objectsToSpawn)
                {
                    // Haz algo con cada objeto, por ejemplo, imprime su nombre
                    Destroy(obj);
                }


                // Código a ejecutar si la expresión es igual a valor2
                break;
            // Puedes tener más casos si es necesario
            default:
                // Código a ejecutar si la expresión no coincide con ninguno de los casos anteriores
                break;
        }
    }

}

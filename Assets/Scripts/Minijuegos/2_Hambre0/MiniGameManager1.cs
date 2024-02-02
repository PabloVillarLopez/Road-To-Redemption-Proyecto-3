using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager1 : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn; // Objeto a spawnear
    [SerializeField] GameObject objectSpawner; // Objeto donde Spawnear

    public float offSetYSpawn;
    public float offSetZSpawn;
    Vector3 spawnPosition;

    void Start()
    {
        // Calcula la posición de spawn para cada objeto en el bucle
         spawnPosition = new Vector3(objectSpawner.transform.position.x, (objectSpawner.transform.position.y + offSetYSpawn), objectSpawner.transform.position.z);

        // Llama a la función para spawnear un objeto sobre el spawner
        SpawnObjectsOnSpawner(objectToSpawn, 3,0);


        StartCoroutine(MyCoroutine());

    }


    // Update is called once per frame
    void Update()
    {
        
    }




    void SpawnObjectsOnSpawner(GameObject objectToSpawn,int amount, int typesToSpawn)
    {
        

        for (int i = 0; i < amount; i++)
        {
            // Spawnear el objeto sobre la posición calculada
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            // Sumar el offset a la posición para el siguiente spawn
            spawnPosition += new Vector3(0, 0, offSetZSpawn);
        }
    }





    IEnumerator MyCoroutine()
    {
        Debug.Log("La corrutina ha comenzado.");

        // Espera 2 segundos
        yield return new WaitForSeconds(2);

        Debug.Log("Han pasado 2 segundos.");

        // Espera 3 segundos
        yield return new WaitForSeconds(3);

        Debug.Log("Han pasado 3 segundos.");

        // Puedes seguir agregando más instrucciones o bucles dentro de la corrutina según sea necesario

        // Finaliza la corrutina
        Debug.Log("La corrutina ha terminado.");
    }




}

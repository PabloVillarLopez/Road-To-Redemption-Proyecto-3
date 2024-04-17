using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToCatch : MonoBehaviour
{
    public int id;
    public Mesh[] meshArray;

    private MiniGameManager4 gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<MiniGameManager4>();
        id = Random.Range(1, 5);
        switch (id)
        {
            case 0:
                // No hacer nada si la ID es 0
                break;

            case 1:
                // Asignar la primera mesh del array
                GetComponent<MeshFilter>().mesh = meshArray[0];
                break;

            case 2:
                // Asignar la segunda mesh del array
                GetComponent<MeshFilter>().mesh = meshArray[1];
                break;

            case 3:
                // Asignar la tercera mesh del array
                GetComponent<MeshFilter>().mesh = meshArray[2];
                break;

            case 4:
                // Asignar la cuarta mesh del array
                GetComponent<MeshFilter>().mesh = meshArray[3];
                break;

            default:
                Debug.LogError("ID no válida");
                break;
        }
    }

    void Update()
    {
        // Código de actualización si es necesario
    }
}

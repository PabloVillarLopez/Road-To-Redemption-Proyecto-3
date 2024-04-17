using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Cablebox : MonoBehaviour
{
    public List<Material> materials;
    public List<GameObject> Part1GameObject;
    public List<GameObject> Part2GameObject;
    private List<int> idUsed = new List<int>();

    public Camera cam;

    public int arraySize = 4; // Tamaño del array
    public int minValue = 0; // Valor mínimo
    public int maxValue = 2; // Valor máximo

    public LineRenderer lineRendererPrefab; // Prefab de la línea
    private LineRenderer currentLine; // Referencia a la línea actualmente activa
    public Image cursor;
    bool dragging = false; // Booleano que indica si se está arrastrando

    private MiniGameManager4 gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<MiniGameManager4>();
        AssingColorsPartLeft();
        AssingColorsPartRight();
        Cursor.visible = true;
    }


    void Update()
    {
        if (cam.enabled)
        {
            Cursor.lockState = CursorLockMode.None;

            // Raycast desde la cámara hacia el plano XY (2D)
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePositionScreen = Input.mousePosition;

            // Convertir la posición del ratón a coordenadas del mundo
            Vector3 mousePositionWorld = cam.ScreenToWorldPoint(new Vector3(mousePositionScreen.x, mousePositionScreen.y, 8f));

            // Lanzar un rayo hacia atrás desde la posición del cursor
            if (Physics.Raycast(mousePositionWorld, Vector3.right, out RaycastHit hit))
            {
                // Verificar si el rayo golpeó algún objeto
                Debug.Log("Objeto detectado: " + hit.collider.gameObject.name);
                // Aquí puedes realizar cualquier acción que desees con el objeto detectado

                // Si se hace clic y se mantiene en el objeto, activar el arrastre
                if (Input.GetMouseButtonDown(0))
                {
                    dragging = true;

                    // Verificar si el objeto tiene un componente LineRenderer antes de intentar usarlo
                    LineRenderer lineRenderer = hit.collider.gameObject.GetComponent<LineRenderer>();
                    if (lineRenderer != null)
                    {
                        // Establecer la posición inicial del line renderer
                        lineRenderer.SetPosition(0, hit.collider.transform.position);
                    }
                }
            }

            // Si se está arrastrando, actualizar la posición del line renderer
            if (dragging)
            {
                // Actualizar la posición del line renderer para que siga al ratón
                LineRenderer lineRenderer = hit.collider.gameObject.GetComponent<LineRenderer>();
                if (lineRenderer != null)
                {
                    lineRenderer.SetPosition(1, mousePositionWorld);
                }
            }

            cursor.transform.position = mousePositionWorld;

            // Si se suelta el botón del mouse, detener el arrastre
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }
        }
    }


    int[] GenerateRandomIntArray()
    {
        // Crear una lista para almacenar los números disponibles
        List<int> availableNumbers = new List<int>();

        // Llenar la lista con los números posibles
        for (int i = minValue; i <= maxValue; i++)
        {
            availableNumbers.Add(i);
        }

        // Crear un nuevo array de ints
        int[] randomArray = new int[arraySize];

        // Llenar el array con valores aleatorios
        for (int i = 0; i < arraySize; i++)
        {
            // Obtener un índice aleatorio en la lista de números disponibles
            int randomIndex = Random.Range(0, availableNumbers.Count);

            // Asignar el número aleatorio al array
            randomArray[i] = availableNumbers[randomIndex];

            // Eliminar el número utilizado de la lista
            availableNumbers.RemoveAt(randomIndex);
        }

        return randomArray;
    }

    void AssingColorsPartLeft()
    {
        int[] randomArray = GenerateRandomIntArray();

        for (int i = 0; i < Part1GameObject.Count; i++)
        {
            Part1GameObject[i].GetComponent<MeshRenderer>().material = materials[randomArray[i]];
        }
    }

    void AssingColorsPartRight()
    {
        int[] randomArray = GenerateRandomIntArray();

        for (int i = 0; i < Part2GameObject.Count; i++)
        {
            Part2GameObject[i].GetComponent<MeshRenderer>().material = materials[randomArray[i]];
        }
    }

    public void HandleCableClick(Transform cableTransform)
    {
        currentLine = Instantiate(lineRendererPrefab, cableTransform.position, Quaternion.identity);
        currentLine.SetPosition(0, cableTransform.position);

        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        currentLine.SetPosition(1, mousePosition);
    }
}

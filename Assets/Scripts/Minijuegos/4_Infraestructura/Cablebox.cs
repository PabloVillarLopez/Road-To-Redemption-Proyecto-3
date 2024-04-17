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

    public int arraySize = 4; // Tama�o del array
    public int minValue = 0; // Valor m�nimo
    public int maxValue = 2; // Valor m�ximo

    public LineRenderer lineRendererPrefab; // Prefab de la l�nea
    private LineRenderer currentLine; // Referencia a la l�nea actualmente activa
    public Image cursor;
    bool dragging = false; // Booleano que indica si se est� arrastrando

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

            // Raycast desde la c�mara hacia el plano XY (2D)
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePositionScreen = Input.mousePosition;

            // Convertir la posici�n del rat�n a coordenadas del mundo
            Vector3 mousePositionWorld = cam.ScreenToWorldPoint(new Vector3(mousePositionScreen.x, mousePositionScreen.y, 8f));

            // Lanzar un rayo hacia atr�s desde la posici�n del cursor
            if (Physics.Raycast(mousePositionWorld, Vector3.right, out RaycastHit hit))
            {
                // Verificar si el rayo golpe� alg�n objeto
                Debug.Log("Objeto detectado: " + hit.collider.gameObject.name);
                // Aqu� puedes realizar cualquier acci�n que desees con el objeto detectado

                // Si se hace clic y se mantiene en el objeto, activar el arrastre
                if (Input.GetMouseButtonDown(0))
                {
                    dragging = true;

                    // Verificar si el objeto tiene un componente LineRenderer antes de intentar usarlo
                    LineRenderer lineRenderer = hit.collider.gameObject.GetComponent<LineRenderer>();
                    if (lineRenderer != null)
                    {
                        // Establecer la posici�n inicial del line renderer
                        lineRenderer.SetPosition(0, hit.collider.transform.position);
                    }
                }
            }

            // Si se est� arrastrando, actualizar la posici�n del line renderer
            if (dragging)
            {
                // Actualizar la posici�n del line renderer para que siga al rat�n
                LineRenderer lineRenderer = hit.collider.gameObject.GetComponent<LineRenderer>();
                if (lineRenderer != null)
                {
                    lineRenderer.SetPosition(1, mousePositionWorld);
                }
            }

            cursor.transform.position = mousePositionWorld;

            // Si se suelta el bot�n del mouse, detener el arrastre
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }
        }
    }


    int[] GenerateRandomIntArray()
    {
        // Crear una lista para almacenar los n�meros disponibles
        List<int> availableNumbers = new List<int>();

        // Llenar la lista con los n�meros posibles
        for (int i = minValue; i <= maxValue; i++)
        {
            availableNumbers.Add(i);
        }

        // Crear un nuevo array de ints
        int[] randomArray = new int[arraySize];

        // Llenar el array con valores aleatorios
        for (int i = 0; i < arraySize; i++)
        {
            // Obtener un �ndice aleatorio en la lista de n�meros disponibles
            int randomIndex = Random.Range(0, availableNumbers.Count);

            // Asignar el n�mero aleatorio al array
            randomArray[i] = availableNumbers[randomIndex];

            // Eliminar el n�mero utilizado de la lista
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

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Cablebox : MonoBehaviour
{
    public List<Material> materials;
    public List<GameObject> Part1GameObject;
    public List<GameObject> Part2GameObject;
    private List<int> idUsed = new List<int>();
    private int countProgress = 0;
    public Camera cam;

    public int arraySize = 4;
    public int minValue = 0;
    public int maxValue = 2;

    private LineRenderer currentLine;
    public Image cursor;
    bool dragging = false;
    bool rightChoise;
    public GameObject currentCable;
    private MiniGameManager4 gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<MiniGameManager4>();
        AssingColorsPartLeft();
        AssingColorsPartRight();
       cursor.enabled = false;
    }

    void Update()
    {
        ControlLineRenderer();
    }

    int[] GenerateRandomIntArray()
    {
        List<int> availableNumbers = new List<int>();
        for (int i = minValue; i <= maxValue; i++)
        {
            availableNumbers.Add(i);
        }

        int[] randomArray = new int[arraySize];
        for (int i = 0; i < arraySize; i++)
        {
            int randomIndex = Random.Range(0, availableNumbers.Count);
            randomArray[i] = availableNumbers[randomIndex];
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
            Part1GameObject[i].GetComponent<CableInfo>().id = randomArray[i];
            //Part1GameObject[i].GetComponent<LineRenderer>().materials[0] = materials[0];
        }
    }

    void AssingColorsPartRight()
    {
        int[] randomArray = GenerateRandomIntArray();
        for (int i = 0; i < Part2GameObject.Count; i++)
        {
            Part2GameObject[i].GetComponent<MeshRenderer>().material = materials[randomArray[i]];
            Part2GameObject[i].GetComponent<CableInfo>().id = randomArray[i];
            Part2GameObject[i].GetComponent<LineRenderer>().materials[0] = materials[randomArray[i]];
        }
    }

    private void UpdatePositionsRenderer(LineRenderer lineRenderer)
    {
        if (lineRenderer.positionCount == 0)
        {
            lineRenderer.positionCount = 2;
            Transform firstChild = lineRenderer.gameObject.transform.GetChild(0);
            Vector3 firstChildPosition = firstChild.position;
            lineRenderer.SetPosition(0, firstChildPosition);
        }
    }

    void ClearLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 0;
    }


    public void ControlLineRenderer()
    {
        if (cam.enabled)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePositionScreen = Input.mousePosition;
            Vector3 mousePositionWorld = cam.ScreenToWorldPoint(new Vector3(mousePositionScreen.x, mousePositionScreen.y, 10f));

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (Input.GetMouseButtonDown(0) && dragging == false)
                {
                    currentCable = hit.collider.gameObject;

                    LineRenderer lineRenderer = currentCable.GetComponent<LineRenderer>();
                    if (lineRenderer != null && hit.collider.CompareTag("Pipeline"))
                    {
                        currentLine = lineRenderer;
                        UpdatePositionsRenderer(currentLine);
                        currentLine.materials[0] = currentCable.GetComponent<Renderer>().material;
                        mousePositionWorld = hit.point;
                        dragging = true;
                    }
                    else
                    {
                        Debug.Log("null");
                    }
                }

             
            }

            if (dragging && currentLine != null && currentCable.GetComponent<CableInfo>().connected==false)
            {
                currentLine.SetPosition(1, mousePositionWorld);
            }

            cursor.transform.position = mousePositionWorld;




            if (dragging)
            {
                // Realizar un nuevo raycast
                RaycastHit newHit;
                Debug.DrawRay(mousePositionWorld, Vector3.forward * 10f, Color.red);

                if (Physics.Raycast(mousePositionWorld, Vector3.forward, out newHit))
                {
                    // Comprobar si está tocando algo y es un Pipeline diferente al actual
                    if (newHit.collider.tag == "Pipeline" && newHit.collider.gameObject != null && newHit.collider.name != currentCable.gameObject.name)
                    {
                        Debug.Log(newHit.collider.gameObject.name); 
                        CableInfo hitCableInfo = newHit.collider.gameObject.GetComponent<CableInfo>();
                        CableInfo currentCableInfo = currentCable != null ? currentCable.GetComponent<CableInfo>() : null;

                        Debug.Log(hitCableInfo.id);
                        Debug.Log(currentCableInfo.id);

                        if (hitCableInfo != null && currentCableInfo != null && hitCableInfo.id == currentCableInfo.id)
                        {
                            rightChoise=true;
                            
                            LineRenderer currentCableLineRenderer = currentCable.GetComponent<LineRenderer>();
                            
                            if (currentCableLineRenderer != null)
                            {

                                if (Input.GetMouseButtonUp(0))
                                {
                                    Transform firstChild = newHit.collider.gameObject.transform.GetChild(0);
                                    if (firstChild != null)
                                    {
                                        currentCableLineRenderer.positionCount = 2;
                                        currentCableLineRenderer.SetPosition(1, firstChild.position);
                                        currentCable.GetComponent<CableInfo>().connected = true;
                                        hitCableInfo.gameObject.GetComponent<CableInfo>().connected = true;
                                        currentCable = null;
                                        rightChoise = false;
                                        countProgress++;
                                        Invoke("CheckFinished", 1f);
                                        
                                    }
                                }
                            }
                        }
                        else
                        {
                            rightChoise = false;
                        }
                    }
                }
            }

          



            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                currentLine = null;
                rightChoise = false;
                Debug.Log("Suelto");
                if (rightChoise == false && currentCable != null)
                {
                    ClearLineRenderer(currentCable.GetComponent<LineRenderer>());

                }

            }

        }
        else
        {
            Cursor.visible = false;
        }
    }

    private void CheckFinished()
    {
        if(countProgress == 4)
        {
            gameManager.ChangeCameraMode();
            gameManager.PhaseMode(2);

            ResetCables();
        }
    }



    private void ResetCables()
    {
        foreach (GameObject cable in Part1GameObject)
        {
            cable.GetComponent<CableInfo>().connected = false;
            cable.GetComponent<LineRenderer>().positionCount = 0;
        }

        foreach (GameObject cable in Part2GameObject)
        {
            cable.GetComponent<CableInfo>().connected = false;
            cable.GetComponent<LineRenderer>().positionCount = 0;
        }
    }
}

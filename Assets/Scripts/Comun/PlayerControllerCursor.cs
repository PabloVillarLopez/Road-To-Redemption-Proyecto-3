using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using Slider = UnityEngine.UI.Slider;

public class PlayerControllerCursor : MonoBehaviour
{
    public int mode = 0;

    #region Movement Variables
    [Header("Movement Variables")]
    public float movementSpeed;
    private Transform orientation;
    public float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rigy;
    #endregion

    #region MiniGame1 Variables
    public int currentSeed = -1;
    public float plantingTime = 3f;
    public Slider progressBar;
    [SerializeField] GameObject[] caughtSeed = new GameObject[6];
    private GameObject planTarget;
    private int countSeeds = 0;
    private bool isPlanting;
    private float plantingTimer = 3f;
    public KeyCode plantingKey = KeyCode.E;
    private GameObject caughtObject;
    public MiniGameManager1 manager; // Variable específica para MiniGameManager1
    #endregion

    #region MiniGame8 Variables
    public Camera newMainCamera;
    public Camera mainCamera;
    public bool monitoring;
    #endregion

    #region General Variables
    private MonoBehaviour activeGameManager; // Variable para el GameManager dinámico
    #endregion

    #region Start
    void Start()
    {
        rigy = GetComponent<Rigidbody>();

        mainCamera = Camera.main;

        // Ajustar la orientación si hay múltiples hijos
        if (transform.childCount > 2)
        {
            orientation = transform.GetChild(1).transform;
            orientation = transform.GetChild(2).transform;
        }
        else
        {
            orientation = transform.GetChild(0).transform;
        }
    }
    #endregion

    #region Update
    void Update()
    {
        SelectMode();

        if (monitoring && Input.GetKeyDown(KeyCode.E) && mode == 2)
        {
            ChangeMainCamera();
        }

        if (isPlanting && manager.PlantSound(false) == false)
        {
            manager.PlantSound(true);
        }
        else
        {
            if (manager != null)
            {
                manager.PlantSound(false);
            }
        }

        TalkWithNpc(); // Llamar a la función TalkWithNpc en cada actualización
    }
    #endregion

    #region Fixed Update
    private void FixedUpdate()
    {
        if (!monitoring)
        {
            MovePlayer();
        }
    }
    #endregion

    #region Movement
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rigy.AddForce(moveDirection.normalized * movementSpeed * 10, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rigy.velocity.x, 0f, rigy.velocity.z);
        if (flatVelocity.magnitude > movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            rigy.velocity = new Vector3(limitedVelocity.x, rigy.velocity.y, limitedVelocity.z);
        }
    }
    #endregion

    #region Cast Ray From Mouse Position
    void CastRayFromMousePosition()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
      {
            if (hit.collider.CompareTag("CatchAble"))
        {
                manager.activeInteract(true);

                if (Input.GetKeyDown(KeyCode.E))
            {
                caughtObject = hit.collider.gameObject;
                if (PickupObject(caughtObject))
                {
                    caughtSeed[countSeeds] = caughtObject;
                    countSeeds++;
                    caughtObject.transform.parent = transform;
                    caughtObject.SetActive(false);
                    manager.activeInteract(false);
                    manager.PlaySound(4);
                    int id = caughtObject.GetComponent<ObjectInfo>().GetobjectInfo();
                }
                else
                {
                    if (manager != null) manager.reminderNotCatch();
                }
            }
        }
        else if (hit.collider.CompareTag("SeedPlanted"))
        {
            manager.activeInteract(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                var objectInfo = hit.collider.gameObject.GetComponent<ObjectInfo>();
                if (objectInfo != null && objectInfo.timeToCollect >= 10)
                {
                    objectInfo.Recollect();
                    manager.Reminders();
                    manager.PlaySound(2);
                }
                else
                {
                    manager.reminderNotRecollect();
                }
            }
        }
        else if (hit.collider.CompareTag("PlantArea") && countSeeds > 0)
        {
            planTarget = hit.collider.gameObject;
            isPlanting = true;
        }
        else if (hit.collider.CompareTag("Respawn"))
        {
            manager.activeInteract(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                manager.PlaySound(5);
                manager.SpawnObjectsOnSpawner();
            }
        }
        else
        {
            manager.activeInteract(false);
            isPlanting = false;
        }

        HandleZoneTags(hit);
    

    


        if (!monitoring)
        {
            
            
                if (hit.collider.CompareTag("CatchAble") && mode == 2)
                {
                    GameObject managerObject = GameObject.Find("GameManager");
                    managerObject.GetComponent<MiniGameManager8>().activeInteract(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ChangeCamera();
                        managerObject.GetComponent<MiniGameManager8>().StartGame();
                    }
                }
                else if (mode == 2 && !hit.collider.CompareTag("CatchAble"))
                {
                    GameObject managerObject = GameObject.Find("GameManager");
                    managerObject.GetComponent<MiniGameManager8>().activeInteract(false);
                }
            }
      }
    }


    private void HandleZoneTags(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Hot"))
        {
            manager.activeInteract(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                manager.PlaySound(5);
                hit.collider.transform.parent.gameObject.GetComponent<CultiveZone>().state = "Hot";
            }
        }
        else if (hit.collider.CompareTag("Neutral"))
        {
            manager.activeInteract(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                manager.PlaySound(5);
                hit.collider.transform.parent.gameObject.GetComponent<CultiveZone>().state = "Neutral";
            }
        }
        else if (hit.collider.CompareTag("Cold"))
        {
            manager.activeInteract(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                manager.PlaySound(5);
                hit.collider.transform.parent.gameObject.GetComponent<CultiveZone>().state = "Cold";
            }
        }
    }
    #endregion

    #region SetPlants
    public bool PickupObject(GameObject objectToPickup)
    {
        ObjectInfo objectInfo = objectToPickup.GetComponent<ObjectInfo>();

        if (objectInfo != null)
        {
            int newObjectType = objectInfo.GetobjectInfo();

            if (currentSeed == -1)
            {
                currentSeed = newObjectType;
                return true;
            }
            else if (currentSeed == newObjectType)
            {
                return true;
            }
            else
            {
                Debug.Log("You already have an object of another type in your inventory. You cannot pick up this object.");
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void StartPlanting()
    {
        isPlanting = true;
        plantingTimer = 0f;
        progressBar.gameObject.SetActive(true);
    }

    void UpdateProgress(float progress)
    {
        progressBar.value = Mathf.Clamp01(progress);
    }

    void UpdatePlantingProcess()
    {
        if (isPlanting)
        {
            plantingTimer += Time.deltaTime;
            UpdateProgress(plantingTimer / plantingTime);

            if (plantingTimer >= plantingTime)
            {
                FinishPlanting();
            }
        }
    }

    void FinishPlanting()
    {
        isPlanting = false;
        progressBar.value = 0f;
        progressBar.gameObject.SetActive(false);
        PlantSeed();
    }

    void PlantSeed()
    {
        if (countSeeds - 1 >= 0 && countSeeds - 1 < caughtSeed.Length && caughtSeed[countSeeds - 1] != null)
        {
            GameObject seedSpawned;
            switch (currentSeed)
            {
                case 1:
                    seedSpawned = Instantiate(manager.seeds[0], planTarget.transform.position, Quaternion.identity);
                    seedSpawned.GetComponent<ObjectInfo>().id = 0;
                    planTarget.transform.GetComponentInParent<CultiveZone>().AddChild(seedSpawned.gameObject);
                    Debug.Log("Planted0");
                    break;
                case 2:
                    seedSpawned = Instantiate(manager.seeds[1], planTarget.transform.position, Quaternion.identity);
                    seedSpawned.GetComponent<ObjectInfo>().id = 1;
                    planTarget.transform.GetComponentInParent<CultiveZone>().AddChild(seedSpawned.gameObject);
                    Debug.Log("Planted1");
                    break;
                case 3:
                    seedSpawned = Instantiate(manager.seeds[2], planTarget.transform.position, Quaternion.identity);
                    seedSpawned.GetComponent<ObjectInfo>().id = 2;
                    planTarget.transform.GetComponentInParent<CultiveZone>().AddChild(seedSpawned.gameObject);
                    Debug.Log("Planted2");
                    break;
            }

            planTarget.SetActive(false);
            caughtSeed[countSeeds - 1] = null;
            countSeeds--;

            if (countSeeds == 0)
            {
                currentSeed = -1;
            }
        }
    }
    #endregion

    #region Camera Handling
    public void ChangeMainCamera()
    {
        mainCamera.enabled = false;
        newMainCamera.enabled = true;
        newMainCamera.tag = "MainCamera";
    }

    public void RevertToPreviousCamera()
    {
        newMainCamera.enabled = false;
        mainCamera.enabled = true;
        mainCamera.tag = "MainCamera";
        monitoring = false;
    }

    private void ChangeCamera()
    {
        if (monitoring)
        {
            RevertToPreviousCamera();
            monitoring = false;
        }
        else
        {
            ChangeMainCamera();
            monitoring = true;
        }
    }
    #endregion

    #region Mode Selection
    void SelectMode()
    {
        switch (mode)
        {
            case 1:
                MyInput();
                SpeedControl();
                CastRayFromMousePosition();
                if (isPlanting)
                {
                    progressBar.gameObject.SetActive(true); // Activate progress bar
                }
                else
                {
                    plantingTimer = 0f; // Reset planting timer
                    progressBar.gameObject.SetActive(false);
                }

                UpdatePlantingProcess();
                break;
            case 2:
                MyInput();
                if (monitoring)
                {
                    SpeedControl();
                }
                CastRayFromMousePosition();
                break;
            case 3:
                MyInput();
                SpeedControl();
                break;
            case 4:
                MyInput();
                SpeedControl();
                break;
        }
    }
    #endregion

    #region NPC Interaction
    private void TalkWithNpc()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10))
        {
            if (hit.collider.CompareTag("NPC"))
            {
                if (activeGameManager == null)
                {
                    activeGameManager = FindActiveGameManager();
                }

                if (activeGameManager != null)
                {
                    CallActiveInteract(activeGameManager, true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        npc npcScript = hit.collider.gameObject.GetComponent<npc>();
                        if (npcScript != null)
                        {
                            npcScript.showTutorial();
                        }

                        CallActiveInteract(activeGameManager, false);
                    }
                }
            }
        }
        else
        {
            if (activeGameManager != null)
            {
                CallActiveInteract(activeGameManager, false);
            }
        }
    }

    private MonoBehaviour FindActiveGameManager()
    {
        // Nombres de los scripts de los GameManagers específicos dentro de los GameObjects "GameManager".
        string[] managerNames = { "MiniGameManager1", "MiniGameManager4", "MiniGameManager7", "MiniGameManager8" };

        // Buscar en cada GameObject llamado "GameManager".
        foreach (string managerName in managerNames)
        {
            // Buscar el GameObject con el nombre "GameManager".
            GameObject[] gameManagerObjects = GameObject.FindGameObjectsWithTag("GameController");

            foreach (GameObject gameObject in gameManagerObjects)
            {
                // Intentar encontrar el componente MonoBehaviour con el nombre específico en el GameObject actual.
                MonoBehaviour managerInstance = gameObject.GetComponent(managerName) as MonoBehaviour;

                // Si se encuentra, devolverlo.
                if (managerInstance != null)
                {
                    return managerInstance;
                }
            }
        }

        // Si no se encuentra ningún GameManager válido, devolver null.
        return null;
    }

    private void CallActiveInteract(MonoBehaviour manager, bool active)
    {
        var method = manager.GetType().GetMethod("activeInteract");
        if (method != null)
        {
            method.Invoke(manager, new object[] { active });
        }
    }
    #endregion
}

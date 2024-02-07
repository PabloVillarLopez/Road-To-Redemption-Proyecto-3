
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.VersionControl.Asset;


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
    private int currentSeed = -1;
    public float plantingTime = 3f;
    public Slider progressBar;
    [SerializeField] GameObject[] caughtSeed = new GameObject[6];
    private GameObject planTarget;
    private int countSeeds = 0;
    private bool isPlanting;
    private float plantingTimer = 3f;
    public KeyCode plantingKey = KeyCode.E;
    private GameObject caughtObject;
    public MiniGameManager1 manager;
    #endregion


    #region MiniGame8 Variables
    public Camera newMainCamera;
    public Camera mainCamera;
    public bool monitoring;
    
    #endregion
    #region Start
    // Start is called before the first frame update
    void Start()
    {
        rigy = GetComponent<Rigidbody>();
        orientation = transform.GetChild(2).transform;
        mode = 2;
        mainCamera = Camera.main;
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        SelectMode();

        if (monitoring  &&  Input.GetKeyDown(KeyCode.E))
        {
            ChangeMainCamera();
            
        }
    }
    #endregion

    #region Fixed Update
    private void FixedUpdate()
    {
        if (monitoring == false)
        {
            MovePlayer();
        }
        
    }
    #endregion

    #region Movement
    #region Get Movement Input
    private void MyInput() // Gets movement input in horizontal and vertical axis with AWSD and arrows
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    #endregion

    #region Move Player with Direction
    private void MovePlayer() // Moves player taking into account the direction the player is facing
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rigy.AddForce(moveDirection.normalized * movementSpeed * 10, ForceMode.Force);
    }
    #endregion

    #region Max Speed Control
    private void SpeedControl() // Limits the velocity to the max speed velocity and controls that max velocity
    {
        Vector3 flatVelocity = new Vector3(rigy.velocity.x, 0f, rigy.velocity.z);
        if (flatVelocity.magnitude > movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            rigy.velocity = new Vector3(limitedVelocity.x, rigy.velocity.y, limitedVelocity.z);
        }
    }
    #endregion
    #endregion

    #region Cast Ray From Mouse Position
    void CastRayFromMousePosition()
    {

        if (monitoring == false)
        {

        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
       
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("CatchAble"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                     caughtObject = hit.collider.gameObject;

                    if (PickupObject(caughtObject))
                    {
                        caughtSeed[countSeeds] = hit.collider.gameObject;
                        countSeeds++;
                        caughtObject.transform.parent = transform;
                        caughtObject.SetActive(false);

                        ObjectInfo info = caughtObject.GetComponent<ObjectInfo>();
                        int id = info.GetobjectInfo();
                    }
                }
            }

            if (hit.collider.CompareTag("PlantArea") && countSeeds > 0)
            {
                planTarget = hit.collider.gameObject;
                
                isPlanting = true;
            }
            else
            {
                isPlanting = false;
            }






            

            if (hit.collider.CompareTag("Hot") && Input.GetKeyDown(KeyCode.E))
            {

                CultiveZone cultiveZone = hit.collider.transform.parent.gameObject.GetComponent<CultiveZone>();
                if (cultiveZone != null)
                {
                    cultiveZone.GetComponent<CultiveZone>().state="Hot" ;
                }
            }

            if (hit.collider.CompareTag("Neutral") && Input.GetKeyDown(KeyCode.E))
            {
                GameObject hitObject = hit.collider.transform.parent.gameObject;
                hitObject.GetComponent<CultiveZone>().state = "Neutral";
            }
            if (hit.collider.CompareTag("Cold") && Input.GetKeyDown(KeyCode.E))
            {
                GameObject hitObject = hit.collider.transform.parent.gameObject;
                hitObject.GetComponent<CultiveZone>().state = "Cold";
            }

            if(hit.collider.CompareTag("CatchAble")&& mode == 2 && Input.GetKeyDown(KeyCode.E))
            {
                ChangeCamera();
                    GameObject manager = GameObject.Find("GameManager");
                    manager.GetComponent<MiniGameManager8>().StartGame();
                }
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
            caughtSeed[countSeeds - 1].SetActive(true);
            
            caughtSeed[countSeeds - 1].transform.position = planTarget.transform.position;
            caughtSeed[countSeeds - 1].transform.parent = planTarget.transform.parent;

            planTarget.transform.GetComponentInParent<CultiveZone>().AddChild(caughtSeed[countSeeds - 1].gameObject);

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

    public void ChangeMainCamera()
    {
        // Desactiva la cámara actualmente marcada como principal
        
        mainCamera.enabled = false;

        // Activa la nueva cámara
        newMainCamera.enabled = true;

        // Configura la nueva cámara como la cámara principal
        newMainCamera.tag = "MainCamera";
    }

    public void RevertToPreviousCamera()
    {
        // Desactiva la nueva cámara
        newMainCamera.enabled = false;

        // Activa la cámara anteriormente marcada como principal
        mainCamera.enabled = true;

        // Configura la cámara anterior como la cámara principal
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
        else if (monitoring == false)
        {
            ChangeMainCamera();
            monitoring = true;
            
        }


    }

    
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
                if (monitoring) { 
                SpeedControl();

                
                }
                CastRayFromMousePosition();
                break;
            case 3:
                // Código para el tercer caso
                break;
            case 4:
                // Código para el cuarto caso
                break;


        }
    }
}

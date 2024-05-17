
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
        
       
            mainCamera = Camera.main;
        
        orientation = transform.GetChild(1).transform;
        orientation = transform.GetChild(2).transform;
        

    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        SelectMode();

        if (monitoring  &&  Input.GetKeyDown(KeyCode.E) && mode==2)
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
        if (!monitoring)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("CatchAble") && mode == 2 && Input.GetKeyDown(KeyCode.E))
                {
                    ChangeCamera();
                    GameObject manager = GameObject.Find("GameManager");
                    manager.GetComponent<MiniGameManager8>().StartGame();
                }
            }

            //    if (Physics.Raycast(ray, out hit))
            //    {
            //        // Manejar colisiones con CatchAble
            //        if (hit.collider.CompareTag("CatchAble"))
            //        {
            //            //manager.activeInteract(true);
            //            if (Input.GetKeyDown(KeyCode.E))
            //            {
            //                caughtObject = hit.collider.gameObject;

            //                if (PickupObject(caughtObject))
            //                {
            //                    caughtSeed[countSeeds] = caughtObject;
            //                    countSeeds++;
            //                    caughtObject.transform.parent = transform;
            //                    caughtObject.SetActive(false);

            //                    ObjectInfo info = caughtObject.GetComponent<ObjectInfo>();
            //                    int id = info.GetobjectInfo();
            //                }
            //            }
            //        }
            //        else if (hit.collider.CompareTag("SeedPlanted")) 
            //        {
            //            manager.activeInteract(true);
            //        }
            //        else
            //        {
            //            manager.activeInteract(false);
            //        }


            //        // Manejar colisiones con PlantArea
            //        if (hit.collider.CompareTag("PlantArea") && countSeeds > 0)
            //        {
            //            planTarget = hit.collider.gameObject;
            //            isPlanting = true;
            //        }
            //        else
            //        {
            //            isPlanting = false;
            //        }

            //        if (hit.collider.CompareTag("SeedPlanted"))
            //        {
            //            manager.activeInteract(true);
            //            if (Input.GetKeyDown(KeyCode.E))
            //            {
            //                if (hit.collider.gameObject.GetComponent<ObjectInfo>().timeToCollect >= 10 && hit.collider.gameObject.GetComponent<ObjectInfo>() != null)
            //                {
            //                    hit.collider.gameObject.GetComponent<ObjectInfo>().Recollect();
            //                    manager.Reminders();
            //                }
            //                else
            //                {
            //                    Debug.Log("No se puede recolectar");
            //                }
            //            }


            //        }



            //        // Manejar colisiones con Hot, Neutral, y Cold
            //        if (hit.collider.CompareTag("Hot") && Input.GetKeyDown(KeyCode.E))
            //        {
            //            CultiveZone cultiveZone = hit.collider.transform.parent.gameObject.GetComponent<CultiveZone>();
            //            if (cultiveZone != null)
            //            {
            //                cultiveZone.state = "Hot";
            //            }
            //        }

            //        if (hit.collider.CompareTag("Neutral") && Input.GetKeyDown(KeyCode.E))
            //        {
            //            GameObject hitObject = hit.collider.transform.parent.gameObject;
            //            hitObject.GetComponent<CultiveZone>().state = "Neutral";
            //        }

            //        if (hit.collider.CompareTag("Cold") && Input.GetKeyDown(KeyCode.E))
            //        {
            //            GameObject hitObject = hit.collider.transform.parent.gameObject;
            //            hitObject.GetComponent<CultiveZone>().state = "Cold";
            //        }

            //        // Manejar colisiones con CatchAble y modo 2

            //    }
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

                Vector3 addposition1 = new Vector3(0.019f, 0.038f,-0.120f);
                seedSpawned = Instantiate(manager.seeds[0], planTarget.transform.position + addposition1, Quaternion.Euler(340.059998f, 180, 180));
                    seedSpawned.GetComponent<ObjectInfo>().id = 0;
                    planTarget.transform.GetComponentInParent<CultiveZone>().AddChild(seedSpawned.gameObject);
                    Debug.Log("Planted0");
                    break;
                case 2:
                    Vector3 addposition2 = new Vector3(0.030f, 0.218f,-0.120f);

                    seedSpawned = Instantiate(manager.seeds[1], planTarget.transform.position + addposition2, Quaternion.Euler(281.509979f, 0.240004182f, 359.73999f)); 
                    seedSpawned.GetComponent<ObjectInfo>().id = 1;
                    planTarget.transform.GetComponentInParent<CultiveZone>().AddChild(seedSpawned.gameObject);
                    Debug.Log("Planted1");
                    break;
                case 3:
                    Vector3 addposition3 = new Vector3(-0.154f, 0.925f, 0.679f);
                    seedSpawned = Instantiate(manager.seeds[2], planTarget.transform.position +addposition3 , Quaternion.Euler(286.439972f, 11.3599997f, 2.01000643f)); 
                    seedSpawned.GetComponent<ObjectInfo>().id = 2;
                    seedSpawned.transform.rotation = Quaternion.Euler(286.439972f, 11.3599997f, 2.01000643f);
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
                MyInput();
                SpeedControl();
                break;
            case 4:
                MyInput();
                SpeedControl();

                break;


        }
    }
}

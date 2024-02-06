
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerCursor : MonoBehaviour
{
    #region Movement Variables
    [Header("Movement Variables")]
    public float movementSpeed;
    private Transform orientation;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rigy;
    #endregion

    #region MiniGame1 Variables
    private int currentSeed = -1;
    public float plantingTime = 3f;
    public Slider progressBar;
    GameObject[] caughtSeed = new GameObject[6];
    private GameObject planTarget;
    private int countSeeds = 0;
    private bool isPlanting;
    private float plantingTimer = 3f;
    public KeyCode plantingKey = KeyCode.E;
    private GameObject caughtObject;
    public MiniGameManager1 manager;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        rigy = GetComponent<Rigidbody>();
        orientation = transform.GetChild(2).transform;
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();
        Vector3 mouseWorldPosition = CastRayFromMousePosition();

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
    }
    #endregion

    #region Fixed Update
    private void FixedUpdate()
    {
        MovePlayer();
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
    Vector3 CastRayFromMousePosition()
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

            return hit.point;
        }

        return Vector3.zero;
    }
    #endregion

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
            Debug.Log("The object does not have type information. It cannot be picked up.");
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
            caughtSeed[countSeeds - 1].GetComponent<ObjectInfo>().AssignToCultive(caughtObject.gameObject);
            caughtSeed[countSeeds - 1].transform.position = planTarget.transform.position;
            caughtSeed[countSeeds - 1].transform.parent = null;
            caughtSeed[countSeeds - 1] = null;
            countSeeds--;

            if (countSeeds == 0)
            {
                currentSeed = -1;
            }
        }
    }
}

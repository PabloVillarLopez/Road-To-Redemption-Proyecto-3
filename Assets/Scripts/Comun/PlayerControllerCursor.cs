using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool catchObject;
    private int TypeObject;
    public MiniGameManager1 Manager;
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
    private void MyInput() //Gets movement in horizontal and vertical axis with AWSD and arrows
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    #endregion

    #region Move Player with Direction
    private void MovePlayer() //Moves player taking into account the direction the player is facing
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rigy.AddForce(moveDirection.normalized * movementSpeed * 10, ForceMode.Force);
    }
    #endregion

    #region Max Speed Control
    private void SpeedControl() //Limits the velocity to the max speed velocity and controls that max velocity
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
    Vector3 CastRayFromMousePosition()
    {
        // Perform a raycast from the camera position to the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Return the position of the ray hit
            if (hit.collider.CompareTag("CatchAble"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    catchObject = true;
                    hit.collider.gameObject.SetActive(false);
                    ObjectInfo info = hit.collider.GetComponent<ObjectInfo>();
                    int id = info.GetID();
                    Debug.Log(id);

                    switch (id)
                    {
                        case 0:
                            Manager.AddTomatoToInventory();
                            break;
                        case 1:
                            Manager.AddLettuceToInventory();
                            break;
                        case 2:
                            Manager.AddCarrotToInventory();
                            break;
                        default:
                            Debug.LogWarning("Unrecognized object ID: " + id);
                            break;
                    }
                }
            }
        }
        // If the ray doesn't hit any object, return Vector3.zero
        return Vector3.zero;
    }
    #endregion
    #endregion
}

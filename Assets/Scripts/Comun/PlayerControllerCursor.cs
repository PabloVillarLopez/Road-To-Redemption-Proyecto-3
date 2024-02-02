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


    #endregion Movement Variables


    #region MiniGame1 Variables

    private bool catchObject;
    private int TypeObject;



    #endregion MiniGame1 Variables
    #region Start
    // Start is called before the first frame update
    void Start()
    {
        rigy = GetComponent<Rigidbody>();
        orientation = transform.GetChild(2).transform;
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();


        Vector3 mouseWorldPosition = CastRayFromMousePosition();

        CheckCatchObject();



    }

    #endregion Update

    #region Fixed Update
    private void FixedUpdate()
    {
        MovePlayer();

       
    }

    #endregion Fixed Update

    #region Movement
    #region Get Movement Input

    private void MyInput() //Gets movement in horizontal and vertical axis with AWSD and arrows
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    #endregion Get Movement Input

    #region Move Player with Direction

    private void MovePlayer() //Moves player taking into account the direction the player is facing
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rigy.AddForce(moveDirection.normalized * movementSpeed * 10, ForceMode.Force);
    }

    #endregion Move Player with Direction

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

    #endregion Max Speed Control


    Vector3 CastRayFromMousePosition()
    {
        // Realiza un raycast desde la posición de la cámara hacia el ratón
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Devuelve la posición del hit del rayo
            if (hit.collider.CompareTag("CatchAble"))
            {
                Debug.Log("El raycast ha golpeado un objeto con el tag 'CatchAble'.");
                catchObject = true;

            }
            




          
        }
        
        // Si el rayo no golpea ningún objeto, devuelve Vector3.zero
        return Vector3.zero;
    }


    void CheckCatchObject()
    {
        if (catchObject)
        {
            if (Input.GetMouseButtonDown(0))
            {

                Debug.Log("Cogido 'CatchAble'.");


                catchObject = false;

            }
        }
       


    }



    #endregion Movement
}

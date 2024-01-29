using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    // Public variables
    public float offSet = 1f;

    // Private variables
    private Vector3 pos;

    // Start method, called at the beginning
    void Start()
    {

    }

    // Update method, called once per frame
    void Update()
    {
        // Get mouse position
        pos = Input.mousePosition;

        // Set z-coordinate to offset
        pos.z = offSet;

        // Convert screen position to world position
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashInfo : MonoBehaviour
{
    public int id;
    public Material[] materials = new Material[3];
    public float fuerzaHaciaAbajo = 0.2f;
    // Start is called before the first frame update
 
        // Update is called once per frame
        void Update()
        {
        AddForce();
        }

        public void SetId(int newId)
        {
            id = newId;
        Renderer renderer =gameObject.GetComponentInChildren<Renderer>();
        switch (id)
        {

            
            case 1:
                Debug.Log("1");
                renderer.material = materials[0];
                break;
            case 2:

                renderer.material = materials[1];
                break;
            case 3:

                renderer.material = materials[2];
                break;

        }
    }

    public void AddForce()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        rb.useGravity = true;
        
        rb.AddForce(Vector3.down * fuerzaHaciaAbajo, ForceMode.Acceleration);
        
    }

    public int GetId()
    {
        return id;

    }


    } 

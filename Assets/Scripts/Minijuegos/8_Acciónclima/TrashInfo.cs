using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashInfo : MonoBehaviour
{
    public int id;
    public Material[] materials = new Material[3];
    public float fuerzaHaciaAbajo = 0.2f;
    public float lifespan = 10f; // Lifespan in seconds

    // Start is called before the first frame update
    void Start()
    {
        // Schedule the object to be destroyed after its lifespan
        Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        AddForce();
    }

    public void SetId(int newId)
    {
        id = newId;
        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pipeline"))
        {
            // Access the Rigidbody component
            Rigidbody rb = GetComponent<Rigidbody>();

            // Reduce the object's velocity and angular velocity
            rb.velocity *= 0.1f;       // Reduce linear velocity to 10% of its current value
            rb.angularVelocity *= 0.1f; // Reduce angular velocity to 10% of its current value
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        // Aqu� puedes poner el c�digo que deseas que se ejecute cuando se hace clic en el GameObject
        Debug.Log("Se hizo clic en el GameObject: " + gameObject.name);
    }
}

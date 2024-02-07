using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTaget : MonoBehaviour
{
     public GameObject cultive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void addFruit(GameObject newGameObject)
    {
        cultive.GetComponent<CultiveZone>().AddFruit();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("almoñejar");
        // Verifica si la colisión es con el objeto deseado
        if (collision.gameObject.CompareTag("CatchAble"))
        {
            
            Debug.Log("almoñejar");
        }
    } 
}

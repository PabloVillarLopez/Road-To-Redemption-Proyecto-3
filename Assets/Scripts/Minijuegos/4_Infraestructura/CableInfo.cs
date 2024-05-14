using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableInfo : MonoBehaviour
{
    public int id;
    public bool connected;
    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
       
        Invoke("BuscarDespuesDe2Segundos", 1f);
    }

    void BuscarDespuesDe2Segundos()
    {
        // Verificar si el objeto tiene hijos
        if (transform.childCount > 0)
        {
            // Obtener el primer hijo
            Transform primerHijo = transform.GetChild(0);

            // Obtener la posición del primer hijo
            Vector3 startPosition = primerHijo.position;

            // Configurar el LineRenderer para mostrar una línea desde la posición del objeto actual hasta la posición del primer hijo
            line = GetComponent<LineRenderer>();
            line.SetPosition(0, transform.position);
            line.SetPosition(1, startPosition);


            switch (id)
            {
                case 0:
                    line.material.color = Color.blue;
                    break;
                case 1:
                    line.material.color = Color.red;
                    break;
                case 2:
                    line.material.color = Color.yellow;
                    break;
                case 3:
                    line.material.color = Color.green;
                    break;
                case 4:
                    line.material.color = Color.magenta;
                    break;
               
                default:
                    line.material.color = Color.white;
                    break;
            }
        }
        else
        {
            // Si no hay hijos, imprimir un mensaje de error en la consola
            Debug.LogError("No se encontró ningún hijo.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

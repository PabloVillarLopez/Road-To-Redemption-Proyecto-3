using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorFirtsDialogue : MonoBehaviour
{
    public MonoBehaviour gameManager; // Permitir seleccionar cualquier MonoBehaviour que tenga el m�todo requerido.
    private bool firstTime = true;

    void OnTriggerEnter(Collider other)
    {
        // Comprobar si gameManager no es null y tiene el m�todo requerido
        if (firstTime && gameManager != null && gameManager is MiniGameManager8)
        {
            firstTime = false;
            (gameManager as MiniGameManager8).GetComponent<DialogueScript>().StartSpanishDialogue(); ;
        }
        else if (firstTime && gameManager != null && gameManager is MiniGameManager1)
        {
            firstTime = false;
            (gameManager as MiniGameManager1).GetComponent<DialogueScript>().StartSpanishDialogue(); ;
        }
        // Puedes a�adir m�s condiciones si hay m�s tipos de MiniGameManagers
    }
}

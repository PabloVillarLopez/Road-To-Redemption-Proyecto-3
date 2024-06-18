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
        if (firstTime && gameManager != null)
        {
            firstTime = false;
            var dialogueScript = gameManager.GetComponent<DialogueScript>();

            // Verificar el idioma actual
            bool isSpanish = LanguageManager.currentLanguage == LanguageManager.Language.Spanish;

            // Seleccionar el tipo de gameManager y ejecutar el di�logo correspondiente
            if (gameManager is MiniGameManager8)
            {
                if (isSpanish)
                {
                    dialogueScript.StartSpanishDialogue();
                }
                else
                {
                    dialogueScript.StartEnglishDialogue();
                }
            }
            else if (gameManager is MiniGameManager1)
            {
                if (isSpanish)
                {
                    dialogueScript.StartSpanishDialogue();
                }
                else
                {
                    dialogueScript.StartEnglishDialogue();
                }
            }
            else if (gameManager is MiniGameManager4)
            {
                if (isSpanish)
                {
                    dialogueScript.StartSpanishDialogue();
                }
                else
                {
                    dialogueScript.StartEnglishDialogue();
                }
            }
            else if (gameManager is MiniGameManager7)
            {
                if (isSpanish)
                {
                    dialogueScript.StartSpanishDialogue();
                }
                else
                {
                    dialogueScript.StartEnglishDialogue();
                }
            }
        }
        // Puedes a�adir m�s condiciones si hay m�s tipos de MiniGameManagers
    }
}

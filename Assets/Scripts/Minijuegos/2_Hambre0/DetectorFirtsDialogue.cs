using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorFirtsDialogue : MonoBehaviour
{
    public MonoBehaviour gameManager; // Permitir seleccionar cualquier MonoBehaviour que tenga el método requerido.
    private bool firstTime = true;

    void OnTriggerEnter(Collider other)
    {
        // Comprobar si gameManager no es null y tiene el método requerido
        if (firstTime && gameManager != null)
        {
            firstTime = false;
            var dialogueScript = gameManager.GetComponent<DialogueScript>();

            // Verificar el idioma actual
            bool isSpanish = LanguageManager.currentLanguage == LanguageManager.Language.Spanish;

            // Seleccionar el tipo de gameManager y ejecutar el diálogo correspondiente
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
        // Puedes añadir más condiciones si hay más tipos de MiniGameManagers
    }
}

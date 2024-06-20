using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// DialogueByImage Script

// Funciones disponibles para llamar desde otros scripts:

// void ShowFirstImage()
//   - Muestra la primera imagen del array en el componente Image especificado.
//
// void ShowNextImage()
//   - Muestra la siguiente imagen en la secuencia actual.
//   - Si se est� usando una secuencia personalizada, muestra la siguiente imagen en esa secuencia.
//
// void ShowImageByIndex(int index)
//   - Muestra una imagen espec�fica del array seg�n su �ndice.
//
// void ShowCustomSequence(List<int> indices)
//   - Muestra una secuencia personalizada de im�genes seg�n los �ndices especificados.
//
// void HideImage()
//   - Oculta la imagen actualmente visible.

public class DialogueByImage : MonoBehaviour
{
    public Image displayImage;
    public Image nInteract;
    public Sprite[] imageArray;        // Array de im�genes.
    private int currentIndex = -1;     // �ndice actual de la imagen en el array.
    private bool isShowingByIndex = false; // Flag para rastrear si se est� mostrando una imagen por �ndice.
    private List<int> customSequence;  // Lista de �ndices para la secuencia personalizada.
    private int customSequenceIndex = 0; // �ndice actual en la secuencia personalizada.
    public Sprite nInteractSpriteEnglish;
    public Sprite[] ImageDialogueSpanish; // Array de im�genes en espa�ol.
    public Sprite[] ImageDialogueEnglish; // Array de im�genes en ingl�s.

    public GameObject pausaMenu;
    public GameObject pauseMenu;
    private void Start()
    {
        // Inicializa el array de im�genes seg�n el idioma actual.
        if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            imageArray = ImageDialogueSpanish;
        }
        else if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            imageArray = ImageDialogueEnglish;
            nInteract.sprite= nInteractSpriteEnglish;
        }

        // Desactiva la imagen al inicio.
        displayImage.enabled = false;
        nInteract.enabled = false;




    }

    void Update()
    {
        // Si se presiona la tecla "n".
        if (Input.GetKeyDown(KeyCode.N) && displayImage.IsActive())
        {
            if (isShowingByIndex)
            {
                // Si estamos mostrando una imagen espec�fica por �ndice, ocultar la imagen.
                HideImage();
                isShowingByIndex = false;
            }
            else
            {
                // De lo contrario, mostrar la siguiente imagen en la secuencia o en la secuencia personalizada.
                ShowNextImage();
            }
        }
    }

    // Muestra la primera imagen del array.
    public void ShowFirstImage()
    {
        if (imageArray.Length > 0)
        {
            currentIndex = 0;
            displayImage.sprite = imageArray[currentIndex];
            displayImage.enabled = true; // Activa la imagen.
            nInteract.enabled = true;
            isShowingByIndex = false; // No se est� mostrando por �ndice espec�fico.
            customSequence = null; // Reinicia la secuencia personalizada.
        }
    }

    // Muestra la siguiente imagen en la secuencia o en la secuencia personalizada.
    public void ShowNextImage()
    {
        if (customSequence != null && customSequence.Count > 0)
        {
            // Si hay una secuencia personalizada, avanza en la secuencia personalizada.
            customSequenceIndex++;
            if (customSequenceIndex < customSequence.Count)
            {
                currentIndex = customSequence[customSequenceIndex];
                if (currentIndex >= 0 && currentIndex < imageArray.Length)
                {
                    displayImage.sprite = imageArray[currentIndex];
                    displayImage.enabled = true; // Activa la imagen.
                }
                else
                {
                    HideImage();
                }
            }
            else
            {
                HideImage();
            }
        }
        else
        {
            // Si no hay secuencia personalizada, avanza en la secuencia completa.
            currentIndex++;
            if (currentIndex < imageArray.Length)
            {
                displayImage.sprite = imageArray[currentIndex];
                displayImage.enabled = true; // Activa la imagen.
                nInteract.enabled = true;
            }
            else
            {
                HideImage();
            }
        }
    }

    // Muestra una imagen espec�fica del array seg�n su �ndice.
    public void ShowImageByIndex(int index)
    {
        if (index >= 0 && index < imageArray.Length)
        {
            currentIndex = index;
            displayImage.sprite = imageArray[currentIndex];
            displayImage.enabled = true; // Activa la imagen.
            nInteract.enabled = true;
            isShowingByIndex = true; // Indica que estamos mostrando una imagen por �ndice espec�fico.
            customSequence = null; // Reinicia la secuencia personalizada.
        }
    }

    // Muestra una secuencia personalizada de im�genes seg�n los �ndices especificados.
    public void ShowCustomSequence(List<int> indices)
    {
        // Filtra los �ndices inv�lidos.
        customSequence = indices.FindAll(index => index >= 0 && index < imageArray.Length);
        customSequenceIndex = 0;

        if (customSequence.Count > 0)
        {
            currentIndex = customSequence[customSequenceIndex];
            displayImage.sprite = imageArray[currentIndex];
            displayImage.enabled = true; // Activa la imagen.
            nInteract.enabled   = true;
            isShowingByIndex = false; // No se est� mostrando por �ndice espec�fico individual.
        }
        else
        {
            HideImage();
        }
    }

    // Oculta la imagen actualmente visible.
    public void HideImage()
    {
        displayImage.enabled = false;
        nInteract.enabled = false;
        currentIndex = -1; // Reinicia el �ndice.
        isShowingByIndex = false; // Reinicia el estado de mostrar por �ndice.
        customSequence = null; // Reinicia la secuencia personalizada.
    }
}

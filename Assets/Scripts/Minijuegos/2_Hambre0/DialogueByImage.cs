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
//   - Si se está usando una secuencia personalizada, muestra la siguiente imagen en esa secuencia.
//
// void ShowImageByIndex(int index)
//   - Muestra una imagen específica del array según su índice.
//
// void ShowCustomSequence(List<int> indices)
//   - Muestra una secuencia personalizada de imágenes según los índices especificados.
//
// void HideImage()
//   - Oculta la imagen actualmente visible.

public class DialogueByImage : MonoBehaviour
{
    public Image displayImage;
    public Image nInteract;
    public Sprite[] imageArray;        // Array de imágenes.
    private int currentIndex = -1;     // Índice actual de la imagen en el array.
    private bool isShowingByIndex = false; // Flag para rastrear si se está mostrando una imagen por índice.
    private List<int> customSequence;  // Lista de índices para la secuencia personalizada.
    private int customSequenceIndex = 0; // Índice actual en la secuencia personalizada.
    public Sprite nInteractSpriteEnglish;
    public Sprite[] ImageDialogueSpanish; // Array de imágenes en español.
    public Sprite[] ImageDialogueEnglish; // Array de imágenes en inglés.

    public GameObject pausaMenu;
    public GameObject pauseMenu;
    private void Start()
    {
        // Inicializa el array de imágenes según el idioma actual.
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
                // Si estamos mostrando una imagen específica por índice, ocultar la imagen.
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
            isShowingByIndex = false; // No se está mostrando por índice específico.
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

    // Muestra una imagen específica del array según su índice.
    public void ShowImageByIndex(int index)
    {
        if (index >= 0 && index < imageArray.Length)
        {
            currentIndex = index;
            displayImage.sprite = imageArray[currentIndex];
            displayImage.enabled = true; // Activa la imagen.
            nInteract.enabled = true;
            isShowingByIndex = true; // Indica que estamos mostrando una imagen por índice específico.
            customSequence = null; // Reinicia la secuencia personalizada.
        }
    }

    // Muestra una secuencia personalizada de imágenes según los índices especificados.
    public void ShowCustomSequence(List<int> indices)
    {
        // Filtra los índices inválidos.
        customSequence = indices.FindAll(index => index >= 0 && index < imageArray.Length);
        customSequenceIndex = 0;

        if (customSequence.Count > 0)
        {
            currentIndex = customSequence[customSequenceIndex];
            displayImage.sprite = imageArray[currentIndex];
            displayImage.enabled = true; // Activa la imagen.
            nInteract.enabled   = true;
            isShowingByIndex = false; // No se está mostrando por índice específico individual.
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
        currentIndex = -1; // Reinicia el índice.
        isShowingByIndex = false; // Reinicia el estado de mostrar por índice.
        customSequence = null; // Reinicia la secuencia personalizada.
    }
}

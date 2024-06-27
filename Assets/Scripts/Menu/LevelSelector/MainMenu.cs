using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public GameObject level1;
    public GameObject level2;
        
    public GameObject level3;
        
    public GameObject level4;
    public GameObject level5;
    public GameObject level6;
        
    public GameObject level7;
        
    public GameObject level8;

    // List of predefined pivot points
    private Vector3[] pivotPoints = new Vector3[]
    {
        new Vector3(10f, 10f, 100f),
        new Vector3(1f, 10f, 1f),
        new Vector3(1f, 1f, 0f),
        new Vector3(1f, 1f, -1f),
        new Vector3(0f, 1f, -1f),
        new Vector3(-1f, 1f, -10f),
        new Vector3(-1f, 1f, 0f),
        new Vector3(-1f, 1f, 1f)

    };

    // Index of the current pivot point
    private int currentPivotPointIndex = 0;

    // Rotation speed
    public float rotationSpeed = 2f;

    // Reference to the text that will display the current pivot point
    public Text pivotPointText;

    public GameObject pulsaAIcono;
    public GameObject pressAIcon;
    public GameObject pulsaDIcono;
    public GameObject pressDIcon;
    public GameObject chooseWithIcon;
    public GameObject eligeConIcono;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject firstMenuCanvasEnglish;
    public GameObject firstMenuCanvasSpanish;
    //public GameObject engButton;
    //public GameObject espButton;

    [Header("Animations / Animaciones")]
    public Animator animatorIdle;
    public Animator animatorInteract;
    public GameObject spaceShipIdle;
    public GameObject spaceShipInteract;

    [Header("Stamps / Sellos")]
    public GameObject stamp1;
    public GameObject stamp2;
    public GameObject stamp3;
    public GameObject stamp4;
    public GameObject stamp5;
    public GameObject stamp6;
    public GameObject stamp7;
    public GameObject stamp8;

    [Header("Planet Phases")]
    public GameObject planet;
    private Renderer planetRenderer;
    public Material phase0Material;
    public Material phase1Material;
    public Material phase2Material;
    public Material phase3Material;
    public Material phase4Material;
    public Material phase5Material;
    public Material phase6Material;
    public Material phase7Material;
    public Material phase8Material;

    [Header("Sound Variables")]
    public List<AudioClip> soundClips = new List<AudioClip>(); // Lista de clips de sonido
    public AudioSource audioSource; // Referencia al componente AudioSource

    [Header("Cinematic After Completing Minigame 1 References")]
    public VideoPlayer videoAfterMinigame1English;
    public VideoPlayer videoAfterMinigame1Spanish;
    public RawImage outputVideoAfterMinigame1English;
    public RawImage outputVideoAfterMinigame1Spanish;
    private bool canPlaySecondAnimation = true;

    [Header("Cinematic Ending Game 1 References")]
    public VideoPlayer videoEndingEnglish;
    public VideoPlayer videoEndingSpanish;
    public RawImage outputVideoEndingEnglish;
    public RawImage outputVideoEndingSpanish;
    private bool canPlayLastCinematic = true;

    private void Start()
    {
        spaceShipInteract.SetActive(false);
        videoAfterMinigame1English.loopPointReached += HideVideo2English;
        videoAfterMinigame1Spanish.loopPointReached += HideVideo2Spanish;
        videoAfterMinigame1English.gameObject.SetActive(false);
        videoAfterMinigame1Spanish.gameObject.SetActive(false);
        outputVideoAfterMinigame1English.gameObject.SetActive(false);
        outputVideoAfterMinigame1Spanish.gameObject.SetActive(false);

        videoEndingEnglish.loopPointReached += BackToFirstMenu;
        videoEndingSpanish.loopPointReached += BackToFirstMenu;
        videoEndingEnglish.gameObject.SetActive(false);
        videoEndingSpanish.gameObject.SetActive(false);
        outputVideoEndingEnglish.gameObject.SetActive(false);
        outputVideoEndingSpanish.gameObject.SetActive(false);

        planetRenderer = planet.GetComponent<Renderer>();
        HandleMenuUI();
    }

    void Update()
    {
        // Check if the 'D' key is being pressed
        if (Input.GetKeyDown(KeyCode.D))
        {
            RotateToNextPivotPoint();
        }

        // Check if the 'A' key is being pressed
        if (Input.GetKeyDown(KeyCode.A))
        {
            RotateToPreviousPivotPoint();
        }

        // Ensure the index stays within bounds
        if (currentPivotPointIndex < 0)
        {
            currentPivotPointIndex = pivotPoints.Length - 1;
        }
        else if (currentPivotPointIndex >= pivotPoints.Length)
        {
            currentPivotPointIndex = 0;
        }

        // Get the current pivot point
        Vector3 targetPivotPoint = pivotPoints[currentPivotPointIndex];

        // Smoothly rotate the object towards the new pivot point
        Quaternion targetRotation = Quaternion.LookRotation(targetPivotPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
    public void RotateToNextPivotPoint()
    {
        PlaySound(1);
        StartCoroutine(WaitAndRotateToNext());
        StartCoroutine(ShowInteractAnimation());
    }

    public void RotateToPreviousPivotPoint()
    {
        PlaySound(1);
        StartCoroutine(WaitAndRotateToPrevious());
        StartCoroutine(ShowInteractAnimation());  
    }

    void DisplayPivotPointText()
    {
        // Display the text with the current pivot point
        pivotPointText.text = "Activity: " + pivotPoints[currentPivotPointIndex];
    }

    public void Exit()
    {
        PlaySound(0);
        Application.Quit();
    }

    public void StartGame()
    {
        PlaySound(0);
        SceneManager.LoadScene("Minijuego1_Paz");
    }

    private void HandleMenuUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!MinigamesCompleted.minigame1Finished && LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            firstMenuCanvasEnglish.SetActive(true);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
            stamp1.SetActive(false);
            planetRenderer.sharedMaterial = phase0Material;
        }
        else if (!MinigamesCompleted.minigame1Finished && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            firstMenuCanvasSpanish.SetActive(true);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
            stamp1.SetActive(false);
            planetRenderer.sharedMaterial = phase0Material;
        }
        else if (MinigamesCompleted.minigame1Finished && LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            firstMenuCanvasEnglish.SetActive(false);
            firstMenuCanvasSpanish.SetActive(false);
            pressAIcon.SetActive(true);
            pulsaAIcono.SetActive(false);
            pressDIcon.SetActive(true);
            pulsaDIcono.SetActive(false);
            chooseWithIcon.SetActive(true);
            eligeConIcono.SetActive(false);
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
            stamp1.SetActive(true);
            planetRenderer.sharedMaterial = phase1Material;
        }
        else if (MinigamesCompleted.minigame1Finished && LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            firstMenuCanvasEnglish.SetActive(false);
            firstMenuCanvasSpanish.SetActive(false);
            pressAIcon.SetActive(false);
            pulsaAIcono.SetActive(true);
            pressDIcon.SetActive(false);
            pulsaDIcono.SetActive(true);
            chooseWithIcon.SetActive(false);
            eligeConIcono.SetActive(true);
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
            stamp1.SetActive(true);
            planetRenderer.sharedMaterial = phase1Material;
        }

        if (MinigamesCompleted.minigame1Finished && LanguageManager.currentLanguage == LanguageManager.Language.English && !MinigamesCompleted.minigame2Finished
            && !MinigamesCompleted.minigame3Finished && !MinigamesCompleted.minigame4Finished && !MinigamesCompleted.minigame5Finished && !MinigamesCompleted.minigame6Finished
            && !MinigamesCompleted.minigame7Finished && !MinigamesCompleted.minigame8Finished)
        {
            if (canPlaySecondAnimation)
            {
                videoAfterMinigame1English.gameObject.SetActive(true);
                outputVideoAfterMinigame1English.gameObject.SetActive(true);
                videoAfterMinigame1English.Play();
                canPlaySecondAnimation = false;
            }
        }
        else if (MinigamesCompleted.minigame1Finished && LanguageManager.currentLanguage == LanguageManager.Language.Spanish && !MinigamesCompleted.minigame2Finished
            && !MinigamesCompleted.minigame3Finished && !MinigamesCompleted.minigame4Finished && !MinigamesCompleted.minigame5Finished && !MinigamesCompleted.minigame6Finished
            && !MinigamesCompleted.minigame7Finished && !MinigamesCompleted.minigame8Finished)
        {
            if (canPlaySecondAnimation)
            {
                videoAfterMinigame1Spanish.gameObject.SetActive(true);
                outputVideoAfterMinigame1Spanish.gameObject.SetActive(true);
                videoAfterMinigame1Spanish.Play();
                canPlaySecondAnimation = false;
            }
        }

        if (MinigamesCompleted.minigame2Finished)
        {
            stamp2.SetActive(true);
            planetRenderer.sharedMaterial = phase2Material;
        }
        else if (!MinigamesCompleted.minigame2Finished)
        {
            stamp2.SetActive(false);
        }

        if (MinigamesCompleted.minigame3Finished)
        {
            planetRenderer.sharedMaterial = phase3Material;
            stamp3.SetActive(true);
        }
        else if (!MinigamesCompleted.minigame3Finished)
        {
            stamp3.SetActive(false);
        }

        if (MinigamesCompleted.minigame4Finished)
        {
            planetRenderer.sharedMaterial = phase4Material;
            stamp4.SetActive(true);
        }
        else if (!MinigamesCompleted.minigame4Finished)
        {
            stamp4.SetActive(false);
        }

        if (MinigamesCompleted.minigame5Finished)
        {
            planetRenderer.sharedMaterial = phase5Material;
            stamp5.SetActive(true);
        }
        else if (!MinigamesCompleted.minigame5Finished)
        {
            stamp5.SetActive(false);
        }

        if (MinigamesCompleted.minigame6Finished)
        {
            planetRenderer.sharedMaterial = phase6Material;
            stamp6.SetActive(true);
        }
        else if (!MinigamesCompleted.minigame6Finished)
        {
            stamp6.SetActive(false);
        }

        if (MinigamesCompleted.minigame7Finished)
        {
            planetRenderer.sharedMaterial = phase7Material;
            stamp7.SetActive(true);
        }
        else if (!MinigamesCompleted.minigame7Finished)
        {
            stamp7.SetActive(false);
        }

        if (MinigamesCompleted.minigame8Finished)
        {
            planetRenderer.sharedMaterial = phase8Material;
            stamp8.SetActive(true);
        }
        else if (!MinigamesCompleted.minigame8Finished)
        {
            stamp8.SetActive(false);
        }

        if (MinigamesCompleted.minigame1Finished && MinigamesCompleted.minigame2Finished && MinigamesCompleted.minigame3Finished
            && MinigamesCompleted.minigame4Finished && MinigamesCompleted.minigame5Finished && MinigamesCompleted.minigame6Finished
            && MinigamesCompleted.minigame7Finished && MinigamesCompleted.minigame8Finished)
        {
            if (LanguageManager.currentLanguage == LanguageManager.Language.English)
            {
                StartCoroutine(WaitASecondAndLastCinematicEnglish());
            }
            else if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
            {
                StartCoroutine(WaitASecondAndLastCinematicSpanish());
            }
        }
    }

    public void SelectSpanishLanguage()
    {
        PlaySound(0);
        LanguageManager.currentLanguage = LanguageManager.Language.Spanish;
    }

    public void SelectEnglishLanguage()
    {
        PlaySound(0);
        LanguageManager.currentLanguage = LanguageManager.Language.English;
    }

    public void CanSelectLanguage()
    {
        //espButton.SetActive(true);
        //engButton.SetActive(true);
    }

    public IEnumerator ShowInteractAnimation()
    {
        spaceShipIdle.SetActive(false);
        spaceShipInteract.SetActive(true);
        yield return new WaitForSeconds(2f);
        spaceShipInteract.SetActive(false);
        spaceShipIdle.SetActive(true);
    }

    public IEnumerator WaitAndRotateToNext()
    {
        yield return new WaitForSeconds(1f);

        // Increment the index of the current pivot point
        currentPivotPointIndex = (currentPivotPointIndex + 1) % pivotPoints.Length;

        // Display the text with the current pivot point if the text is not null
        if (pivotPointText != null)
        {
            DisplayPivotPointText();
        }
    }

    public IEnumerator WaitAndRotateToPrevious()
    {
        yield return new WaitForSeconds(1f);

        // Decrement the index of the current pivot point
        currentPivotPointIndex = (currentPivotPointIndex - 1 + pivotPoints.Length) % pivotPoints.Length;

        // Display the text with the current pivot point if the text is not null
        if (pivotPointText != null)
        {
            DisplayPivotPointText();
        }
    }

    public void PlaySound(int sound)
    {
        if (audioSource != null)
        {
            audioSource.clip = soundClips[sound];
            audioSource.Play();
        }

    }

    public void PlaysSoundButton()
    {
        PlaySound(0);
    }

    void HideVideo2English(VideoPlayer vp)
    {
        outputVideoAfterMinigame1English.gameObject.SetActive(false);
        videoAfterMinigame1English.gameObject.SetActive(false);
    }

    void HideVideo2Spanish(VideoPlayer vp)
    {
        outputVideoAfterMinigame1Spanish.gameObject.SetActive(false);
        videoAfterMinigame1Spanish.gameObject.SetActive(false);
    }

    void BackToFirstMenu(VideoPlayer vp)
    {
        MinigamesCompleted.minigame1Finished = false;
        MinigamesCompleted.minigame2Finished = false;
        MinigamesCompleted.minigame3Finished = false;
        MinigamesCompleted.minigame4Finished = false;
        MinigamesCompleted.minigame5Finished = false;
        MinigamesCompleted.minigame6Finished = false;
        MinigamesCompleted.minigame7Finished = false;
        MinigamesCompleted.minigame8Finished = false;
        SceneManager.LoadScene("LevelSelector");
    }

    private IEnumerator WaitASecondAndLastCinematicEnglish()
    {
        if (canPlayLastCinematic)
        {
            yield return new WaitForSeconds(1f);
            videoEndingEnglish.gameObject.SetActive(true);
            outputVideoEndingEnglish.gameObject.SetActive(true);
            videoEndingEnglish.Play();
            canPlayLastCinematic = false;
        }
    }

    private IEnumerator WaitASecondAndLastCinematicSpanish()
    {
        if (canPlayLastCinematic)
        {
            yield return new WaitForSeconds(1f);
            videoEndingSpanish.gameObject.SetActive(true);
            outputVideoEndingSpanish.gameObject.SetActive(true);
            videoEndingSpanish.Play();
            canPlayLastCinematic = false;
        }
    }
}

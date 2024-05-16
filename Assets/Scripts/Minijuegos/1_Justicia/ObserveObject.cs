using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ObserveObject : MonoBehaviour
{
    #region Cameras Variables
    [Header("Camera References")]
    public GameObject clueCamera;
    public GameObject playerCamera;

    #endregion Cameras Variables

    #region Clues References
    [Header("Clues References")]
    public GameObject[] clues;
    //public bool canAddRotateToButton = true;
    public RotateClue currentClue;
    public GameObject RotateUI;
    public Button leftRotateButton;
    public Button rightRotateButton;
    public int currentClueId;
    public Vector3 previousCluePosition;
    public int clueIndex;

    private bool canActivateClues;
    private bool canDeactivateClues;

    #endregion Clues References

    #region Take Clues Variables
    public static List<GameObject> TakenClues = new List<GameObject>();

    #endregion Take Clues Variables

    #region MiniGame Phases Variables
    public enum JusticePhases
    {
        INVESTIGATION,
        ANALYSIS,
        JUDGMENT
    }

    public JusticePhases phases;
    public GameObject lenseAimGlass;
    public GameObject judgementPanel;

    #endregion MiniGame Phases Variables

    #region Analysis Panel Fade In Effect
    public PanelFader panelFader;
    public static bool cantMove;
    public Button clue1AnalyseButton;
    public GameObject tick1;
    public Button clue2AnalyseButton;
    public GameObject tick2;
    public Button clue3AnalyseButton;
    public GameObject tick3;
    public bool analyzing;
    public bool canFadeIn = true;
    public bool notAnalyzing = true;
    public Button analyzeButton;
    public Slider analyzeSlider;
    private bool canJudge = true;
    public PanelFader judgementPanelFader;
    private bool judgementPanelCanFade = true;

    #endregion Analysis Panel Fade In Effect

    #region UI Handle depending on Language

    public TextMeshProUGUI analyzingText1;
    public TextMeshProUGUI analyzingText2;
    public TextMeshProUGUI analyzingText3;
    public TextMeshProUGUI analyzingButtonText;

    #endregion UI Handle depending on Language

    public GameObject sellosPanel;
    public GameObject SellosPanelPanel;
    public GameObject interactIndicator;

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        clueCamera.SetActive(false);
        playerCamera.SetActive(true);
        RotateUI.SetActive(false);
        judgementPanel.SetActive(false);
        clue1AnalyseButton.gameObject.SetActive(false);
        clue2AnalyseButton.gameObject.SetActive(false);
        clue3AnalyseButton.gameObject.SetActive(false);
        tick1.SetActive(false);
        tick2.SetActive(false);
        tick3.SetActive(false);
        analyzeButton.gameObject.SetActive(false);
        analyzeSlider.gameObject.SetActive(false);
        sellosPanel.SetActive(false);
        SellosPanelPanel.SetActive(false);
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerController.playerEnteredInObjectClue1Area)
        {
            Clue1CameraChange();
        }

        if (Input.GetKeyDown(KeyCode.E) && PlayerController.playerEnteredInObjectClue2Area)
        {
            Clue2CameraChange();
        }

        if (Input.GetKeyDown(KeyCode.E) && PlayerController.playerEnteredInObjectClue3Area)
        {
            Clue3CameraChange();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && clueCamera.activeInHierarchy && notAnalyzing)
        {
            CameraBackToPlayer();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && clueCamera.activeInHierarchy && !notAnalyzing)
        {
            CameraBackToCluePanel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            LanguageManager.currentLanguage = LanguageManager.Language.Spanish;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            LanguageManager.currentLanguage = LanguageManager.Language.English;
        }

        HandleMinigamePhases();
        CheckCameraAnalysis();
        HandleAnalyzingUI();
        HandleAnalyzingUIDependingOnLanguage();
    }

    #endregion Update

    #region Change from PlayerCamera to ClueCamera and from ClueCamera to PlayerCamera

    private void Clue1CameraChange()
    {
        clueCamera.SetActive(true);
        playerCamera.SetActive(false);
        clueCamera.transform.position = clues[0].transform.position + new Vector3(0, 0.5f, -0.5f); //Place the camera in front of the correct clue
        clueCamera.transform.eulerAngles = clueCamera.transform.eulerAngles + new Vector3(45, 0, 0);

        clueIndex = 0;
        ManageUIClueType();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Clue1AnalysisCameraChange()
    {
        //panelFader.panelFaded = false;
        if (panelFader.canvGroup.alpha >= 1)
        {
            panelFader.Fade();
        }

        if (!analyzeButton.gameObject.activeInHierarchy)
        {
            analyzeButton.gameObject.SetActive(true);
            
        }

        if (!analyzeSlider.gameObject.activeInHierarchy)
        {
            analyzeSlider.gameObject.SetActive(true);
        }

        clueCamera.transform.position = clues[0].transform.position + new Vector3(0, 0.5f, -0.5f); //Place the camera in front of the correct clue
        clueCamera.transform.eulerAngles = clueCamera.transform.eulerAngles + new Vector3(45, 0, 0);

        clueIndex = 0;
        ManageUIClueType();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Clue2CameraChange()
    {
        clueCamera.SetActive(true);
        playerCamera.SetActive(false);
        clueCamera.transform.position = clues[1].transform.position + new Vector3(0, 0.5f, -0.5f); //Place the camera in front of the correct clue
        //clueCamera.transform.eulerAngles = clueCamera.transform.eulerAngles + new Vector3(45, 0, 0);
        clueIndex = 1;
        ManageUIClueType();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Clue2AnalysisCameraChange()
    {
        clueCamera.SetActive(true);
        playerCamera.SetActive(false);
        clueCamera.transform.position = clues[1].transform.position + new Vector3(0, 0.5f, -0.5f); //Place the camera in front of the correct clue
        //clueCamera.transform.eulerAngles = clueCamera.transform.eulerAngles + new Vector3(45, 0, 0);

        if (panelFader.canvGroup.alpha >= 1)
        {
            panelFader.Fade();
        }

        if (!analyzeButton.gameObject.activeInHierarchy)
        {
            analyzeButton.gameObject.SetActive(true);

        }

        if (!analyzeSlider.gameObject.activeInHierarchy)
        {
            analyzeSlider.gameObject.SetActive(true);
        }

        clueIndex = 1;
        ManageUIClueType();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Clue3CameraChange()
    {
        clueCamera.SetActive(true);
        playerCamera.SetActive(false);
        clueCamera.transform.position = clues[2].transform.position + new Vector3(0, 0.5f, -0.5f); //Place the camera in front of the correct clue
        //clueCamera.transform.eulerAngles = clueCamera.transform.eulerAngles + new Vector3(45, 0, 0);
        clueIndex = 2;
        ManageUIClueType();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Clue3AnalysisCameraChange()
    {
        clueCamera.SetActive(true);
        playerCamera.SetActive(false);
        clueCamera.transform.position = clues[2].transform.position + new Vector3(0, 0.5f, -0.5f); //Place the camera in front of the correct clue
        //clueCamera.transform.eulerAngles = clueCamera.transform.eulerAngles + new Vector3(45, 0, 0);
        if (panelFader.canvGroup.alpha >= 1)
        {
            panelFader.Fade();
        }

        if (!analyzeButton.gameObject.activeInHierarchy)
        {
            analyzeButton.gameObject.SetActive(true);

        }

        if (!analyzeSlider.gameObject.activeInHierarchy)
        {
            analyzeSlider.gameObject.SetActive(true);
        }

        clueIndex = 2;
        ManageUIClueType();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CameraBackToPlayer()
    {
        clueCamera.SetActive(false);
        playerCamera.SetActive(true);
        RotateUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CameraBackToCluePanel()
    {
        if (panelFader.canvGroup.alpha >= 0.5)
        {
            clueCamera.SetActive(false);
            playerCamera.SetActive(true);

            if (analyzeButton.gameObject.activeInHierarchy)
            {
                analyzeButton.gameObject.SetActive(false);

            }

            if (analyzeSlider.gameObject.activeInHierarchy)
            {
                analyzeSlider.gameObject.SetActive(false);
            }
        }
        
        RotateUI.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        if (panelFader.canvGroup.alpha <= 0)
        {
            panelFader.Fade();
        }
    }

    #endregion Change from PlayerCamera to ClueCamera and from ClueCamera to PlayerCamera

    #region Rotate Clue

    private void ManageUIClueType()
    {
        switch (clues[clueIndex].GetComponent<RotateClue>().clueType)
        {
            case RotateClue.ClueType.NOTROTATE:
                break;

            case RotateClue.ClueType.ROTATE:
                RotateUI.SetActive(true);

                if (clues[clueIndex].GetComponent<RotateClue>().canAddRotateToButton)
                {
                    leftRotateButton.onClick.RemoveAllListeners();
                    rightRotateButton.onClick.RemoveAllListeners();


                    leftRotateButton.onClick.AddListener(clues[clueIndex].GetComponent<RotateClue>().RotateLeftX);
                    rightRotateButton.onClick.AddListener(clues[clueIndex].GetComponent<RotateClue>().RotateRightX);
                    //clues[clueIndex].GetComponent<RotateClue>().canAddRotateToButton = false;
                }
                break;

            default:
                break;
        }
    }

    #endregion Rotate Clue

    #region Handle Minigame Phases
    private void HandleMinigamePhases()
    {
        switch (phases)
        {
            case JusticePhases.INVESTIGATION:
                lenseAimGlass.SetActive(true);
                judgementPanel.SetActive(false);
                notAnalyzing = true;
                cantMove = false;
                canDeactivateClues = true;
                //if (canDeactivateClues)
                //{
                    //for (int i = 0; i < clues.Length; i++)
                    //{
                        //clues[i].SetActive(false);
                    //}

                    //canDeactivateClues = false;
                //}

                if (AimLenseGlass.takenClues >= 3)
                {
                    phases = JusticePhases.ANALYSIS;
                }
                break;
            case JusticePhases.ANALYSIS:
                judgementPanel.SetActive(false);
                lenseAimGlass.SetActive(false);
                interactIndicator.SetActive(false);
                notAnalyzing = false;
                analyzing = true;
                cantMove = true;
                FixFadeInAndOut();
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                clue1AnalyseButton.gameObject.SetActive(true);
                
                HandleAnalyzingUI();

                canActivateClues = true;
                if (canActivateClues)
                {
                    for (int i = 0; i < clues.Length; i++)
                    {
                        clues[i].SetActive(true);
                    }

                    canActivateClues = false;
                }

                break;
            case JusticePhases.JUDGMENT:
                notAnalyzing = true;
                judgementPanel.SetActive(true);
                FixJudgementPanelFadeInAndOut();
                cantMove = false;
                break;
            default:
                break;
        }
    }

    private void CheckCameraAnalysis()
    {
        if (panelFader.canvGroup.alpha >= 0.95 && analyzing)
        {
            clueCamera.SetActive(true);
            playerCamera.SetActive(false);

            analyzing = false;
        }
    }

    private void FixFadeInAndOut()
    {
        if (canFadeIn)
        {
            panelFader.Fade();
            canFadeIn = false;
        }
        
    }

    private void FixJudgementPanelFadeInAndOut()
    {
        if (judgementPanelCanFade)
        {
            judgementPanelFader.Fade();
            judgementPanelCanFade = false;
        }
    }

    #endregion Handle Minigame Phases

    #region Handle Analyzing UI
    private void HandleAnalyzingUI()
    {
        switch (AnalyzeClue.cluesAnalyzed)
        {
            case 0:
                if (clue2AnalyseButton.gameObject.activeInHierarchy)
                {
                    clue2AnalyseButton.gameObject.SetActive(false);
                }

                if (clue3AnalyseButton.gameObject.activeInHierarchy)
                {
                    clue3AnalyseButton.gameObject.SetActive(false);
                }
                
                break;
            case 1:
                if (clue1AnalyseButton.gameObject.activeInHierarchy)
                {
                    clue1AnalyseButton.gameObject.SetActive(false);
                }

                if (!clue2AnalyseButton.gameObject.activeInHierarchy)
                {
                    clue2AnalyseButton.gameObject.SetActive(true);
                }

                if (!tick1.activeInHierarchy)
                {
                    tick1.SetActive(true);
                }

                if (clue3AnalyseButton.gameObject.activeInHierarchy)
                {
                    clue3AnalyseButton.gameObject.SetActive(false);
                }
                break;
            case 2:
                if (clue1AnalyseButton.gameObject.activeInHierarchy)
                {
                    clue1AnalyseButton.gameObject.SetActive(false);
                }

                if (clue2AnalyseButton.gameObject.activeInHierarchy)
                {
                    clue2AnalyseButton.gameObject.SetActive(false);
                }

                if (!tick2.activeInHierarchy)
                {
                    tick2.SetActive(true);
                }

                if (!clue3AnalyseButton.gameObject.activeInHierarchy)
                {
                    clue3AnalyseButton.gameObject.SetActive(true);
                }
                break;
            case 3:
                if (clue1AnalyseButton.gameObject.activeInHierarchy)
                {
                    clue1AnalyseButton.gameObject.SetActive(false);
                }

                if (clue3AnalyseButton.gameObject.activeInHierarchy)
                {
                    clue3AnalyseButton.gameObject.SetActive(false);
                }

                if (!tick3.activeInHierarchy)
                {
                    tick3.SetActive(true);
                }

                FixWaitForJudgment();

                /*if (!analisysPanel.gameObject.activeInHierarchy)
                {
                    analysisPanel.
                }*/

                break;
            default:
                break;
        }
        ;
    }

    private void HandleAnalyzingUIDependingOnLanguage()
    {
        if (LanguageManager.currentLanguage == LanguageManager.Language.Spanish)
        {
            analyzingText1.text = "Analizar";
            analyzingText2.text = "Analizar";
            analyzingText3.text = "Analizar";
            analyzingButtonText.text = "Analizar";
        }

        if (LanguageManager.currentLanguage == LanguageManager.Language.English)
        {
            analyzingText1.text = "Analyze";
            analyzingText2.text = "Analyze";
            analyzingText3.text = "Analyze";
            analyzingButtonText.text = "Analyze";
        }

    }

    private IEnumerator WaitForJudgment()
    {
        yield return new WaitForSeconds(1f);
        phases = JusticePhases.JUDGMENT;
    }

    private void FixWaitForJudgment()
    {
        if (canJudge)
        {
            StartCoroutine(WaitForJudgment());
            canJudge = false;
        }
        
    }

    #endregion Handle Analyzing UI

    public void SelectGuiltyAndFinishMinigame1()
    {
        MinigamesCompleted.minigame1Finished = true;
        SellosPanelPanel.SetActive(true);
        sellosPanel.SetActive(true);
        Debug.Log("Button pressed");
        StartCoroutine(Wait());   
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("LevelSelector");
    }
}

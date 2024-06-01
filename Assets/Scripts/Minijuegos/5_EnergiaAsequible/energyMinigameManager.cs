using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class energyMinigameManager : MonoBehaviour
{
    #region Minigame Phases


    public enum Phase
    {
        PHASE1, //Mount solar plaques
        PHASE2, //Install solar plaques
        PHASE3  //Install cables
    }

    [Header("Minigame Phase")]
    [Tooltip("Tres fases: Montaje placa solar, Instalaci�n placas y Cableado")]
    public Phase phase;
    public PanelFader cablePanelFader;

    #endregion Minigame Phases

    #region Phase 3 Variables
    [Header("Phase 3 Variables")]
    public GameObject cablePanel;
    public GameObject congratulationsPhase3Panel;
    //public TextMeshProUGUI congratulationsPhase3PanelText;

    public static int globalElectricity;
    public TextMeshProUGUI globalElectricityText;
    public SlotScript slotScript;

    [Header("Check if cables positioned")]
    public NewDragAnDrop cable1;
    public NewDragAnDrop cable2;
    public NewDragAnDrop cable3;
    public NewDragAnDrop cable4;
    public NewDragAnDrop cable5;
    public NewDragAnDrop cable6;

    #endregion Phase 3 Variables

    [Header("SolarLight References")]
    public SolarLight solarLightPlace1;
    public SolarLight solarLightPlace2;
    public SolarLight solarLightPlace3;
    public SolarLight solarLightPlace4;

    public static bool minigamePhase2Completed = false;

    [Header("Instructions Panel")]
    public GameObject instructionsPanel;
    public TextMeshProUGUI instructionsPanelText;

    [Header("Phase 3 slot cables")]
    public GameObject[] slotCables;

    [Header("Stamp Panel")]
    public GameObject stampPanel;
    public bool canCheckElectricity = true;

    [Header("Cable Camera")]
    [Space]
    public Camera cableCamera;

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < slotCables.Length; i++)
        {
            slotCables[i].GetComponent<SlotScript>().initialSlotIndividualElectricity = slotCables[i].GetComponent<SlotScript>().slotIndividualElectricity;
        }

        cablePanel.SetActive(false);
        instructionsPanel.SetActive(true);
        instructionsPanelText.text = "Fase 1. Encuentra las partes de la placa solar y mira alrededor de ellas hasta que salga que puedes hacer click izquierdo sobre ellas.";
        congratulationsPhase3Panel.SetActive(false);
        slotScript.electricityFailObject.SetActive(false);
        stampPanel.SetActive(false);
        cableCamera.gameObject.SetActive(false); 
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.K))
        {
            phase = Phase.PHASE3;
            HandleMinigamePhase();
        }*/

        HandleGlobalElectricityUI();
        SumGlobalElectricity();
        CheckGlobalElectricity();
    }

    #endregion Update

    #region Handle MiniGame Phases

    private void HandleMinigamePhase()
    {
        switch (phase)
        {
            case Phase.PHASE1:
                //L�gica de que si presionas la pieza se inicie la animaci�n de ponerse en su sitio

                //Comprobar si pieza1, pieza2, pieza3, pieza4, pieza5, pieza6 y pieza7 est�n bien colocada
                    //Pasar a siguiente fase

                break;
            case Phase.PHASE2:
                //L�gica de sol y de porcentaje de luz natural solar seg�n la luz que le da o el momento del d�a

                //Comprobar si pieza8, pieza9, pieza10 y piez11 est�n bien colocadas
                    //Pasar a siguiente fase

                break;
            case Phase.PHASE3:
                //Se muestra panel de cableado
                cablePanel.SetActive(true);
                cablePanelFader.Fade();

                //L�gica de cableado con drag and drop de los cables a su lugar correspondiente

                //Comprobar si todos los cables est�n bien puestos
                    //Mostrar panel de ahorro energ�tico y como las casas empiezan a gastar menos energ�a
                    //Despu�s mostrar el robot que agradece al jugador y le otorga uno de los sellos

                break;
            default:
                break;
        }
    }

    #endregion Handle MiniGame Phases

    private void HandleGlobalElectricityUI()
    {
        if (phase == Phase.PHASE3)
        {
            globalElectricityText.text = "Global Electricity: " + globalElectricity + " / 5";
        }
        else
        {
            globalElectricityText.text = string.Empty;
        }
    }

    private void CheckGlobalElectricity()
    {
        if (cable1.isPositioned && cable2.isPositioned && cable3.isPositioned && cable4.isPositioned && cable5.isPositioned && cable6.isPositioned)
        {
            if (globalElectricity == 5 && canCheckElectricity)
            {
                Debug.Log("Congratulations");
                congratulationsPhase3Panel.SetActive(true);
                StartCoroutine(ShowStampPanel());
                canCheckElectricity = false;
            }
            else if (globalElectricity != 5 && canCheckElectricity)
            {
                StartCoroutine(slotScript.ShowElectrictyFail());

                

                cable1.ResetPosition();
                cable2.ResetPosition();
                cable3.ResetPosition();
                cable4.ResetPosition();
                cable5.ResetPosition();
                cable6.ResetPosition();
                ResetSlots();
                //canCheckElectricity = false;
            }
        }
        
    }

    public void SumGlobalElectricity()
    {
        int temp = 0;

        for (int i = 0; i < slotCables.Length; i++)
        {
            temp += slotCables[i].GetComponent<SlotScript>().slotIndividualElectricity;
            
        }

        globalElectricity = temp;
    }

    public void PassToPhase2()
    {
        SolarLight.randomCorroutinCanStart = true;
        solarLightPlace1.StartCoroutine(solarLightPlace1.RandomChangeSunPercent());
        solarLightPlace2.StartCoroutine(solarLightPlace2.RandomChangeSunPercent());
        solarLightPlace3.StartCoroutine(solarLightPlace3.RandomChangeSunPercent());
        solarLightPlace4.StartCoroutine(solarLightPlace4.RandomChangeSunPercent());
        phase = Phase.PHASE2;
    }

    public void PassToPhase3()
    {
        instructionsPanelText.text = "Fase 3. Conecta los cables por medio de arrastrarlos y soltarlos, de forma que sumen la electricidad necesaria.";
        instructionsPanel.transform.localPosition = new Vector3(438, 452, 0);
        instructionsPanel.transform.localScale = new Vector3(0.64f, 0.64f, 0.64f);
        phase = Phase.PHASE3;
        HandleMinigamePhase();
    }

    private IEnumerator ShowStampPanel()
    {
        yield return new WaitForSeconds(2f);
        stampPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("LevelSelector");
    }

    private void ResetSlots()
    {
        for (int i = 0; i < slotCables.Length; i++)
        {
            slotCables[i].GetComponent<SlotScript>().ResetSlot();
        }
    }
}

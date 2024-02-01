using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    #region Type of Catastrophes and Catastrophes Variables

    public enum Catastrophes
    {
        WATERLEAK,
        BACTERIA,
        LOWPH,
        HIGHPH,
        HIGHCONTAMINATION
    }

    [Header("Catastrophes")]
    public Catastrophes catastrophes;
    public float waterContamination;
    public float contaminationSpeed;
    public Slider contaminationSlider;

    #endregion Type of Catastrophes and Catastrophes Variables

    #region Minigame Phases

    public enum Phases
    {
        TREATMENTPLANT,
        TOWN
    }

    [Header("Minigame Phases")]
    public Phases phases;

    #endregion Minigame Phases

    #region Resources

    [Header("Resources")]
    public int money = 1000;
    public TextMeshProUGUI moneyText;
    public int points;

    #endregion Resources

    #region Verify Pipelines Correct Position Variables

    [Header("Verify Pipelines Correct Position")]
    public GameObject[] pipelines;
    public GameObject[] pipelinesIcons;
    public bool pipeline1CorrectPlaced;

    #endregion Verify Pipelines Correct Position Variables

    #region Select Pipelines Variables

    [Header("Select Pipelines")]
    public int selectedPipeline = 0;
    public int selectedPipelineIcon = 0;
    public int previousPipeline;

    #endregion Select Pipelines Variables

    #region Difficulty Levels Variables

    public enum DifficultyLevel
    {
        EASY,
        INTERMEDIATE,
        HARD
    }

    [Header("Difficulty Level")]
    public DifficultyLevel difficultyLevel;

    #endregion Difficulty Levels Variables

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        SelectPipeline();

        //StartCoroutine(ContaminationTransition());
    }

    #endregion Start

    #region Update

    // Update is called once per frame
    void Update()
    {
        if (pipelines[0].transform.rotation.x == 90f)
        {
            pipeline1CorrectPlaced = true;
        }

        moneyText.text = "Money: " + money;

        CheckSelectedPipeline();

        //waterContamination = Mathf.Lerp(0.5f, 1f, contaminationSpeed);

        contaminationSlider.value = waterContamination;
    }

    #endregion Update

    #region Select Pipeline Types

    void SelectPipeline()
    {
        int i = 0;

        foreach (GameObject pipeline in pipelines)
        {
            if (i == selectedPipeline)
            {
                pipeline.SetActive(true);
                pipelinesIcons[i].gameObject.SetActive(true);
                pipeline.gameObject.transform.position = pipelines[0].transform.position;
            }
            else
            {
                pipeline.SetActive(false);
                pipelinesIcons[i].gameObject.SetActive(false);
            }
            i++;
        }

    }

    public void SelectPipelineLeft()
    {
        if (money >= 100 && money >= 0)
        {
            if (selectedPipeline <= 0)
            {
                selectedPipeline = pipelines.Length - 1;
                selectedPipelineIcon = pipelinesIcons.Length - 1;
            }
            else
            {
                selectedPipeline--;
                selectedPipelineIcon--;
            }

            if (previousPipeline != selectedPipeline)
            {
                SelectPipeline();
                money -= 100;
            }
        }
        
    }

    public void SelectPipelineRight()
    {
        if (money >= 100 && money >= 0)
        {
            if (selectedPipeline >= pipelines.Length - 1)
            {
                selectedPipeline = 0;
                selectedPipelineIcon = 0;
            }
            else
            {
                selectedPipeline++;
                selectedPipelineIcon++;
            }

            if (previousPipeline != selectedPipeline)
            {
                SelectPipeline();
                money -= 100;
            }
        }
        
    }

    private void CheckSelectedPipeline()
    {
        previousPipeline = selectedPipeline;

        if (previousPipeline != selectedPipeline)
        {
            SelectPipeline();
        }
    }

    #endregion Select Pipeline Types

    #region Contamination Transition

    public IEnumerator ContaminationTransition()
    {
        float elapsedTime = 0;

        while (elapsedTime < contaminationSpeed)
        {
            elapsedTime += Time.deltaTime;

            waterContamination = Mathf.Lerp(0f, 1f, elapsedTime / contaminationSpeed);
            yield return null;
        }

        //StartCoroutine(ContaminationTransition());
    }

    #endregion Contamination Transition

    #region Difficulty Levels

    public void EasyLevel()
    {
        PipelineForeground.maxNumOfMovements = 10;
        difficultyLevel = DifficultyLevel.EASY;
    }

    public void IntermediateLevel()
    {
        PipelineForeground.maxNumOfMovements = 7;
        difficultyLevel = DifficultyLevel.INTERMEDIATE;
    }

    public void HardLevel()
    {
        PipelineForeground.maxNumOfMovements = 5;
        difficultyLevel = DifficultyLevel.HARD;
    }

    #region Manage Points According to the DifficultyLevel

    public void ManagePointsDifficulty()
    {
        switch (difficultyLevel)
        {
            case DifficultyLevel.EASY:
                break;
            case DifficultyLevel.INTERMEDIATE:
                break;
            case DifficultyLevel.HARD:
                break;
            default:
                break;
        }
    }

    #endregion Manage Points According to the DifficultyLevel

    #endregion Difficulty Levels
}


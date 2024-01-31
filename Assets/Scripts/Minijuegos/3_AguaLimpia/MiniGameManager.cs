using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    public enum Catastrophes
    {
        WATERLEAK,
        BACTERIA,
        LOWPH,
        HIGHPH,
        HIGHCONTAMINATION
    }

    public Catastrophes catastrophes;
    public float waterContamination;
    public float contaminationSpeed;

    public enum Phases
    {
        TREATMENTPLANT,
        TOWN
    }

    public Phases phases;

    public int money = 1000;
    public TextMeshProUGUI moneyText;
    public int points;

    public GameObject[] pipelines;
    public bool pipeline1CorrectPlaced;

    public int selectedPipeline = 0;
    public GameObject[] pipelinesIcons;
    public int selectedPipelineIcon = 0;
    public int previousPipeline;

    // Start is called before the first frame update
    void Start()
    {
        SelectPipeline();
    }

    // Update is called once per frame
    void Update()
    {
        if (pipelines[0].transform.rotation.x == 90f)
        {
            pipeline1CorrectPlaced = true;
        }

        moneyText.text = "Money: " + money;

        CheckSelectedPipeline();

        waterContamination = Mathf.Lerp(0f, 1f, contaminationSpeed);
    }

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
}


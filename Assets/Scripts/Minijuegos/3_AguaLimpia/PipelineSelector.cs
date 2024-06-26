using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipelineSelector : MonoBehaviour
{
    [Header("References")]
    [Space]
    public GameObject[] pipelinesToRotateAndSelect;
    public GameObject[] pipelinesIcons;
    public Camera pipelineCamera;
    public Button leftButton;
    public Button rightButton;
    //public int money = 500;

    private int selectedPipeline = 0;
    private int previousPipeline = -1;

    [Header("Minigame Manager")]
    public MiniGameManager minigameManager;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SelectPipeline();
        leftButton.onClick.AddListener(SelectPipelineLeft);
        rightButton.onClick.AddListener(SelectPipelineRight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivatePipelines()
    {
        int i = 0;
        foreach (GameObject pipeline in pipelinesToRotateAndSelect)
        {
            pipeline.SetActive(false);
            pipelinesIcons[i].gameObject.SetActive(false);
            i++;
        }
    }

    public void SelectPipeline()
    {
        int i = 0;
        foreach (GameObject pipeline in pipelinesToRotateAndSelect)
        {
            if (i == selectedPipeline)
            {
                minigameManager.PlaySound(3);
                pipeline.SetActive(true);
                PlayerController.pipelineEntered = pipeline;
                minigameManager.pipelineActiveGameObject = pipeline;
                if (pipeline.transform.parent.GetComponent<PipelineForeground>() != null)
                {
                    minigameManager.pipelineActive = pipeline.transform.parent.GetComponent<PipelineForeground>();
                    minigameManager.pipelineActiveGameObject = pipeline.transform.parent.gameObject;
                }
                else
                {
                    minigameManager.pipelineActive = pipeline.GetComponent<PipelineForeground>();
                }
                
                pipelinesIcons[i].gameObject.SetActive(true);
                //pipeline.transform.position = pipelinesToRotateAndSelect[0].transform.position;
                //PipelineForeground pipelineActive = pipeline.GetComponent<PipelineForeground>();
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
        if (selectedPipeline <= 0)
        {
            selectedPipeline = pipelinesToRotateAndSelect.Length - 1;
        }
        else
        {
            selectedPipeline--;
        }

        if (previousPipeline != selectedPipeline)
        {
            SelectPipeline();
        }
        previousPipeline = selectedPipeline;
    }

    public void SelectPipelineRight()
    {
        if (selectedPipeline >= pipelinesToRotateAndSelect.Length - 1)
        {
            selectedPipeline = 0;
        }
        else
        {
            selectedPipeline++;
        }

        if (previousPipeline != selectedPipeline)
        {
            SelectPipeline();
        }
        previousPipeline = selectedPipeline;
    }
}

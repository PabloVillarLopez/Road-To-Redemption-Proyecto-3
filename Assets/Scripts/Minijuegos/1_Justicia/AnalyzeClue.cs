using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnalyzeClue : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static float timer = 0;
    public static int cluesAnalyzed;
    public bool buttonPressed;
    public Slider analyzeSlider;
    public GameObject analyzeButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed && timer < 5)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
        }

        if (timer >= 5)
        {
            cluesAnalyzed++;
            Debug.Log(cluesAnalyzed);
            analyzeSlider.gameObject.SetActive(false);
            analyzeButton.SetActive(false);
            buttonPressed = false;
            timer = 0;
        }

        if (!buttonPressed)
        {
            timer = 0;
        }

        analyzeSlider.value = timer;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class ApplyMaterial : MonoBehaviour
{
    public Material originalMaterial; 
    private Material material;
    [SerializeField] public float noiseStrength = 46.7f;
    [SerializeField] public float objectHeight = 1.0f;
    private GameObject MiniGameManager;
    private MiniGameManager7 minigame;
    private void Awake()
    {
        Material uniqueMaterial = new Material(originalMaterial);
        GetComponent<Renderer>().material = uniqueMaterial;
        material = GetComponent<Renderer>().material;
         MiniGameManager = GameObject.Find("GameManager");
         minigame = MiniGameManager.GetComponent<MiniGameManager7>();

    }

    public void SetHeight(float height)
    {
        material.SetFloat("_CutoffHeight", height);
        material.SetFloat("_NoiseStrength", 40f);
        material.SetFloat("_NoiseScale", 46.34f);
        
    }
    public void AddHeight(float additionalHeight)
    {
        float currentHeight = material.GetFloat("_CutoffHeight");
        float newHeight = currentHeight + additionalHeight;
        material.SetFloat("_CutoffHeight", newHeight);
        Debug.Log(newHeight);
        if(newHeight >= 30 && newHeight <=49)
        {
            material.SetFloat("_CutoffHeight", 50);
            minigame.updateProgressRepareText();
        }
    }

}

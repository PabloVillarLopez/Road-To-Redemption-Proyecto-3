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
        material.SetFloat("_NoiseStrength", 10f);
        material.SetFloat("_NoiseScale", 46.34f);
        
    }
    public void AddHeight(float additionalHeight)
    {
        float currentHeight = material.GetFloat("_CutoffHeight");
        float newHeight = currentHeight + additionalHeight;
        material.SetFloat("_CutoffHeight", newHeight);
        Debug.Log(newHeight);

        if (newHeight >= 20 && newHeight <= 32)
        {
            material.SetFloat("_CutoffHeight", 50);
            minigame.updateProgressText(-1);

            // Corregir el límite del bucle for para evitar el error fuera de índice
            for (int i = 0; i < minigame.wallsBad.Length; i++) // Cambiado <= por <
            {
                if (minigame.wallsBad[i] != null && minigame.wallsBad[i] == gameObject)
                {
                    minigame.wallsBad[i].SetActive(false);
                    minigame.wallsGood[i].SetActive(true);
                    Debug.Log("Desactivo valla"); // Cambiado print por Debug.Log
                    break; // Salir del bucle una vez encontrado el objeto
                }
            }
        }
    }

}

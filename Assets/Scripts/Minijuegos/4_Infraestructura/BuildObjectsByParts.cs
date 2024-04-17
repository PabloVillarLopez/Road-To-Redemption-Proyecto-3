using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObjectsByParts : MonoBehaviour
{
    public GameObject part1;
    public GameObject part2;
    public GameObject part3;
    public Material hologram;


    private Material ownMaterialPart1;
    private Material ownMaterialPart2;
    private Material ownMaterialPart3;

    private int phases = -1;
    private MiniGameManager4 gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<MiniGameManager4>();

       

        ownMaterialPart1 = part1.GetComponent<MeshRenderer>().material;
        ownMaterialPart2 = part2.GetComponent<MeshRenderer>().material;
        ownMaterialPart3 = part3.GetComponent<MeshRenderer>().material;


        part1.GetComponent<MeshRenderer>().material = hologram;
        part2.SetActive(false);
        part3.SetActive(false);


        BuildObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        
    }

    public void BuildObject()
    {
        phases++;
        Debug.Log("Pieza construida");
        switch (phases)
        {
            case 0:
                break;

            case 1:

                part1.GetComponent<MeshRenderer>().material = ownMaterialPart1;

                part2.SetActive(true);
                part2.GetComponent<MeshRenderer>().material = hologram;
                gameManager.PhaseBuild();
                
                break;

            case 2:

                part2.GetComponent<MeshRenderer>().material = ownMaterialPart2;

                part3.SetActive(true);
                part3.GetComponent<MeshRenderer>().material = hologram;
                gameManager.PhaseBuild();

                break;

            case 3:

                part3.GetComponent<MeshRenderer>().material = ownMaterialPart3;
                gameManager.phaseBuild=0;



                break;

            default:
                break;
        }
    }
}

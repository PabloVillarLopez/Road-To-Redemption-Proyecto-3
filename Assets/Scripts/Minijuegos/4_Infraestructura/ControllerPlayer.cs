using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static energyMinigameManager;

public class ControllerPlayer : MonoBehaviour
{

    private MiniGameManager4 gameManager;
   
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<MiniGameManager4>();

    }

    // Update is called once per frame
    void Update()
    {
        CastRayFromMousePosition();
    }



    void CastRayFromMousePosition()
    {
        if(Camera.main != null) { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("CatchAble"))
            {

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hit.collider.GetComponent<ObjectToCatch>() != null && hit.collider.GetComponent<ObjectToCatch>().id == gameManager.idMaterial1)
                    {
                        gameManager.countMaterial1++;
                        hit.collider.gameObject.SetActive(false);


                    }
                    else if (hit.collider.GetComponent<ObjectToCatch>() != null && hit.collider.GetComponent<ObjectToCatch>().id == gameManager.idMaterial2)
                    {
                        gameManager.countMaterial2++;
                        hit.collider.gameObject.SetActive(false);

                    }
                    else if (hit.collider.GetComponent<ObjectToCatch>() != null && hit.collider.GetComponent<ObjectToCatch>().id == gameManager.idMaterial3)
                    {
                        gameManager.countMaterial3++;

                        hit.collider.gameObject.SetActive(false);
                    }

                    gameManager.updateTextsProgress();
                }
            }


            

            if (hit.collider != null && hit.collider.CompareTag("Respawn"))
            {

                if (Input.GetKeyDown(KeyCode.E) && gameManager.phaseState == 3 && gameManager.phaseBuild==0)
                {
                    gameManager.onlyDescriptionMode();
                    gameManager.PhaseMode(4);
                    Debug.Log("Elige donde plantar");

                }
                else if (Input.GetKeyDown(KeyCode.E) && gameManager.phaseState == 3 && gameManager.phaseBuild != 0)
                {
                    Debug.Log("Planta la pieza");
                    gameManager.onlyDescriptionMode();
                    gameManager.PhaseMode(5);
                }
            }
            

            if (gameManager.phaseState == 4 && gameManager.phaseBuild==0)
            {
                if (Input.GetKeyDown(KeyCode.E) && hit.collider.CompareTag("PlantArea"))
                {
                    gameManager.ChooseAreaToSpawn(hit.point);
                    Debug.Log("Cojo punto de referencia");

                }

            }

            if (hit.collider != null && hit.collider.CompareTag("Pipeline") && gameManager.phaseState==5 && gameManager.phaseBuild != 0 && gameManager.phaseBuild != 4)
            {
                
                if (Input.GetKeyDown(KeyCode.E) && gameManager.phaseState == 5)
                {
               

                    Transform child = hit.collider.gameObject.transform;
                    if (child.parent != null)
                    {
                        // Obtenemos el transform del padre
                        Transform parentTransform = child.parent;
                        parentTransform.gameObject.GetComponent<BuildObjectsByParts>().BuildObject();
                        Debug.Log("Añado punto de pieza");

                    }




                    gameManager.resetTextsProgress();
                    gameManager.PhaseMode(6);



                }
               }
            }
        }
    }
}

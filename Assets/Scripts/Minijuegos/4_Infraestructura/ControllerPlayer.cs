using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static energyMinigameManager;

public class ControllerPlayer : MonoBehaviour
{
    private Animator animator;
    private MiniGameManager4 gameManager;
    public string animation;
   
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
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.CompareTag("CatchAble"))
                {
                    gameManager.activeInteract(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ObjectToCatch objectToCatch = hit.collider.GetComponent<ObjectToCatch>();
                        if (objectToCatch != null)
                        {
                            if (objectToCatch.id == gameManager.idMaterial1)
                            {
                                gameManager.countMaterial1++;
                                hit.collider.gameObject.SetActive(false);
                            }
                            else if (objectToCatch.id == gameManager.idMaterial2)
                            {
                                gameManager.countMaterial2++;
                                hit.collider.gameObject.SetActive(false);
                            }
                            else if (objectToCatch.id == gameManager.idMaterial3)
                            {
                                gameManager.countMaterial3++;
                                hit.collider.gameObject.SetActive(false);
                            }

                            gameManager.updateTextsProgress();
                        }
                    }
                }
                else if (hit.collider != null && hit.collider.CompareTag("Respawn"))
                {
                    gameManager.activeInteract(true);
                   

                    if (Input.GetKeyDown(KeyCode.E) && gameManager.phaseState == 3 && gameManager.phaseBuild == 0)
                    {

                        animator = hit.collider.gameObject.GetComponent<Animator>();

                        StartCoroutine(PlayAnimationAfterDelay(animator));
                        gameManager.onlyDescriptionMode();
                        gameManager.PhaseMode(4);
                        Debug.Log("Elige donde plantar");
                    }
                    else if (Input.GetKeyDown(KeyCode.E) && gameManager.phaseState == 3 && gameManager.phaseBuild != 0)
                    {
                        animator = hit.collider.gameObject.GetComponent<Animator>();

                        StartCoroutine(PlayAnimationAfterDelay(animator));
                        Debug.Log("Planta la pieza");
                        gameManager.onlyDescriptionMode();
                        gameManager.PhaseMode(5);
                    }
                }
                else if (gameManager.phaseState == 4 && gameManager.phaseBuild == 0 && hit.collider.CompareTag("PlantArea"))
                {
                    gameManager.activeInteract(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        animator = hit.collider.gameObject.GetComponent<Animator>();

                        StartCoroutine(PlayAnimationAfterDelay(animator));
                        gameManager.ChooseAreaToSpawn(hit.point);
                        Debug.Log("Cojo punto de referencia");
                    }
                }
                else if (hit.collider != null && hit.collider.CompareTag("Pipeline") && gameManager.phaseState == 5 && gameManager.phaseBuild != 0 && gameManager.phaseBuild != 5)
                {
                    gameManager.activeInteract(true);

                    if (Input.GetKeyDown(KeyCode.E) && gameManager.phaseState == 5)
                    {
                        Transform child = hit.collider.gameObject.transform;
                        if (child.parent != null)
                        {
                            Transform parentTransform = child.parent;
                            parentTransform.gameObject.GetComponent<BuildObjectsByParts>().BuildObject();
                            Debug.Log("Añado punto de pieza");
                        }

                        gameManager.resetTextsProgress();
                        gameManager.PhaseMode(6);
                    }
                }
                else
                {
                    // Si el collider no tiene ninguno de los tags especificados, desactiva la interacción
                    gameManager.activeInteract(false);
                }
            }
            else
            {
                // Si no se ha detectado ningún collider, también desactiva la interacción
                gameManager.activeInteract(false);
            }
        }
    }

    IEnumerator PlayAnimationAfterDelay(Animator hitAnimator)
    {

        hitAnimator.SetBool("hit", true);
        hitAnimator.Play("Maquina");
        // Esperar el tiempo especificado antes de reproducir la animación
        yield return new WaitForSeconds(2f);
        hitAnimator.speed=0f;
        animator.Play("Any State");
        // También puedes restaurar el estado de los booleanos o parámetros según sea necesario
        hitAnimator.SetBool("hit", false);

    }

}

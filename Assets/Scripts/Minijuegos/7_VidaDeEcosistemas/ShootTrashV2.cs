using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootTrashV2 : MonoBehaviour
{
    public GameObject manager;
    public bool isPlanting;
    public float plantingTimer;
    public Slider progressBar;
    // Start is called before the first frame update
    void Start()
    {
        manager= GameObject.Find("GameManager");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            Debug.Log("Pium Pium");
        }
        PerformRaycast();
    }

    private void Shoot()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.GetChild(0).transform.forward); //playerCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            if (hit.collider.CompareTag("CatchAble"))
            {
                hit.transform.position = new Vector3(0, 0, 0);
                Debug.Log("Barum");
                manager.GetComponent<MiniGameManager7>().countProgress--;
            }
        }
    }



    void PerformRaycast()
    {
        // Obtener la posición del ratón en la pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Lanzar un rayo desde la cámara hacia la posición del ratón
        Ray ray = Camera.main.ScreenPointToRay(transform.GetChild(0).transform.forward);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red);


    }

    /*

    void StartPlanting()
    {
        isPlanting = true;
        plantingTimer = 0f;
        progressBar.gameObject.SetActive(true);
    }

    void UpdateProgress(float progress)
    {
        progressBar.value = Mathf.Clamp01(progress);
    }

    void UpdatePlantingProcess()
    {
        if (isPlanting)
        {
            plantingTimer += Time.deltaTime;
            UpdateProgress(plantingTimer / plantingTime);

            if (plantingTimer >= plantingTime)
            {
                FinishPlanting();
            }
        }
    }

    void FinishPlanting()
    {
        isPlanting = false;
        progressBar.value = 0f;
        progressBar.gameObject.SetActive(false);
        PlantSeed();
    }


    */
}

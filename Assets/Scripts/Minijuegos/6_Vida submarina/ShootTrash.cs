using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootTrash : MonoBehaviour
{
    public LayerMask Trash;
    public LayerMask PlasticBottle;
    public LayerMask KeyChain;
    public LayerMask WoodMaterialObjects;
    public LayerMask Cork;
    public LayerMask MetalTube;
    public LayerMask TVScreen;
    public LayerMask Cushion;
    public LayerMask Rubber;
    public LayerMask CardBoard;
    public LayerMask Metal;
    public Vector3 relocationPosition;
    public static int points;
    public int totalNeededPoints = 31;
    public Camera playerCamera;

    public GameObject trashFeedbackPanel;
    public TextMeshProUGUI titleFeedbackText;
    public TextMeshProUGUI subtitleFeedbackText;
    public TextMeshProUGUI descriptionFeedbackText;
    public TextMeshProUGUI pointsText;
    [SerializeField] private LineRenderer _beam;
    [SerializeField] private Transform _muzzlePoint;

    // Start is called before the first frame update
    void Start()
    {

        _beam.enabled = false;
        trashFeedbackPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            Debug.Log("Mouse presionado");
        }
        if (Input.GetMouseButtonDown(0))
            Activate();
        else if (Input.GetMouseButtonUp(0))
            Desactivate();

        if (pointsText != null) { pointsText.text = "Points: " + points + " / " + totalNeededPoints; }
       

        beamUpdate();

    }
    private void Activate()
    {
        _beam.enabled = true;
    }

    private void beamUpdate ()
    {
        if (_beam)
        { Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);  
        
        bool cast = Physics.Raycast(ray, out RaycastHit hit, 20);

        // Establece la longitud del rayo
        Vector3 hitPosition = cast ? hit.point : ray.origin + ray.direction * 20;

        _beam.SetPosition(0, _muzzlePoint.position);
        _beam.SetPosition(1, hitPosition);
        }
    }

    // Deactivate the laser beam
    private void Desactivate()
    {
        _beam.enabled = false;
    }

    private void Shoot()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.GetChild(0).transform.forward); //playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.Log("Intentado disparar");
        Debug.DrawLine(transform.position, transform.position + new Vector3(0,0, 1000), Color.yellow);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Trash))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Basura";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tardará demasiado en degradarse";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, PlasticBottle))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Botella de plástico";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tarda de 100 a 1.000 años en degradarse.";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, KeyChain))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Llavero de metal";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tarda de 50 a 100 años en degradarse, aunque también se puede alargar hasta los 500 años si llega a ser de cobre.";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Metal))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Objeto de metal";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tarda de 50 a 100 años en degradarse, aunque también se puede alargar hasta los 500 años si llega a ser de cobre.";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, WoodMaterialObjects))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Objeto de madera";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tarda de 1 a 5 años en degradarse.";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Cork))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Corcho";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tarda más de 20 años en degradarse.";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, MetalTube))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Tubería de metal";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tarda de 50 a 100 años en degradarse.";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, TVScreen))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Pantalla de televisión";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tarda de 1 a 2 millones de años en degradarse.";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Cushion))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Cojín";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tarda de 5 a 80 años en degradarse.";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Rubber))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Botella de plástico";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tarda de 50 a 80 años en degradarse.";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, CardBoard))
        {
            Debug.Log("Rayo sale");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            //relocationPosition += new Vector3(2f, 0, 0);
            hit.transform.gameObject.SetActive(false);
            titleFeedbackText.text = "Caja de cartón";
            subtitleFeedbackText.text = string.Empty;
            descriptionFeedbackText.text = "Tarda de 2 a 5 meses en degradarse.";
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.GetChild(0).transform.rotation.eulerAngles, transform.GetChild(0).transform.forward * Mathf.Infinity, Color.yellow);
    }

    private IEnumerator ShowTrashFeedbackPanel()
    {
        trashFeedbackPanel.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        trashFeedbackPanel.SetActive(false);
    }
}

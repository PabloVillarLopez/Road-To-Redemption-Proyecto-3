using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer _beam; // LineRenderer component used for the laser beam
    [SerializeField] private Transform _muzzlePoint; // Transform representing the point where the laser originates
    [SerializeField] private Camera _mainCamera; // Reference to the main camera
    [SerializeField] private float _maxLength; // Maximum length of the laser beam
    private MiniGameManager7 miniGameManager; // Reference to the MiniGameManager7 script

    private void Awake()
    {
        _beam.enabled = false; // Disable the laser beam initially
        miniGameManager = FindObjectOfType<MiniGameManager7>(); // Find the MiniGameManager7 script in the scene
        _mainCamera = Camera.main; // Assign the main camera if not assigned from the editor
    }

    // Activate the laser beam
    private void Activate()
    {
        _beam.enabled = true;
    }

    // Deactivate the laser beam
    private void Deactivate()
    {
        _beam.enabled = false;
    }

    private void Update()
    {
        // Check if the player can shoot based on the current phase
        if (miniGameManager.currentPhase == 1)
        {
            if (Input.GetMouseButtonDown(0))
                Activate();
            else if (Input.GetMouseButtonUp(0))
                Deactivate();
        }
        else if (miniGameManager.currentPhase == 2)
        {
            // Logic for phase 2
        }
        else if (miniGameManager.currentPhase == 3)
        {
            if (Input.GetMouseButtonDown(0))
                Activate();
            else if (Input.GetMouseButtonUp(0))
                Deactivate();
        }
    }

    private void FixedUpdate()
    {
        // Laser behavior for phase 1
        if (miniGameManager.currentPhase == 1 && _beam.enabled)
        {
            // Realiza el raycast desde la cámara principal
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            bool cast = Physics.Raycast(ray, out RaycastHit hit, _maxLength);

            // Establece la longitud del rayo
            Vector3 hitPosition = cast ? hit.point : ray.origin + ray.direction * _maxLength;

            // Actualiza la posición del rayo láser
            _beam.SetPosition(0, _muzzlePoint.position);
            _beam.SetPosition(1, hitPosition);

            // Si el rayo láser golpea un objeto, verifica si es "CatchAble" y reduce su vida
            if (cast && hit.collider.CompareTag("CatchAble"))
            {
                
                if (hit.collider.gameObject != null)
                {
                    var catchableObject = hit.collider.GetComponent<ObjectToExplote>();
                    if (catchableObject != null)
                    {
                        catchableObject.TakeDamage(0.2f); // Puedes ajustar el daño aquí
                        
                    }
                  
                }
            }

            
        }


        if (miniGameManager.currentPhase == 2 )
        {
            // Logic for phase 2
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            bool cast = Physics.Raycast(ray, out RaycastHit hit, _maxLength);
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);

            // Establece la longitud del rayo
            Vector3 hitPosition = cast ? hit.point : ray.origin + ray.direction * _maxLength;
            if (cast && hit.collider.CompareTag("PlantArea") && miniGameManager.haveSeeds==true)
            {
                print("Se puede plantar");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    print("Se ha plantado");
                    miniGameManager.isPlanting = true;
                    hit.collider.gameObject.SetActive(false);

                }

            }
         

            if (cast && hit.collider.CompareTag("CatchAble"))
            {
                
                    // Incrementar el proceso de la barra de progreso
                    miniGameManager.haveSeeds = true;
                    hit.collider.gameObject.SetActive(false);   
                    print("Se ha recogido la semilla");
                
            }

        }
        






        // Laser behavior for phase 3
        if (miniGameManager.currentPhase == 3 && _beam.enabled)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            bool cast = Physics.Raycast(ray, out RaycastHit hit, _maxLength);

            // Establece la longitud del rayo
            Vector3 hitPosition = cast ? hit.point : ray.origin + ray.direction * _maxLength;

            // Actualiza la posición del rayo láser
            _beam.SetPosition(0, _muzzlePoint.position);
            _beam.SetPosition(1, hitPosition);

            // Si el rayo láser golpea un objeto, verifica si es "CatchAble" y reduce su vida
            if (cast && hit.collider.CompareTag("CatchAble"))
            {
                var catchableObject = hit.collider.GetComponent<ApplyMaterial>();
                if (catchableObject != null)
                {
                    catchableObject.AddHeight(0.1f);
                }
            }
        }
    }
}

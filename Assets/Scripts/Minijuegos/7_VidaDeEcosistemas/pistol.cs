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
    public GameObject spawnSeed;
    public GameObject spawnSeed2;
    public GameObject pauseSpanish;
    public GameObject pauseEnglish;

    private bool shoot;
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

    public void checkPause () 
    {
        if (pauseSpanish.activeInHierarchy || pauseEnglish.activeInHierarchy)
        {
            // El GameObject 'pause' está activo
            Debug.Log("El GameObject 'pause' está activo.");
            shoot = false;

            // Aquí puedes añadir el código que quieras ejecutar si 'pause' está activo.
        }
        else
        {
            // El GameObject 'pause' no está activo
            shoot = true;
            Debug.Log("El GameObject 'pause' no está activo.");

            // Aquí puedes añadir el código que quieras ejecutar si 'pause' no está activo.
        }


    }

    private void Update()
    {
        // Check if the player can shoot based on the current phase
        if (miniGameManager.currentPhase == 1)
        {
            if (Input.GetMouseButtonDown(0) && shoot)
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
        checkPause();
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
                        catchableObject.TakeDamage(0.1f); // Puedes ajustar el daño aquí
                    }
                }
            }
            else
            {
                miniGameManager.activeInteract(false);
            }
        }

        if (miniGameManager.currentPhase == 2)
        {
            // Fase 2: Lógica de rayos
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            bool cast = Physics.Raycast(ray, out RaycastHit hit, _maxLength);
            Debug.DrawRay(ray.origin, ray.direction * (cast ? hit.distance : _maxLength), Color.green);

            // Establece la posición de hit o la longitud máxima del rayo
            Vector3 hitPosition = cast ? hit.point : ray.origin + ray.direction * _maxLength;
            Debug.Log($"Posición de hit: {hitPosition}");

            if (cast && hit.collider != null)
            {
                if (hit.collider.CompareTag("PlantArea") && miniGameManager.haveSeeds == true)
                {
                    // Habilitar la interacción de plantación
                    miniGameManager.activeInteract(true);
                    Debug.Log("Puedes plantar en la 'PlantArea'.");

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Desactivar el GameObject actual
                        hit.collider.gameObject.SetActive(false);
                        Debug.Log("El GameObject en 'PlantArea' ha sido desactivado.");

                        // Obtener la posición donde se hizo el hit
                        Vector3 spawnPosition = hit.point;

                        // Rotación por defecto
                        Quaternion rotacionPorDefecto = Quaternion.Euler(0, 0, 0);

                        // Ajustar la altura de la posición de spawn en -1.5 unidades
                        Vector3 posicionAjustada = spawnPosition;
                        posicionAjustada.y -= 1.5f;
                        Debug.Log($"Posición ajustada para el spawn: {posicionAjustada}");

                        // Verificar el objeto padre
                        if (hit.collider.transform.parent != null)
                        {
                            string nombrePadre = hit.collider.transform.parent.name;
                            Debug.Log($"Nombre del objeto padre: {nombrePadre}");

                            if (nombrePadre == "BOSQUE")
                            {
                                // Rotación específica para el objeto en el BOSQUE
                                Quaternion rotacionBosque = Quaternion.Euler(0, 0, 0);
                                GameObject objetoSpawn = Instantiate(spawnSeed2, posicionAjustada, rotacionBosque);
                                Debug.Log("Objeto instanciado en el BOSQUE.");
                            }
                            else if (nombrePadre == "LAGO")
                            {
                                // Usar la rotación por defecto para el objeto en el LAGO
                                GameObject objetoSpawn = Instantiate(spawnSeed, posicionAjustada, rotacionPorDefecto);
                                Debug.Log("Objeto instanciado en el LAGO.");
                            }
                            else
                            {
                                Debug.Log("El objeto padre no es ni 'BOSQUE' ni 'LAGO'.");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("El collider no tiene un objeto padre.");
                        }

                        // Confirmación de spawn
                        Debug.Log("Se ha spawneado un nuevo GameObject en la posición del hit.");

                        // Desactivar la lógica de plantación
                        miniGameManager.activeInteract(false);
                        miniGameManager.isPlanting = true;
                        Debug.Log("Interacción de plantar desactivada. Estado de plantación activado.");
                    }
                }
                else if (hit.collider.CompareTag("CatchAble"))
                {
                    // Habilitar la interacción de recolección
                    miniGameManager.activeInteract(true);
                    Debug.Log("Puedes recoger el objeto 'CatchAble'.");

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        miniGameManager.haveSeeds = true;
                        hit.collider.gameObject.SetActive(false);
                        Debug.Log("Se ha recogido la semilla y el objeto 'CatchAble' ha sido desactivado.");
                    }
                }
                else
                {
                    // Desactivar la interacción
                    miniGameManager.activeInteract(false);
                    Debug.Log("El collider no tiene el tag 'PlantArea' o 'CatchAble'.");
                }
            }
            else
            {
                // Raycast no golpeó nada o no hay collider válido
                miniGameManager.activeInteract(false);
                Debug.Log("El Raycast no golpeó nada o no se detectó un collider.");
            }

            // Estado de semillas
            Debug.Log($"Estado de haveSeeds: {miniGameManager.haveSeeds}");
        }

        // Laser behavior for phase 3
        if (miniGameManager.currentPhase == 3)
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
                    catchableObject.AddHeight(0.05f);
                }
            }
            else
            {
                miniGameManager.activeInteract(false);
            }
        }
    }




}

   



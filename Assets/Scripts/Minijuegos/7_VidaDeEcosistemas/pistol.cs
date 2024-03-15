using System.Diagnostics;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer _beam;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _maxLength;
    private MiniGameManager7 miniGameManager;
    

    private void Awake()
    {
        _beam.enabled = false;
        miniGameManager = FindObjectOfType<MiniGameManager7>(); // Encuentra el MiniGameManager7 en la escena
        _mainCamera = Camera.main; // Asigna la cámara principal si no se asigna desde el editor
    }

    private void Activate()
    {
        _beam.enabled = true;
    }

    private void Deactivate()
    {
        _beam.enabled = false;
    }

    private void Update()
    {
        // Verifica si el jugador puede disparar según la fase actual
        if (miniGameManager.currentPhase == 1)
        {
            if (Input.GetMouseButtonDown(0))
                Activate();
            else if (Input.GetMouseButtonUp(0))
                Deactivate();
        }
        else if (miniGameManager.currentPhase == 2)
        {
            

            if (Input.GetMouseButton(0))
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo))
                {
                    if (hitInfo.collider.CompareTag("PlantArea") && miniGameManager.IsVectorInsideZones(hitInfo.point))
                    {
                        miniGameManager.isPlanting=true;
                    }
                }
            }
            else
            {
                miniGameManager.isPlanting = false;
            }

        }
    }

    private void FixedUpdate()
    {
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
                var catchableObject = hit.collider.GetComponent<ObjectToExplote>();
                if (catchableObject != null)
                {
                    catchableObject.TakeDamage(0.2f); // Puedes ajustar el daño aquí
                }
            }
        }
    }

    
}
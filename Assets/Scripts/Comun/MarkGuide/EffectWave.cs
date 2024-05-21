using System.Collections.Generic;
using UnityEngine;

public class EffectWave : MonoBehaviour
{
    public float speed = 1.0f;          // Velocidad de la onda
    public float magnitude = 0.1f;      // Magnitud de la onda
    public float disappearDuration = 5f; // Duraci�n antes de desaparecer

    private Vector3 originalPosition;    // Posici�n original del objeto
    private Transform playerTransform;   // Referencia al transform del jugador

    void Start()
    {
        // Guardar la posici�n original del objeto
        originalPosition = transform.position;

        // Encontrar el transform del jugador
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Iniciar la corutina para desaparecer despu�s de cierto tiempo
        StartCoroutine(DisappearAfterDelay());
    }

    void Update()
    {
        // Calcular la posici�n en el eje Y con el efecto de onda
        float newY = originalPosition.y + Mathf.Sin(Time.time * speed) * magnitude;
        // Actualizar la posici�n del objeto
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Girar para mirar al jugador
        transform.LookAt(playerTransform);
    }

    System.Collections.IEnumerator DisappearAfterDelay()
    {
        // Esperar cierto tiempo antes de desaparecer
        yield return new WaitForSeconds(disappearDuration);

        // Gradualmente escalar el objeto hasta llegar a 0 en todos los ejes
        while (transform.localScale.x > 0 && transform.localScale.y > 0 && transform.localScale.z > 0)
        {
            float scaleStep = Time.deltaTime * 1f; // Velocidad de desaparici�n
            transform.localScale -= new Vector3(scaleStep, scaleStep, scaleStep);
            yield return null;
        }

        // Destruir el objeto despu�s de desaparecer
        Destroy(gameObject);
    }
}
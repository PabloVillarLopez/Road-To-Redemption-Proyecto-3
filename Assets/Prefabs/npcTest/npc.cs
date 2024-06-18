using UnityEngine;

public class npc : MonoBehaviour
{
    // Variables para controlar la animaci�n de flotaci�n
    public float floatAmplitude = 0.5f; // Amplitud de la flotaci�n
    public float floatFrequency = 1f;   // Frecuencia de la flotaci�n
    private Vector3 initialPosition;    // Posici�n inicial del NPC

    void Start()
    {
        // Guardar la posici�n inicial del NPC
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calcular la nueva posici�n vertical basada en el seno del tiempo transcurrido
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Actualizar la posici�n del NPC
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
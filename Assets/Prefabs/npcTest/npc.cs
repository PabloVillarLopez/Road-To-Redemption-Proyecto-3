using UnityEngine;

public class npc : MonoBehaviour
{
    // Variables para controlar la animación de flotación
    public float floatAmplitude = 0.5f; // Amplitud de la flotación
    public float floatFrequency = 1f;   // Frecuencia de la flotación
    private Vector3 initialPosition;    // Posición inicial del NPC

    void Start()
    {
        // Guardar la posición inicial del NPC
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calcular la nueva posición vertical basada en el seno del tiempo transcurrido
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Actualizar la posición del NPC
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
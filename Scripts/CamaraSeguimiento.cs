using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    public Transform jugador; // Referencia al transform del jugador
    public float suavizado = 5f; // Par�metro de suavizado
    public Vector3 desplazamiento = new Vector3(0f, 2f, -5f); // Desplazamiento adicional

    void Update()
    {
        if (jugador != null) // Aseg�rate de que haya un jugador asignado
        {
            // Obt�n la posici�n actual del jugador
            Vector3 posicionJugador = jugador.position;

            // Ajusta la posici�n de la c�mara solo en el eje Z (horizontal) con suavizado y desplazamiento
            Vector3 nuevaPosicion = posicionJugador + desplazamiento;

            // Aplica suavizado utilizando Lerp
            transform.position = Vector3.Lerp(transform.position, nuevaPosicion, suavizado * Time.deltaTime);
        }
    }
}

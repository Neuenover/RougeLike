using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tirar_Al_Aro : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool presionado = false;
    
    
    public void OnPointerDown(PointerEventData eventData) // detectar si se presiona el boton
    {
        presionado = true;
       
    }

    public void OnPointerUp(PointerEventData eventData) // detectar si se solto
    {
        presionado = false;
    }

    void Update()
    {
        if (presionado)
        {
            // Realiza las acciones que deseas mientras se mantiene presionado el botón de UI.
            Debug.Log("Botón de UI presionado");
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spawnear_Pelota : MonoBehaviour, IPointerDownHandler
{
    public GameObject inicioBall;

    public GameObject pelotaPrefab;

    float fuerza = 10;


    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject pelotaTemporal = Instantiate(pelotaPrefab, inicioBall.transform.position, pelotaPrefab.transform.rotation) as GameObject;

        Rigidbody rb = pelotaTemporal.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * fuerza);
    }
}

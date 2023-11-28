using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompañeroBase : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform ball;
    [SerializeField] GameObject compañeroIA;
    [SerializeField] Transform posDribblingIA;
    [SerializeField] float tiempoPasar;
    public float fuerzaDePase = 30f;
    bool tieneLaPelota = false;
    //reinicio de nivel
    UI_manager audioSource;


    private void Start()
    {
        audioSource = FindObjectOfType<UI_manager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (tieneLaPelota == true)
        {
            ball.position = posDribblingIA.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
            StartCoroutine(pasarLaPelota());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            tieneLaPelota = true;
            audioSource.enabled = true;
        }
    }

    IEnumerator pasarLaPelota() //corutina para hacer el pase 
    {
        Debug.Log("Pasar al jugador");
       
        
        yield return new WaitForSeconds(1f);
        audioSource.enabled = false;
        Vector3 posJugador = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z); 
        GetComponent<Transform>().rotation = Quaternion.LookRotation(posJugador);
        tieneLaPelota = false;
        ball.transform.position = Vector3.MoveTowards(ball.transform.position, posJugador, fuerzaDePase * Time.deltaTime);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pase : MonoBehaviour, IPointerDownHandler
{
    public _Joystick.Scripts.CharacterGameManager characterGameManager;
    [SerializeField]  float fuerza = 100;
    [SerializeField] public Rigidbody rb;
    public Transform ball;
  

    [SerializeField] Vector3 puntoDeDestino;
    public float T2 = 0;
    public Transform pospass;
    private Tirar_Al_Aro tiro;
    private void Start()
    {
        characterGameManager = FindObjectOfType<_Joystick.Scripts.CharacterGameManager>();
        tiro = FindObjectOfType<Tirar_Al_Aro>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(cambiarBool());
        //ImpulsarHaciaPuntoEspecifico();
        //SePreciono();


    }

    public void ImpulsarHaciaAdelante()
    {
        characterGameManager.picando = false;
        characterGameManager.inBallHands = false;
        // Calcula la dirección hacia adelante del objeto en su espacio local.
        Vector3 direccionAdelante = transform.forward;
        rb.isKinematic = false;
        // Aplica una fuerza en la dirección hacia adelante multiplicada por la fuerza.
        rb.AddForce(direccionAdelante * fuerza, ForceMode.Acceleration);
    }

    /*
    public void ImpulsarHaciaPuntoEspecifico()
    {
       
        rb.isKinematic = false;
        T2 += Time.deltaTime;
        float duration = 0.5f;
        float t01 = T2 / duration;

        Vector3 A2 = pospass.position;
        Vector3 B2= compañero.position;
        Vector3 pos2 = Vector3.Lerp(A2, B2, t01);

        Vector3 arc2 = Vector3.forward * 10 * Mathf.Sin(t01 * 5.14f);
        ball.position = pos2 + arc2;

        if (t01 >= 1)
        {
            
            ball.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    
    */
    /*
    public void SePreciono()
    {
        
        
        Vector3 posAdelante = new Vector3(compañero.transform.position.x, compañero.transform.position.y, compañero.transform.position.z );
        characterGameManager.controller.transform.rotation = Quaternion.LookRotation(posAdelante);
        Debug.Log("Tuki");
        ball.transform.position = Vector3.MoveTowards(ball.transform.position, posAdelante, fuerza * Time.deltaTime);
    }
    */
    IEnumerator cambiarBool()
    {

        characterGameManager.isBallFlying = false;
        characterGameManager.inBallHands = false;
        characterGameManager.pasar = true;
        yield return new WaitForSeconds(1f);
        characterGameManager.pasar = false;
    }
}
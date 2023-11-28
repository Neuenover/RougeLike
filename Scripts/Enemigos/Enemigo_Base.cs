using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo_Base : MonoBehaviour
{
    //var  detectar player
    public float alertRank;
    //var  robar pelota
    public float stealRank;
    //detectar pelota
    public LayerMask capeBall;
    
    public Transform Ball;
    //zona donde tira el enemigo
    public Transform zonaTiro;

    [SerializeField] GameObject ballGame;

    [SerializeField] float speed = 5;
    //pos de pique
    [SerializeField] Transform posPelotaEnemigo;

    [SerializeField] float tiempoRobarPelota = 1;
    //refrencia al jugador
    public _Joystick.Scripts.CharacterGameManager characterGameManager;

    public Transform posOverHead;
    public Transform target;
    private float T = 0;
    //detectar jugador bools
    bool isAlert;
    bool steal;
    public bool ballInHand = false;

    bool pelotarobada = false;
    [SerializeField] public bool tirarAlAro = false;

    Animator animEnemigo;

    //reinicio de nivel
    UI_manager uiNiveles;
    void Start()
    {
        characterGameManager = FindObjectOfType<_Joystick.Scripts.CharacterGameManager>(); //encontrar jugador
        uiNiveles = FindObjectOfType<UI_manager>();
        animEnemigo = GetComponentInChildren<Animator>();
    }

   
    void Update()
    {
        if (isAlert == true)
        {
            animEnemigo.SetBool("Run", true);
        }
        else
        {
            animEnemigo.SetBool("Run", false);
        }

        if (ballInHand == false)
        {
            isAlert = Physics.CheckSphere(transform.position, alertRank, capeBall); // crear zonas deteccion
            steal = Physics.CheckSphere(transform.position, stealRank, capeBall); // zona de robo
            if (isAlert == true && steal == false) // avanzar al jugador
            {
                //transform.LookAt(Ball);
                Vector3 posJugador = new Vector3(Ball.position.x, transform.position.y, Ball.position.z);
                transform.LookAt(posJugador);
                transform.position = Vector3.MoveTowards(transform.position, posJugador, speed * Time.deltaTime);
                
            }
            if (steal == true) 
            {
                isAlert = false;
                StartCoroutine(robarPelota());
            }
            if (tirarAlAro == true) // apuntar
            {
                //animEnemigo.SetTrigger("Tiro");
                stealRank = 0;
                alertRank = 0;
                T += Time.deltaTime;
                float duration = 0.5f;
                float t01 = T / duration;

                Vector3 A = posOverHead.position;
                Vector3 B = target.position;
                Vector3 pos = Vector3.Lerp(A, B, t01);

                Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);
                Ball.position = pos + arc;
                if (t01 >= 1)
                {
                    tirarAlAro = false;
                    Ball.GetComponent<Rigidbody>().isKinematic = false;
                    StartCoroutine(uiNiveles.ReiniciarNivel());
                }
            }
        }
        else if (ballInHand == true && tirarAlAro == false) // picar
        {
            Ball.position = posPelotaEnemigo.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 8));
            Vector3 posTiro = new Vector3(zonaTiro.position.x, transform.position.y, zonaTiro.position.z);
            transform.LookAt(posTiro);
            transform.position = Vector3.MoveTowards(transform.position, posTiro, speed * Time.deltaTime);
            animEnemigo.SetBool("Run", true);
            if (transform.position.x == zonaTiro.transform.position.x && transform.position.z == zonaTiro.transform.position.z)
            {
                animEnemigo.SetBool("Run", false);
                Debug.Log("llego a la zona");
                ballInHand = false;
                tirarAlAro = true;

            }
        }

       



    }

    private void OnDrawGizmos() //dibujar las zonas
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRank);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stealRank);
    }

    IEnumerator robarPelota() // tiempo para robar
    {
        Debug.Log("entro en el area de robo");
        
        yield return new WaitForSeconds(tiempoRobarPelota);
        if (steal == true)
        {
            characterGameManager.inBallHands = false;
            characterGameManager.picando = false;
            ballInHand = true;

            Debug.Log("Robo la pelota");
        }
      
    }

    IEnumerator TirarAro()
    {
  
        yield return new WaitForSeconds(0.5f);
       
    }

    
    

}

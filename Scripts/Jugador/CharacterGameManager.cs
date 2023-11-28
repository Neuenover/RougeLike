using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace _Joystick.Scripts // referncia al assest de joystick
{
    public class CharacterGameManager : MonoBehaviour
    {
        //varaible del tactil
        public VariableJoystick joystic;
        
        public Canvas inputCanvas;

        public Tirar_Al_Aro tirarAlAro;

        // variable enemigo
        [SerializeField] Enemigo_Base enemigo;

        [SerializeField] bool isJoystick;
        //variables personaje
        public CharacterController controller;
        //movilidad
        [SerializeField] float movmmentSpeed;
        [SerializeField] float rotationSpeed;

        // botones
        [SerializeField] Button btnRobar;
        [SerializeField] Button btnTiro;

        [SerializeField] Animator anim;

        //variables pelota
        public bool picando = true;
        public Pase pase; //referncia script pase
        public Transform ball; //pelota
        public Transform posOverHead; 
        public Transform arms;
        public Transform posDribble;
        public Transform[] target;
        [SerializeField] float areaRobo;
        [SerializeField] bool puedeRobar;
        [SerializeField] LayerMask capeBall;
        public float fuerza = 10.0f; // La fuerza con la que se impulsará el objeto.
        [SerializeField] float fuerzaPase = 30.0f;
        //pase
        public Transform compañero;
        public Transform posPaseAdelnate;
        [SerializeField] private Rigidbody rb;

        private bool isPressed = false;
        public bool pasar;
        [SerializeField]public bool inBallHands = true;
        
        public Rect zonaDeteccion;
        public bool isBallFlying = false;
        private float T = 0;
        [SerializeField] float potenciaTiro = 0;
        [SerializeField] Image barraCarga;
        [SerializeField] float cargaActual;
        [SerializeField] float cargaMaxima;
        [SerializeField] GameObject barras;

        //Audio y reinicio de nivel
        UI_manager uiAudios;

        [SerializeField] AudioSource score;
        private void Start()
        {
            EnableJoystickInput(); //habilitar movimiento joystick
            zonaDeteccion = new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.5f);
            tirarAlAro = FindObjectOfType<Tirar_Al_Aro>(); // llamar script para tirar al aro
            pase = FindObjectOfType<Pase>();// llamar script para pasar pelota
            enemigo = FindObjectOfType<Enemigo_Base>();
            uiAudios = FindObjectOfType<UI_manager>();
            btnTiro.interactable = false;
            anim = GetComponentInChildren<Animator>();
        }
        
        public void EnableJoystickInput()
        {
            isJoystick = true;
            inputCanvas.gameObject.SetActive(true);
        }

        private void Update()
        {

            puedeRobar = Physics.CheckSphere(transform.position, areaRobo, capeBall);

            if (puedeRobar)
            {
                Debug.Log("Puede Robar la pelota al enemigo");
                btnRobar.interactable = true;
            }
            else
            {
                btnRobar.interactable = false;
            }

            if (tirarAlAro.presionado == true) // cargar barra de tiro
            {
                potenciaTiro += 2f * Time.deltaTime;
                barras.SetActive(true);
                barraCarga.fillAmount = potenciaTiro / cargaMaxima;
            }
            
            if (isPressed)
            {
                // Realiza las acciones que deseas mientras se mantiene presionado el botón de UI.
                Debug.Log("Botón de UI presionado");
            }

            if (inBallHands) 
            {
                if (tirarAlAro.presionado == true && picando == true ) // apuntar al aro
                {
                    Vector3 posAro = new Vector3(target[0].parent.position.x, transform.position.y, target[0].parent.position.z);
                    ball.position = posOverHead.position;
                    controller.enabled = false;
                    transform.LookAt(posAro);
                    picando = false;
                    anim.SetTrigger("TiroAro");
                }
                else if (picando == true && tirarAlAro.presionado == false )// picar pelota
                {
                    ball.position = posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
             
                }
                else if (tirarAlAro.presionado == false && picando == false) // tirar la pelota
                {
                    controller.enabled = true;
                    inBallHands = false;
                    isBallFlying = true;
                    T = 0;
                    
                }
                /*
                else if (pase.pasar == true &&  picando == false)
                {
                    pase.SePreciono();
                }
                */
            }
            
           
            
            if (isBallFlying)
            {
                if (potenciaTiro < 3) // fallar tiro
                {
                    barras.SetActive(false);
                    Debug.Log("Fallo Der");
                    
                    T += Time.deltaTime;
                    float duration = 0.5f;
                    float t01 = T / duration;

                    Vector3 A = posOverHead.position;
                    Vector3 B = target[1].position;
                    Vector3 pos = Vector3.Lerp(A, B, t01);

                    Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);
                    ball.position = pos + arc;

                    if (t01 >= 1)
                    {
                        isBallFlying = false;
                        ball.GetComponent<Rigidbody>().isKinematic = false;
                        barras.SetActive(false);
                        potenciaTiro = 0;
                    }
                    
                }
                else if (potenciaTiro >= 3 && potenciaTiro < 5 ) // anotar punto
                {
                    Debug.Log("Punto");
                    
                    T += Time.deltaTime;
                    float duration = 0.5f;
                    float t01 = T / duration;

                    Vector3 A = posOverHead.position;
                    Vector3 B = target[0].position;
                    Vector3 pos = Vector3.Lerp(A, B, t01);

                    Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);
                    ball.position = pos + arc;

                    if (t01 >= 1)
                    {
                        isBallFlying = false;
                        ball.GetComponent<Rigidbody>().isKinematic = false;
                        barras.SetActive(false);
                        score.Play();
                        potenciaTiro = 0;
                        StartCoroutine(uiAudios.ReiniciarNivel());
                    }
                    
                }
                else if (potenciaTiro >= 5) // fallar x2
                {
                    Debug.Log("Fallo Izq");
                    
                    T += Time.deltaTime;
                    float duration = 0.5f;
                    float t01 = T / duration;

                    Vector3 A = posOverHead.position;
                    Vector3 B = target[2].position;
                    Vector3 pos = Vector3.Lerp(A, B, t01);

                    Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);
                    ball.position = pos + arc;

                    if (t01 >= 1)
                    {
                        isBallFlying = false;
                        ball.GetComponent<Rigidbody>().isKinematic = false;
                        barras.SetActive(false);
                        potenciaTiro = 0;
                    }
                    
                }
                else if (potenciaTiro >= 6) // fallar x3
                {
                    T += Time.deltaTime;
                    float duration = 0.5f;
                    float t01 = T / duration;

                    Vector3 A = posOverHead.position;
                    Vector3 B = target[3].position;
                    Vector3 pos = Vector3.Lerp(A, B, t01);

                    Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);
                    ball.position = pos + arc;

                    if (t01 >= 1)
                    {
                        isBallFlying = false;
                        ball.GetComponent<Rigidbody>().isKinematic = false;
                        barras.SetActive(false);
                        potenciaTiro = 0;
                    }
                }

            }
            else if (pasar == true && inBallHands == false && isBallFlying == false) // pasar la pelota al compañero
            {
                Debug.Log("aplicar Fuerza");
               
                Vector3 posAdelante = new Vector3(compañero.transform.position.x, transform.position.y, compañero.transform.position.z);
                controller.transform.rotation = Quaternion.LookRotation(posAdelante);
                Debug.Log("Tuki");
                ball.transform.position = Vector3.MoveTowards(ball.transform.position, posAdelante, fuerzaPase * Time.deltaTime);
                //anim.SetTrigger("Pase");
            }
        }
        private void FixedUpdate()
        {
            if (isJoystick) // establecer movimiento
            {
                var movmentDirection = new Vector3(joystic.Direction.x, 0.0f, joystic.Direction.y);
                controller.SimpleMove(movmentDirection * movmmentSpeed);

                if (movmentDirection.sqrMagnitude <= 0)
                {
                    anim.SetBool("Running", false);
                    return;
                }
                anim.SetBool("Running", true);
                var targetDirection = Vector3.RotateTowards(controller.transform.forward, movmentDirection,
                    rotationSpeed * Time.deltaTime, 0.0f);

                controller.transform.rotation = Quaternion.LookRotation(targetDirection);
                ball.transform.rotation = controller.transform.rotation;


            }

        }

        private void OnTriggerEnter(Collider other) // agarrar la pelota 
        {
            if (!isBallFlying && !inBallHands && other.gameObject.CompareTag("Ball"))
            {
                picando = true;
                inBallHands = true;
                ball.GetComponent<Rigidbody>().isKinematic = true;
                Debug.Log("contacto");
                uiAudios.audioSonidos.enabled = true;
            }

            if (other.gameObject.CompareTag("ZonaTiro"))
            {
                btnTiro.interactable = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, areaRobo);
        }

        public void RobarPelota()
        {
            enemigo.enabled = false;
            picando = true;
            enemigo.ballInHand = false;
            enemigo.stealRank = 0;
            isBallFlying = false;
            inBallHands = true;

            StartCoroutine(reactivarEnemigo());
        }

        IEnumerator reactivarEnemigo()
        {
            
            yield return new WaitForSeconds(1f);
            enemigo.stealRank = 3;
            enemigo.enabled = true;
        }

        
    }

        
    
}



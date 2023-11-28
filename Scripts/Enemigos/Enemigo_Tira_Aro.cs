using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo_Tira_Aro : MonoBehaviour
{
    [SerializeField] bool ballInHand;
    [SerializeField] bool tirarAlAro;
    [SerializeField] bool isBallFlying;
    [SerializeField] bool picando;
    [SerializeField] Transform zonaTiro;

    [SerializeField] Transform posOverhead;
    [SerializeField] Transform posDribbling;
    [SerializeField] Transform target;
    [SerializeField] Transform ball;
    private float T = 0;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ballInHand)
        {
            if (transform.position != zonaTiro.position)
            {

            }
            if (tirarAlAro == false)
            {
                ball.position = posDribbling.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 8));
            }
            else if( tirarAlAro == true)
            {

                ball.position = posOverhead.position;
                transform.LookAt(target.position);
               
            }
           
        }
    }


    IEnumerator esperarTiro()
    {

        yield return new WaitForSeconds(1f);
        ballInHand = false;
        isBallFlying = true;
        T = 0;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class NPC3 : MonoBehaviour
{
    // Start is called before the first frame update
    private static bool esperando=false,recibiendo = false;
    GameObject ball;
    static System.Random r = new System.Random();
    Rigidbody rbBall;
    public NavMeshAgent miAgente;
    Vector3 izquierda,centro,derecha,velocidadIincial, dir, impactoEnSuelo;
    private static Vector3 posicion;
    //float alturaSuelo;
    void Start()
    {
      //alturaSuelo = 0.7326368f;
        ball = GameObject.FindWithTag("Ball");
        izquierda = new Vector3(0.7f,1.17f,-2);
        centro = new Vector3(0.7f,1.17f,0f);
        derecha = new Vector3(0.7f,1.17f,2);
        rbBall = ball.GetComponent<Rigidbody>();
        miAgente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(esperando){
            switch(NPC2.getPosicion()){
                case 0:
                    posicion = derecha;    
                break;
                case 1:
                    int rInt = r.Next(0, 100);
                    if(rInt<50) posicion = izquierda;
                    else posicion = derecha;
                break;
                default:
                    posicion = izquierda;
                break;
            }
            miAgente.SetDestination(posicion);
        }
        /*if(recibiendo){
            //Obtenemos la velocidad inicial de la pelota en el momento en el que el NPC1 la lanza
            velocidadIincial = Auxiliar.getVelocidad();
            Vector3 posInicialPelota=ball.transform.position;
            //a=0.5 por aceleracion de la gravedad b = velocidad en eje y de la pelota y c = y0 - [altura "0" del campo + altura NPC2]  
            float t = (float) ecuacionDeSegundoGrado(0.5*-9.8, velocidadIincial.y, posInicialPelota.y-1.5);
            //Con el tiempo calculo la posicion donde caera: impactoEnSuelo
            impactoEnSuelo = new Vector3(posInicialPelota.x + velocidadIincial.x * t, alturaSuelo,
            posInicialPelota.z + velocidadIincial.z * t);
            print("HELLO");
            miAgente.SetDestination(impactoEnSuelo);
            recibiendo=false;
        }*/
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Ball"){
            if(posicion == izquierda) rbBall.velocity = new Vector3(0f,6f,3f);
            else rbBall.velocity = new Vector3(0f,6f,-3f);
            NPC2.setAtacando(true);
        }
        
    }

    public static void setRecibiendo(bool valor){
        recibiendo = valor;
    }
    public static void setEsperando(bool valor){
        esperando = valor;
    }

    public static Vector3 getPosicion(){
        return posicion;
    }
    private double ecuacionDeSegundoGrado(double a, double b, double c){
        double raizPositiva = (-b+(Math.Sqrt(Math.Pow(b,2)-4*a*c)))/(2*a);
		double raizNegativa = (-b-(Math.Sqrt(Math.Pow(b,2)-4*a*c)))/(2*a);

		if(raizNegativa.ToString() == "NaN" || raizPositiva.ToString() == "NaN") {
			print("no real roots");
            return -1.0;
		}else if(raizPositiva != raizNegativa) {
			//print("dos raiz 1: "+ raizPositiva + " 2: " + raizNegativa);
            if(raizPositiva>=0)return raizPositiva;
            else return raizNegativa;
		}else {
			print("1 raiz doble");
            return raizPositiva;
		}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class NPC2 : MonoBehaviour
{
    GameObject ball,player3;
    Vector3 velocidadIincial, dir, impactoEnSuelo,centroPista,izquierda,centro,derecha;
    private static bool recibiendo = false,atacando=true,preparandose=false;
    float alturaSuelo;
    private static int posicion = 1 ;//0 -> izquierda, 1 -> centro y 2 -> derecha
    Rigidbody rb,rbBall;
    public NavMeshAgent miAgente;

    // Start is called before the first frame update
    void Start()
    {
        alturaSuelo = 0.7326368f;
        ball = GameObject.FindWithTag("Ball");
        player3 = GameObject.FindWithTag("Player3");
        dir = new Vector3(0,0,0);
        izquierda = new Vector3(0.7f,1.17f,-2);
        centro = new Vector3(0.7f,1.17f,0f);
        derecha = new Vector3(0.7f,1.17f,2);
        impactoEnSuelo = new Vector3(0,0,0);
        centroPista = new Vector3(2,alturaSuelo,0);
        rb = GetComponent<Rigidbody>();
        miAgente = GetComponent<NavMeshAgent>();
        rbBall = ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //double y0;
        Vector3 posInicialPelota;
        if(recibiendo){
            //Obtenemos la velocidad inicial de la pelota en el momento en el que el NPC1 la lanza
            velocidadIincial = Auxiliar.getVelocidad();
            posInicialPelota=ball.transform.position;
            //a=0.5 por aceleracion de la gravedad b = velocidad en eje y de la pelota y c = y0 - [altura "0" del campo + altura NPC2]  
            float t = (float) ecuacionDeSegundoGrado(0.5*-9.8, velocidadIincial.y, posInicialPelota.y-1.5);
            //Con el tiempo calculo la posicion donde caera: impactoEnSuelo
            impactoEnSuelo = new Vector3(posInicialPelota.x + velocidadIincial.x * t, alturaSuelo,
            posInicialPelota.z + velocidadIincial.z * t);
            miAgente.SetDestination(impactoEnSuelo);
            recibiendo=false;
            posicion = calculaPosicion(impactoEnSuelo.z);
            print("POSICION VALE: " + posicion);
            NPC3.setEsperando(true);
        }

        if(preparandose){
            miAgente.SetDestination(new Vector3(2f,transform.position.y,transform.position.z));
            preparandose=false;
        }
    }

    void OnCollisionEnter(Collision collision){
        Vector3 dirPase = NPC3.getPosicion() - transform.position;
            Auxiliar.setVelocidad(new Vector3(dirPase.x,5f,dirPase.z));
            rbBall.velocity = Auxiliar.getVelocidad();
            NPC3.setRecibiendo(true);
            preparandose=true;
    }

    public static void setRecibiendo(bool valor){
        recibiendo = valor;
    }

    public static void setAtacando(bool valor){
        atacando = valor;
    }

    public static int getPosicion(){
        return posicion;
    }
    //Devuelve el entero que representa la parte del campo donde se encuentra el jugador en ese momento
    private int calculaPosicion(float z){
        if(z < 0){
            return 0;
        }else if(z == 0) return 1;
        else return 2;
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

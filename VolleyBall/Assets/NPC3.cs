using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class NPC3 : MonoBehaviour
{
    //Variables de estados
    private static bool esperando=false,recibiendo = false,preparandoPase=false;
    GameObject ball;
    //Variable aleatoria para las probabilidades
    static System.Random r = new System.Random();
    Rigidbody rbBall;
    public NavMeshAgent miAgente;
    Vector3 izquierda,centro,derecha,velocidadIincial, dir, impactoEnSuelo;
    private static Vector3 posicion= new Vector3(0f,0f,0f);
    //cte de tiempo que representa en segundos lo que tarda el NPC2 en avanzar a la red y saltar hasta tener una det altura desde que
    //NPC3 alcanza una det altura
    const float CTE_T_REMATE = 0.738f; //Calculada empiricamente con contadores: (CTE_T_REMATE = t1- t2), es el tiempo [t1] que tarda
    // NPC2 desde que le pasa a NPC3 y comienza a correr hacia a la red en linea recta y salta hasta que consigue una determinada 
    //altura donde rematará, menos el tiempo [t2] que tarda la pelota en llegar desde NPC2 hasta NPC3
    float z0,y0=1.9974f,v0z,v0y,z,y=4.4f; //Parametros necesarios para el calculo de la ecuacion del MRUA, algunos estan igualados 
    //por comodidad ya que son conocidos, y0 es la altura del jugador que pasa mientras que y es la altura final donde va a estar el NPC2 en el aire
    //siempre es la misma

    // Start is called before the first frame update
    void Start()
    {
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
        //En el estado 'esperando' el NPC3 espera a que el NPC2 se coloque para recibir y le envie la pelota. En este metodo obtengo
        //La posicion de la cancha: izquierda, centro o derecha a la que se esta moviendo el NPC2 y el NPC3 comienza a moverse
        // en lado contrario para hacer el juego un poco mas visual y los pases mas largos
        //Activa preparandoPase
        if(esperando){
            switch(NPC2.getPosicion()){
                case 0:
                    posicion = derecha;    
                break;
                case 1:
                    int rInt = r.Next(0, 100);
                    if(rInt<50)posicion = izquierda;
                    else posicion = derecha;
                break;
                default:
                    posicion = izquierda;
                break;
            }
            miAgente.SetDestination(posicion);
            esperando=false;
            preparandoPase = true;
        }
        //En 'preparandoPase' comienza la inteligencia de NPC3, en este metodo obtengo los parametros para las ecuaciones del MRUA que
        // aplicaré sobre el eje 'z' y en el 'y' para obtener la velocidad inicial con la que tengo que mandar la pelota para que le caiga a 
        // NPC2 y remate. Esto es posible porque el NPC2 siempre camina hacia delante y salta entonces siempre tarda el 
        // mismo tiempo
        if(preparandoPase){
            z0 = posicion.z;
            z = NPC2.getZAtaque();
            v0y = despejarVectorVelocidad(y,y0); // Tiro parabolico y = y0 + v0y * t + 0.5 * a * t^2
            v0z = (z-z0) / CTE_T_REMATE; // cte: z = z0 + v0z*t
            preparandoPase=false;
        }
    }
    //Simplemente cuando me de la pelota le doy la velocidad calculada
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Ball"){
            Ball.setVelocidad(new Vector3(0f,v0y,v0z));
            rbBall.velocity = Ball.getVelocidad();
        }  
    }

    //Getters y setters

    public static void setRecibiendo(bool valor){
        recibiendo = valor;
    }
    public static void setEsperando(bool valor){
        esperando = valor;
    }

    public static Vector3 getPosicion(){
        return posicion;
    }

    //Ecuacion de MRUA para despejar la velocidad inicial necesaria con la que lanzar la bola para que llegue a NPC2
    private float despejarVectorVelocidad(float posFin, float posIni){ 
        return (float)(posFin - posIni - (0.5f * (-9.8f) * Math.Pow(CTE_T_REMATE,2)))/CTE_T_REMATE;
    }
}

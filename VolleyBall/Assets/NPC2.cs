using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class NPC2 : MonoBehaviour
{
    GameObject ball,player3;
    Vector3 velocidadIincial, dir, impactoEnSuelo,centroPista,izquierda,centro,derecha,destino;
    //Variables de estados
    private static bool recibiendo = false,atacando=false,preparandose=false,rematando=false,caminando=false,enElAire=false;
    float alturaSuelo,v0xPrimerLanzamiento;
    private static float zAtaque = 0f; //Lo necesitara NPC3
    private static int posicion = 1 ;//0 -> izquierda, 1 -> centro y 2 -> derecha esto es para decirle al NPC3 donde voy a estar antes
    // de empezar a moverme ahi, necesito hacerlo asi para que se empiece a mover o no llegará a la pelota
    Rigidbody rb,rbBall;
    public NavMeshAgent miAgente;

    // Start is called before the first frame update
    void Start()
    {   
        //cte
        alturaSuelo = 0.7326368f;
        v0xPrimerLanzamiento = 0f;
        //gameObjects
        ball = GameObject.FindWithTag("Ball");
        player3 = GameObject.FindWithTag("Player3");
        //Vectores
        dir = new Vector3(0,0,0);
        izquierda = new Vector3(0.7f,1.17f,-2);
        centro = new Vector3(0.7f,1.17f,0f);
        derecha = new Vector3(0.7f,1.17f,2);
        destino=new Vector3(0f,0f,0f);
        impactoEnSuelo = new Vector3(0,0,0);
        centroPista = new Vector3(2,alturaSuelo,0);
        //NavMesh
        miAgente = GetComponent<NavMeshAgent>();
        //RigidBodies
        rbBall = ball.GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posInicialPelota;
        //Recibiendo calcula donde va a caer la pelota segun el vector de velocidad incial y sus posiciones usando las ecuaciones de
        //MRUA, de esta forma me muevo alli antes de que llegue y la recibo
        if(recibiendo){
            //Obtenemos la velocidad inicial de la pelota en el momento en el que el NPC1 la lanza
            velocidadIincial = Ball.getVelocidad();
            v0xPrimerLanzamiento = velocidadIincial.x;
            posInicialPelota=ball.transform.position;
            //a=0.5 por aceleracion de la gravedad b = velocidad en eje y de la pelota y c = y0 - [altura "0" del campo + altura NPC2] 
            //esto lo hago para calcular el instante t en el que la pelota estara sobre la cabeza del jugador 
            // 0 = (y0-y) + v0y * t + 0.5 * (-9.8) * t^2
            float t = (float) ecuacionDeSegundoGrado(0.5*-9.8, velocidadIincial.y, posInicialPelota.y-1.5);
            //Con el tiempo calculo la posicion donde caera: impactoEnSuelo
            impactoEnSuelo = new Vector3(posInicialPelota.x + velocidadIincial.x * t, alturaSuelo,
            posInicialPelota.z + velocidadIincial.z * t);
            zAtaque = impactoEnSuelo.z; // Le digo a NPC3 las coordinadas en Z en las que estaré, las necesita para calcular donde pasarmela
            miAgente.SetDestination(impactoEnSuelo);
            recibiendo=false;
            posicion = calculaPosicion(impactoEnSuelo.z);//Le digo a NPC3 en que parte de la cancha estaré
            NPC3.setEsperando(true); //Activo el estado esperando de NPC3
        }

        //Avanzo hasta la red para colocarme cerca y saltar, activo caminando
        if(preparandose){
            //Si estoy muy cerca de la red tengo que acercarme mas o el pase del NPC3 no me llega a dar por poco mas (ERRORES DE CALCULO ASUMO)
            if(v0xPrimerLanzamiento < 5f) destino = new Vector3(0.5f,transform.position.y,transform.position.z);
            else destino = new Vector3(0.7f,transform.position.y,transform.position.z);
            miAgente.SetDestination(destino);
            preparandose=false;
            caminando=true;
        }
        //Entrare en este metodo cuando el setDestination de preparandose haya llegado a su destino, en ese caso activo rematando
        if(caminando && destinoAlcanzado(destino)) {
            rematando=true;
            caminando=false;
        }
        //Salto para realizar el remate, activo enElAire para saber que estoy saltando y desactivo tanto el navmesh agent como la 
        //componente cinematica del rigidbody del NPC2 para poder hacer que salte
        if(rematando){
            enElAire = true;
            miAgente.enabled = false;
            rb.isKinematic = false;
            rb.velocity = new Vector3(0f,9f,0f);
            rematando=false;
        }
    }

    void OnCollisionEnter(Collision collision){
        //Si colisiono con el suelo y enElAire esta a true significa que acabo de terminar el salto por lo que activo tanto el navmesh agent como la 
        //componente cinematica del rigidbody del NPC p
        if(collision.gameObject.tag == "Suelo" && enElAire){
            rb.isKinematic = true;
            miAgente.enabled = true;
            enElAire=false;
        //Por otro lado si colisiono con la pelota tengo que ver si estoy en el aire o no
        //Si estoy en el aire(Altura >= 1.19f), remato hacia abajo en una direccion que depende de donde este colocado el NPC2. 
        //Si no le paso la pelota al NPC3 
        }else if(collision.gameObject.tag == "Ball"){
            if(transform.position.y <= 1.19f){
                Vector3 dirPase = NPC3.getPosicion() - transform.position;
                Ball.setVelocidad(new Vector3(dirPase.x,5f,dirPase.z));
                rbBall.velocity = Ball.getVelocidad();
                NPC3.setRecibiendo(true);
                preparandose=true;
                
            }else{
                if(transform.position.z > 0f) Ball.setVelocidad(new Vector3(-4f,-2f,-3f));
                else Ball.setVelocidad(new Vector3(-4f,-2f,3f));
                rbBall.velocity = Ball.getVelocidad();
            }
        }
    }

    //Getters y Setters
    public static void setRecibiendo(bool valor){
        recibiendo = valor;
    }

    public static void setAtacando(bool valor){
        atacando = valor;
    }

    public static int getPosicion(){
        return posicion;
    } 
    
    public static float getZAtaque(){
        return zAtaque;
    }

    //Devuelve el entero que representa la parte del campo donde se encuentra el jugador en ese momento: izquierda, centro o derecha
    //Pongo 0.1 y -0.1 porque el NPC2 porque a veces el NPC esta en el centro y su coordenada en el eje Z es ej: 0.0000000001 
    private int calculaPosicion(float z){
        if(z < -0.1f)return 0;
        else if(z > 0.1f) return 2; 
        else return 1;
    }

    //Metodo que calcula la ecuacion de segundo grado dados los parametro a,b y c de su formula
    private double ecuacionDeSegundoGrado(double a, double b, double c){
        double raizPositiva = (-b+(Math.Sqrt(Math.Pow(b,2)-4*a*c)))/(2*a);
		double raizNegativa = (-b-(Math.Sqrt(Math.Pow(b,2)-4*a*c)))/(2*a);

		if(raizNegativa.ToString() == "NaN" || raizPositiva.ToString() == "NaN") { //no tiene sentido que entre pero lo contemplo
            return -1.0;
		}else if(raizPositiva != raizNegativa) { //Si tiene tos raices me quedo con la > 0 pues un tiempo negativo no tiene sentido
            if(raizPositiva>=0)return raizPositiva;
            else return raizNegativa;
		}else {
            return raizPositiva;
		}
    }

    //Calculo si he llegado al destino del nav mesh con un umbral de 0.2. Calculo destinoReal cambiando la componente 'y' 
    //para que no afecte al distance
    private bool destinoAlcanzado(Vector3 destino) {
        bool res=false;
        float x = transform.position.x, xb = destino.x, za = transform.position.z , zb = destino.z;
        Vector3 destinoReal = new Vector3(destino.x,transform.position.y,destino.z);
        float distanciaDestino = Vector3.Distance(transform.position, destinoReal);
        if(distanciaDestino <= 0.2f) res= true;
        return res;
    }
}

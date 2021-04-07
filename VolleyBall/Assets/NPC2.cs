using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPC2 : MonoBehaviour
{
    GameObject ball;
    Vector3 velocidadIincial;
    bool mover=false;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
        ball = GameObject.FindWithTag("Ball");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //double y0;
        Vector3 posInicialPelota,dir=new Vector3(0,0,0);
        int n_frames=0;
        //Obtenemos la velocidad inicial de la pelota en el momento en el que el NPC1 la lanza
        if(velocidad.actualizada){
            velocidadIincial=velocidad.getVelocidad();
            velocidad.actualizada=false;
            posInicialPelota=ball.transform.position;
            //y0=ball.transform.position.y;  Esta perfecta print("Altura de la pelota "+ball.transform.position.y); 

            //a=0.5 por aceleracion de la gravedad b = velocidad en eje y de la pelota y c = y0 - altura "0" del campo   
            float t = (float) ecuacionDeSegundoGrado(0.5*-9.8, velocidadIincial.y, posInicialPelota.y-0.7326368);
            print("tarda en llegar al suelo " + t);
            //Con el tiempo calculo la posicion donde caera: impactoEnSuelo
            Vector3 impactoEnSuelo = new Vector3(posInicialPelota.x + velocidadIincial.x * t, 0.7326368f,
            posInicialPelota.z + velocidadIincial.z * t);

            //calculo en que direccion debo moverme
            dir = new Vector3(impactoEnSuelo.x-transform.position.x,0,impactoEnSuelo.z-transform.position.z);
            print(impactoEnSuelo);
            n_frames=(int)(t/Time.deltaTime);
            print("IMPACTO - POS INICIAL NPC2: " + (impactoEnSuelo - transform.position));
            print("Nos movemos en " + dir);
            mover=true;
        }
        if(mover){
            transform.position += dir * 10f * Time.deltaTime;
            print(transform.position);
        } 
        //Calculamos donde caera la pelota
        //Calculamos tiempo t
        //transform.position=new Vector3(transform.position.x,transform.position.y,ball.transform.position.z);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC1 : MonoBehaviour
{
    GameObject ball;
    Rigidbody rbBall;
    float x,z,fuerza = 6f;
    static System.Random r = new System.Random();

    // // Start is called before the first frame update
     void Start(){
        ball = GameObject.FindWithTag("Ball");
        rbBall = ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){

    }
    
    //Cuando la pelota choca con el NPC1 este calcula con numeros aleatorios a donde lanzarla. He hecho que haya 9 lugares posibles
    // a donde lanzar ya que deben estar dentro de un determinado rango para que caiga en el campo contrario
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Ball"){
            z = direccionZ();
            x = direccionX();
            Ball.setVelocidad(new Vector3(x,1f,z) * fuerza);
            rbBall.velocity = Ball.getVelocidad();
            NPC2.setRecibiendo(true);
        }
    }

    //Devuelve la direccion en la que ira la bola en el eje z
    private float direccionZ(){
        float res;
        int rInt = r.Next(0, 100);
        if(rInt<33)res = 0.2f;
        else if (rInt<66)res =0f;
        else res = -0.2f;
        return res;
    }

    //Devuelve la direccion en la que ira la bola en el eje z
    private float direccionX(){
        float res;
        int rInt = r.Next(0, 100);
        if(rInt<50)res = 0.7f;
        else res = 1.1f;
        return res;
    }

}

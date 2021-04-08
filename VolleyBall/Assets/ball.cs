using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    Vector3 origen,cero;
    float x,z;
    static System.Random r = new System.Random();
    float fuerza = 6f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        origen = transform.position;
        cero = new Vector3(0,0,0);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision){
        //print("Altura de la pelota "+transform.position.y); 
        if(collision.gameObject.tag == "Player"){
            z = direccionZ();
            x = direccionX();
            velocidad.setVelocidad(new Vector3(x,1f,z) * fuerza);
            rb.velocity = velocidad.getVelocidad();
        }else if(collision.gameObject.tag == "Player2"){
            rb.velocity = new Vector3(x*(-1),1f,z*(-1f)) * fuerza ;
        }else{
            //rb.velocity = cero;
            //ransform.position=origen;
        }
    }

    //Devuelve la direccion en la que ira la bola
    private float direccionZ(){
        float res;
        int rInt = r.Next(0, 100);
        if(rInt<33)res = 0.2f;
        else if (rInt<66)res =0f;
        else res = -0.2f;
        return res;
    }
    private float direccionX(){
        float res;
        int rInt = r.Next(0, 100);
        if(rInt<33)res = 0.7f;
        else if (rInt<66)res =1f;
        else res = 1.1f;
        return res;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    Vector3 origen,cero,dir;
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
        if(collision.gameObject.tag == "Player"){
            rb.velocity = direccion() * fuerza;
        }else{
            rb.velocity = cero;
            transform.position=origen;
        }
    }

    //Devuelve la direccion en la que ira la bola
    private Vector3 direccion(){
        Vector3 res;
        int rInt = r.Next(0, 100);
        if(rInt<33)res = new Vector3(0.8f,1,0.2f);
        else if (rInt<66)res = new Vector3(0.7f,1,0);
        else res = new Vector3(0.8f,1,-0.2f);
        return res;
    }
}

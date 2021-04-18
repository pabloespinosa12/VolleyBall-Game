using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ball : MonoBehaviour
{
    Vector3 origen,cero;
    Rigidbody rb;
    public Button boton;
    // Start is called before the first frame update
    void Start()
    {
        origen = transform.position;
        cero = new Vector3(0,0,0);
        rb = GetComponent<Rigidbody>();
        boton.onClick.AddListener(reset);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision){
        /*if(collision.gameObject.tag == "Player"){
            z = direccionZ();
            x = direccionX();
            velocidad.setVelocidad(new Vector3(x,1f,z) * fuerza);
            rb.velocity = velocidad.getVelocidad();
        }else if(collision.gameObject.tag == "Player2"){
            rb.velocity = new Vector3(x*(-1),1f,z*(-1f)) * fuerza ;
        }*/
    }
    void reset(){
        rb.velocity = cero;
        transform.position = origen;
    }
}

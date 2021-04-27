using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    Vector3 origen,cero;
    Rigidbody rb,rb2,rb3;
    public Button boton;
    private static Vector3 velocidadIincial;

   
    // Start is called before the first frame update
    void Start()
    {
        origen = transform.position;
        cero = new Vector3(0,0,0);
        rb = GetComponent<Rigidbody>();
        boton.onClick.AddListener(reset);
        rb2 = GameObject.FindWithTag("Player2").GetComponent<Rigidbody>();
        rb3 = GameObject.FindWithTag("Player3").GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void reset(){
        rb.velocity = cero;
        transform.position = origen;
        rb2.transform.position = new Vector3(4f,rb2.transform.position.y,0f);
        rb3.transform.position = new Vector3(0.7f,rb3.transform.position.y,0f);
    }

    //Get y Set de la velocidad de la pelota
    public static void setVelocidad(Vector3 v){
        velocidadIincial = v;
    }
    public static Vector3 getVelocidad(){
        return velocidadIincial;
    }
}

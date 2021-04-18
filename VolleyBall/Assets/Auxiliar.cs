using UnityEngine;
//Clase que me permite instanciar la velocidad inicial en el script de la pelota y tenerla en el resto
public class Auxiliar{
    private static Vector3 velocidadIincial;
    public static void setVelocidad(Vector3 v){
        velocidadIincial = v;
    }
    public static Vector3 getVelocidad(){
        return velocidadIincial;
    }
}
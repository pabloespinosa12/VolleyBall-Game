using UnityEngine;
//Clase que me permite instanciar la velocidad inicial en el script de la pelota y tenerla en el resto
public class velocidad{
    private static Vector3 velocidadIincial;
    public static bool actualizada=false;
    public static void setVelocidad(Vector3 v){
        velocidadIincial = v;
        actualizada=true;
    }

    public static Vector3 getVelocidad(){
        return velocidadIincial;
    }
}
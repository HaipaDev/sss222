using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysUpsideUp : MonoBehaviour{
    void Update(){
        /*float zRot=0f;
        switch(transform.root.rotation.z){
            case 90:break;
            case -90:break;
            case 180:break;
        }*/
        transform.eulerAngles=new Vector3(transform.rotation.z,transform.rotation.y,-transform.root.rotation.z);
    }
}

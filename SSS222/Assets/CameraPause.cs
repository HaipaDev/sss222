using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPause : MonoBehaviour{
    private Camera camera;

    void Start(){
        camera = (Camera) FindObjectOfType( typeof (Camera) );
    }

    /*void OnApplicationPause(bool paused) {
        if(paused){
            camera.enabled = false;
        }else{
            camera.enabled = true;
        }
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_SandboxOnly : MonoBehaviour{
    void Start(){
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name!="SandboxMode"){Destroy(gameObject);}
        //else{Destroy(this);}
    }
}

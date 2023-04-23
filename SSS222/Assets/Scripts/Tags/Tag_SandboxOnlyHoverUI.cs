using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_SandboxOnlyHoverUI : MonoBehaviour{
    void Start(){
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name!="SandboxMode"){Destroy(GetComponent<HoverUIEnable>());}
        //else{Destroy(this);}
    }
}

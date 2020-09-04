using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMode_Destroy : MonoBehaviour{
    void Start(){
        if(GameSession.instance.cheatmode!=true){Destroy(gameObject);}else{Destroy(this);}
    }
}

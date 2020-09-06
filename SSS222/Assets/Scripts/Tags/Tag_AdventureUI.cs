using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_AdventureUI : MonoBehaviour{
    void Start(){
        if(GameSession.instance.gameModeSelected!=0){Destroy(gameObject);}
    }
}

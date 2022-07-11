using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour{
    void Update(){
        if(GameSession.instance!=null){
            if(GameSession.instance.gamemodeSelected<=0){Destroy(this);}
            if(GameSession.instance.score>GameSession.instance.GetHighscoreCurrent().score){GetComponent<Animator>().SetTrigger("beaten");Destroy(this);}
        }
    }
}

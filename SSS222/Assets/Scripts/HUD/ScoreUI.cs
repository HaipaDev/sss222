using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour{
    void Awake(){
        if(GameSession.instance!=null)if(GameSession.instance.gamemodeSelected<=0){Destroy(this);}
    }
    void Update(){
        if(GameSession.instance!=null)
            if(GameSession.instance.score>GameSession.instance.GetHighscoreCurrent().score){GetComponent<Animator>().SetTrigger("beaten");Destroy(this);}
    }
}

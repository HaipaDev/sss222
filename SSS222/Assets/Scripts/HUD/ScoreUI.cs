using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour{
    void Update(){
        if(GameSession.instance.CheckGameModeSelected("Adventure")){Destroy(this);}
        if(GameSession.instance!=null)if(GameSession.instance.score>GameSession.instance.GetHighscore(GameSession.instance.gameModeSelected)){GetComponent<Animator>().SetTrigger("beaten");Destroy(this);}
    }
}

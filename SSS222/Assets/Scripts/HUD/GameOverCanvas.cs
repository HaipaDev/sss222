using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour{
    public static GameOverCanvas instance;
    [HideInInspector]public bool gameOver;
    void Awake(){instance=this;}
    public void OpenGameOverCanvas(bool open=true){
        gameOver=open;
        transform.GetChild(0).gameObject.SetActive(open);
        //Replace colors
        var scoreTxt=GameObject.Find("ScoreTxt");
        var scoreDescTxt=GameObject.Find("ScoreTxtDesc");
        var highscoreTxt=GameObject.Find("HighscoreTxt");
        var highscoreDescTxt=GameObject.Find("HighscoreTxtDesc");
        var color1=scoreTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient;
        var color2=highscoreTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient;
        if(GameSession.instance.score>=GameSession.instance.GetHighscore(GameSession.instance.gameModeSelected)){
        scoreTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient=color2;
        scoreDescTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient=color2;
        highscoreTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient=color1;
        highscoreDescTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient=color1;
        }
    }
}

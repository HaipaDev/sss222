using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverCanvas : MonoBehaviour{
    public static GameOverCanvas instance;
    [SerializeField] TextMeshProUGUI scoreDescTxt;
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI highscoreDescTxt;
    [SerializeField] TextMeshProUGUI highscoreTxt;
    [HideInInspector]public bool gameOver;
    void Awake(){instance=this;}
    public void OpenGameOverCanvas(bool open=true){
        gameOver=open;
        transform.GetChild(0).gameObject.SetActive(open);
        //Replace colors
        var color1=scoreTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient;
        var color2=highscoreTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient;
        if(GameSession.instance.score>=GameSession.instance.GetHighscoreCurrent().score){
            scoreDescTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient=color2;
            scoreTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient=color2;
            highscoreDescTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient=color1;
            highscoreTxt.GetComponent<TMPro.TextMeshProUGUI>().colorGradient=color1;
        }
    }
}

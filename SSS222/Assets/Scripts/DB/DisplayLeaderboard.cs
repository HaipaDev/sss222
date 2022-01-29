using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DisplayLeaderboard : MonoBehaviour{
    public bool currentUser;
    public int rank;
    public string username;
    public int score;
    TextMeshProUGUI txtRank;
    TextMeshProUGUI txtScore;
    void Start(){
        txtRank=transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        txtScore=transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        DisplayCurrentUserHighscore();
    }
    void Update(){
        txtRank.text="#"+rank.ToString();
        txtScore.text=username+" : \n"+score.ToString();
    }
    public void CurrentUserHighscoreBtn(){
        if(SceneManager.GetActiveScene().name!="ScoreSubmit"){
            if(SaveSerial.instance.hyperGamerLoginData.loggedIn){GSceneManager.instance.LoadScoreSubmitScene();}
            else{GSceneManager.instance.LoadLoginScene();}
        }else{if(SaveSerial.instance.hyperGamerLoginData.loggedIn){GSceneManager.instance.LoadLeaderboardsScene();}
            else{GSceneManager.instance.LoadLoginScene();}
            
        }
    }
    public async void DisplayCurrentUserHighscore(){
    if(currentUser){
        int currentUserRank=0;
        string currentUserName="";
        int currentUserScore=0;
        if(SaveSerial.instance.hyperGamerLoginData.loggedIn){
            var task=FindObjectOfType<DBAccess>().GetScoresFromDB();
            var resultSorted=await task;
            resultSorted=resultSorted.OrderByDescending(e=>e.score).ToList();
            
            for(var i=0;i<resultSorted.Count;i++){if(resultSorted[i].name==SaveSerial.instance.hyperGamerLoginData.username){
                currentUserRank=i;currentUserName=SaveSerial.instance.hyperGamerLoginData.username;currentUserScore=resultSorted[i].score;}}
            if(currentUserScore>0){
                rank=currentUserRank+1;
            }else{
                rank=0;
                currentUserName=SaveSerial.instance.hyperGamerLoginData.username;
            }
            username=currentUserName;
            score=currentUserScore;
        }else{
            rank=0;
            username="Not logged in";
            score=currentUserScore;
        }
    }
    }
}

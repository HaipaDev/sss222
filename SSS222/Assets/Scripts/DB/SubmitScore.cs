﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;
using Steamworks.Data;

public class SubmitScore : MonoBehaviour{
    [SerializeField]GameObject autosubmitScoresToggle;
    void Start(){
        autosubmitScoresToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.autosubmitScores;
    }
    public static void SubmitScoreFunc(){if(GameManager.instance.gamemodeSelected>0){
        if(GameManager.instance.steamAchievsStatsLeaderboards){SteamManager.instance.SubmitScore(GameManager.instance.GetCurrentGamemodeName(),GameManager.instance.GetHighscoreCurrent().score);}
        if(SaveSerial.instance.hyperGamerLoginData.username!=""&&SaveSerial.instance.hyperGamerLoginData.loggedIn){
            DBAccess.instance.SaveScoreToDB(SaveSerial.instance.hyperGamerLoginData.username,GameManager.instance.GetHighscoreCurrent());
            if(_exceptionScenes())if(FindObjectOfType<DisplayLeaderboard>().currentUser)FindObjectOfType<DisplayLeaderboard>().DisplayCurrentUserHighscore();
        }else{if(_exceptionScenes())
            GSceneManager.instance.LoadLoginScene();
        }
    }}
    static bool _exceptionScenes(){return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Leaderboards"||UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="ScoreSubmit";}
    public void ReturnToGMInfo(){StartCoroutine(ReturnToGMInfoI());}
    IEnumerator ReturnToGMInfoI(){
        yield return new WaitForSecondsRealtime(4f);
        GSceneManager.instance.LoadGameModeInfoScene();
    }
    public void SetAutosubmitScores(bool isOn){SaveSerial.instance.settingsData.autosubmitScores=isOn;}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class ScoreUserDataCanvas : MonoBehaviour{
    [Header("Objects")]
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI titleText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI usernameText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI rankText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI scoreText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI playtimeText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI versionBuildText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI dateAchievedText;
    [SceneObjectsOnly][SerializeField]ShipUI shipUI;

    int rank;
    int score;
    float playtime;
    string versionAchieved;
    float buildAchieved;
    DateTime dateAchieved;

    void Start(){
        DisplaySelectedUsersScoreData();
        if(GameSession.instance.GetSelectedUsersDataName()==""){GSceneManager.instance.LoadLeaderboardsScene();}
    }
    void Update(){
        titleText.text=GameSession.instance.GetCurrentGamemodeName()+" highscore for:";
        usernameText.text=GameSession.instance.GetSelectedUsersDataName();
        if(rank==1){rankText.text="<color=#FFD700>#"+rank.ToString()+"</color>";}
        else{rankText.text="#"+rank.ToString();}
        scoreText.text="<color=grey>Score: </color>"+score.ToString();
        playtimeText.text="<color=grey>Playtime: </color>"+GameSession.FormatTime(playtime);
        string versionString="v"+versionAchieved;
        if(buildAchieved!=0)versionString+=" (b"+buildAchieved.ToString()+")";
        versionBuildText.text=versionString;
        dateAchievedText.text=dateAchieved.ToLocalTime().ToString();

        if(GSceneManager.EscPressed()){Back();}
    }
    public void Back(){GSceneManager.instance.LoadLeaderboardsScene();GameSession.instance.ResetSelectedUsersDataName();}
    async void DisplaySelectedUsersScoreData(){
        var result=await DBAccess.instance.GetScoresFromDB();
        var resultSorted=result.OrderByDescending(e=>e.score).ToList();
        for(var i=0;i<resultSorted.Count;i++){if(resultSorted[i].name==GameSession.instance.GetSelectedUsersDataName()){
            rank=i+1;score=resultSorted[i].score;
            playtime=resultSorted[i].playtime;
            versionAchieved=resultSorted[i].version;
            buildAchieved=resultSorted[i].build;
            dateAchieved=resultSorted[i].date;
        }}
    }
}

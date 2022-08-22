using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class ScoreUserDataCanvas : MonoBehaviour{
    [Header("Variables")]
    [SerializeField]bool isSteamDisplay;
    [Header("Objects")]
    [SceneObjectsOnly][SerializeField]ShipUI shipUI;
    [SceneObjectsOnly][SerializeField]Image steamPfp;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI titleText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI usernameText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI rankText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI scoreText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI playtimeText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI versionBuildText;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI dateAchievedText;

    ulong steamId;
    int rank;
    int score;
    float playtime;
    string versionAchieved;
    float buildAchieved;
    DateTime dateAchieved;

    void Start(){
        DisplaySelectedUsersScoreData();
        if(GameSession.instance.GetSelectedUsersDataName()==""){GSceneManager.instance.LoadLeaderboardsScene();}
        if(isSteamDisplay){
            Destroy(playtimeText.gameObject);
            Destroy(versionBuildText.gameObject);
            Destroy(dateAchievedText.gameObject);
        }
        Destroy(steamPfp.gameObject);
        //SetSteamAvatar();
    }
    async void SetSteamAvatar(){
        if(steamId!=0){
            var _steamId=new Steamworks.SteamId();_steamId.Value=steamId;
            var txtr=await SteamManager.instance.GetAvatar(_steamId);
            if(steamPfp!=null){steamPfp.sprite=Sprite.Create(txtr,Rect.zero,Vector2.zero);}
        }
    }
    void Update(){
        if(titleText!=null)titleText.text=GameSession.instance.GetCurrentGamemodeName()+" highscore for:";
        if(usernameText!=null)usernameText.text=GameSession.instance.GetSelectedUsersDataName();
        if(rankText!=null){
            if(rank==1){rankText.text="<color=#FFD700>#"+rank.ToString()+"</color>";}
            else{rankText.text="#"+rank.ToString();}
        }
        if(scoreText!=null)scoreText.text="<color=grey>Score: </color>"+score.ToString();
        if(playtimeText!=null)playtimeText.text="<color=grey>Playtime: </color>"+GameSession.FormatTime(playtime);
        if(versionBuildText!=null){
            string versionString=versionAchieved;
            if(buildAchieved!=0)versionString+=" (b"+buildAchieved.ToString()+")";
            versionBuildText.text=versionString;
        }
        if(dateAchievedText!=null)dateAchievedText.text=dateAchieved.ToLocalTime().ToString();

        if(GSceneManager.EscPressed()){Back();}
    }
    public void Back(){GSceneManager.instance.LoadLeaderboardsScene();GameSession.instance.ResetSelectedUsersDataName();}
    async void DisplaySelectedUsersScoreData(){
        var result=await DBAccess.instance.GetScoresFromDB();
        var resultSorted=result.OrderByDescending(e=>e.score).ToList();
        for(var i=0;i<resultSorted.Count;i++){if(resultSorted[i].name==GameSession.instance.GetSelectedUsersDataName()){
            /*var _user=await DBAccess.instance.GetHyperUser(resultSorted[i].name);
            HyperGamer user=_user.Current.Where(e=>e.username==GameSession.instance.GetSelectedUsersDataName()).First();
            steamId=user.steamId;*/
            rank=i+1;score=resultSorted[i].score;
            playtime=resultSorted[i].playtime;
            versionAchieved=resultSorted[i].version;
            buildAchieved=resultSorted[i].build;
            dateAchieved=resultSorted[i].date;
        }}
    }
}

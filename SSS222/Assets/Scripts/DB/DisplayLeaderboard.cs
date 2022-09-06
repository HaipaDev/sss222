using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Steamworks;
using Steamworks.Data;

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
    }
    void Update(){
        txtRank.text="#"+rank.ToString();
        if(Login._isDeveloperNick(username)){txtScore.text="<color="+Login.developerNicknameColor+">"+username+"</color> : \n <color=#d4d4d4>"+score.ToString()+"</color>";}
        else{txtScore.text=username+" : \n <color=#d4d4d4>"+score.ToString()+"</color>";}
    }
    public void CurrentUserHighscoreBtn(){
        if(SceneManager.GetActiveScene().name!="ScoreSubmit"){
            if(SaveSerial.instance.hyperGamerLoginData.loggedIn){GSceneManager.instance.LoadScoreSubmitScene();}
            else{GSceneManager.instance.LoadLoginScene();}
        }else{if(SaveSerial.instance.hyperGamerLoginData.loggedIn){
            if(score!=0){GameSession.instance.SetSelectedUsersDataName(SaveSerial.instance.hyperGamerLoginData.username);GSceneManager.instance.LoadScoreUsersDataScene();}
            else{GSceneManager.instance.LoadLeaderboardsScene();}
        }
            else{GSceneManager.instance.LoadLoginScene();}
        }
    }
    public void OpenScoreUsersData(){GameSession.instance.SetSelectedUsersDataName(username);GSceneManager.instance.LoadScoreUsersDataScene();}
    public async void DisplayCurrentUserHighscore(){
        if(currentUser){
            int currentUserRank=0;
            int currentUserScore=0;
            if(SaveSerial.instance.hyperGamerLoginData.loggedIn){
                var result=await DBAccess.instance.GetScoresFromDB();
                var resultSorted=result.OrderByDescending(e=>e.score).ToList();
                
                
                for(var i=0;i<resultSorted.Count;i++){
                    var user=await DBAccess.instance.GetUserAsync(SaveSerial.instance.hyperGamerLoginData.username);
                    if(resultSorted[i]._id==user._id){
                        currentUserRank=i;currentUserScore=resultSorted[i].score;
                    }
                }
                if(currentUserScore>0){
                    rank=currentUserRank+1;
                }else{rank=0;}

                username=SaveSerial.instance.hyperGamerLoginData.username;
                score=currentUserScore;
            }else{
                rank=0;
                username="Not logged in";
                score=currentUserScore;
            }
        }
    }
    public async void DisplayCurrentUserHighscoreSteam(bool friends=false){
        if(currentUser){
            int currentUserRank=0;
            int currentUserScore=0;

            Steamworks.Data.Leaderboard? leaderboard = await SteamUserStats.FindLeaderboardAsync(GameSession.instance.GetCurrentGamemodeName());
            if(leaderboard.HasValue){
                LeaderboardEntry[] scores=await leaderboard.Value.GetScoresAsync(100);
                if(friends){scores=await leaderboard.Value.GetScoresFromFriendsAsync();}
                if(scores!=null){if(scores.Length>0){
                    for(var i=0;i<scores.Length;i++){if(scores[i].User.Name==SteamClient.Name){
                        currentUserRank=i;currentUserScore=scores[i].Score;}}
                    if(currentUserScore>0){
                        rank=currentUserRank+1;
                    }
                }else{rank=0;}}else{rank=0;}
                username=SteamClient.Name;
                score=currentUserScore;
            }else{username=SteamClient.Name;rank=0;score=0;}
        }
    }
}

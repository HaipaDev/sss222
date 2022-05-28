using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;

public class SteamManager : MonoBehaviour{  public static SteamManager instance;
    const int appID=playtestID;
    const int mainAppID=2000190;
    const int playtestID=2000200;
    void Awake(){
        if(SteamManager.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
    }
    void Start(){//IEnumerator Start(){
        //yield return new WaitForSeconds(0.1f);
        if(GameSession.instance!=null){if(GameSession.instance.isSteam){
            InitSteam();
            SteamUserStats.RequestCurrentStats();
        }}
    }
    void Update(){
        //SteamClient.RunCallbacks();
    }
    void InitSteam(){
        try{
            SteamClient.Init(appID,true);
            Debug.Log("Steam initialized for appID: " + appID);
        }
        catch(System.Exception e){
            Debug.LogError(e);
            // Something went wrong - it's one of these:
            //
            //     Steam is closed?
            //     Can't find steam_api dll?
            //     Don't have permission to play app?
            //
        }
    }
    void OnApplicationQuit(){SteamClient.Shutdown();}
    public async void SubmitScore(string name,int score){
        Steamworks.Data.Leaderboard? leaderboard = await SteamUserStats.FindLeaderboardAsync(name);
        if(leaderboard.HasValue){
            Steamworks.Data.Leaderboard lb=(Steamworks.Data.Leaderboard)leaderboard;
            var result = await lb.SubmitScoreAsync(score);
        }
    }
}
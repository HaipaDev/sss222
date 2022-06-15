using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Sirenix.OdinInspector;
using Steamworks;
using Steamworks.Data;

public class Leaderboard : MonoBehaviour{
    [Header("Prefabs")]
    [AssetsOnly][SerializeField] GameObject lbElement1;
    [AssetsOnly][SerializeField] GameObject lbElement2;
    [AssetsOnly][SerializeField] GameObject lbElement3;
    [AssetsOnly][SerializeField] GameObject gameModeListElementPrefab;

    [Header("Containers and CurrentUserScores")]
    [SerializeField] RectTransform hgCont;
    [SerializeField] RectTransform steamGlobalCont;
    [SerializeField] RectTransform steamFriendsCont;
    [SceneObjectsOnly][SerializeField] DisplayLeaderboard currentUserScore;
    [SceneObjectsOnly][SerializeField] DisplayLeaderboard currentUserScoreSteamGlobal;
    [SceneObjectsOnly][SerializeField] DisplayLeaderboard currentUserScoreSteamFriends;

    [Header("Panels")]
    [SceneObjectsOnly][SerializeField] GameObject mainPanel;
    [SceneObjectsOnly][SerializeField] GameObject steamGlobalPanel;
    [SceneObjectsOnly][SerializeField] GameObject hyperGamersPanel;
    [SceneObjectsOnly][SerializeField] GameObject steamFriendsPanel;
    [SceneObjectsOnly][SerializeField] GameObject gameModesPanel;
    [SceneObjectsOnly][SerializeField] Transform gameModesListTransform;

    void Start(){
        ClearLeaderboards();
        DisplayLeaderboards();
        OpenMainPanel();
        SetGameModesButtons();
        if(!GameSession.instance.steamAchievsStatsLeaderboards){Destroy(steamGlobalPanel);Destroy(steamFriendsPanel);}
    }
    void Update(){
        if(GSceneManager.EscPressed()){Back();}
    }
    public void OpenMainPanel(){CloseAllPanels();mainPanel.SetActive(true);}
    public void OpenGameModesPanel(){CloseAllPanels();gameModesPanel.SetActive(true);}
    public void Back(){if(gameModesPanel.activeSelf){OpenMainPanel();}else{GSceneManager.instance.LoadSocialsScene();GameSession.instance.ResetSelectedUsersDataName();}}
    public void CloseAllPanels(){
        mainPanel.SetActive(false);
        gameModesPanel.SetActive(false);
    }
    public void ClearLeaderboards(){
        for(var i=0;i<hgCont.childCount;i++){Destroy(hgCont.GetChild(i).gameObject);}
        for(var i=0;i<steamGlobalCont.childCount;i++){Destroy(steamGlobalCont.GetChild(i).gameObject);}
        for(var i=0;i<steamFriendsCont.childCount;i++){Destroy(steamFriendsCont.GetChild(i).gameObject);}
    }
    public async void DisplayLeaderboards(){
        var hgScores=await DBAccess.instance.GetScoresFromDB();
        var hgScoresSorted=hgScores.OrderByDescending(e=>e.score).ToList();

        if(hgScoresSorted.Count>0){
            if(hgCont.childCount>0){
                for(var i=0;i<hgCont.childCount;i++){
                    var go=hgCont.GetChild(i);
                    go.GetComponent<DisplayLeaderboard>().rank=i+1;
                    go.GetComponent<DisplayLeaderboard>().username=hgScoresSorted[i].name;
                    go.GetComponent<DisplayLeaderboard>().score=hgScoresSorted[i].score;
                }
            }
            for(var i=hgCont.childCount;i<hgScoresSorted.Count;i++){
                var element=lbElement2;
                if(i==0)element=lbElement1;
                if(hgScoresSorted[i].name==SaveSerial.instance.hyperGamerLoginData.username&&SaveSerial.instance.hyperGamerLoginData.loggedIn&&(hgScoresSorted[i].name!=""&&SaveSerial.instance.hyperGamerLoginData.username!="")){element=lbElement3;}
                GameObject go=Instantiate(element,hgCont);
                string[]name=go.name.Split('_');
                go.name=name[0]+"_"+(i+1);
                go.GetComponent<DisplayLeaderboard>().rank=i+1;
                go.GetComponent<DisplayLeaderboard>().username=hgScoresSorted[i].name;
                go.GetComponent<DisplayLeaderboard>().score=hgScoresSorted[i].score;
            }
        }
        if(currentUserScore!=null){currentUserScore.DisplayCurrentUserHighscore();}

        //Steam Leaderboards
        if(GameSession.instance.steamAchievsStatsLeaderboards){
            Steamworks.Data.Leaderboard? lb = await SteamUserStats.FindLeaderboardAsync(GameSession.instance.GetCurrentGamemodeName());
            if(lb.HasValue){
                var globalScores = await lb.Value.GetScoresAsync(100);
                if(globalScores.Length>0){
                    if(steamGlobalCont.childCount>0){
                        for(var i=0;i<steamGlobalCont.childCount;i++){
                            var go=steamGlobalCont.GetChild(i);
                            go.GetComponent<DisplayLeaderboard>().rank=i+1;
                            go.GetComponent<DisplayLeaderboard>().username=globalScores[i].User.Name;
                            go.GetComponent<DisplayLeaderboard>().score=globalScores[i].Score;
                        }
                    }
                    for(var i=steamGlobalCont.childCount;i<globalScores.Length;i++){
                        var element=lbElement2;
                        if(i==0)element=lbElement1;
                        if(globalScores[i].User.Name==SteamClient.Name&&(globalScores[i].User.Name!=""&&SteamClient.Name!="")){element=lbElement3;}
                        GameObject go=Instantiate(element,steamGlobalCont);
                        string[]name=go.name.Split('_');
                        go.name=name[0]+"_"+(i+1);
                        go.GetComponent<DisplayLeaderboard>().rank=i+1;
                        go.GetComponent<DisplayLeaderboard>().username=globalScores[i].User.Name;
                        go.GetComponent<DisplayLeaderboard>().score=globalScores[i].Score;
                    }
                }
                if(currentUserScoreSteamGlobal!=null){currentUserScoreSteamGlobal.DisplayCurrentUserHighscoreSteam();}

                var friendsScores = await lb.Value.GetScoresFromFriendsAsync();
                if(friendsScores.Length>0){
                    if(steamFriendsCont.childCount>0){
                        for(var i=0;i<steamFriendsCont.childCount;i++){
                            var go=steamFriendsCont.GetChild(i);
                            go.GetComponent<DisplayLeaderboard>().rank=i+1;
                            go.GetComponent<DisplayLeaderboard>().username=friendsScores[i].User.Name;
                            go.GetComponent<DisplayLeaderboard>().score=friendsScores[i].Score;
                        }
                    }
                    for(var i=steamFriendsCont.childCount;i<friendsScores.Length;i++){
                        var element=lbElement2;
                        if(i==0)element=lbElement1;
                        if(friendsScores[i].User.Name==SteamClient.Name&&(friendsScores[i].User.Name!=""&&SteamClient.Name!="")){element=lbElement3;}
                        GameObject go=Instantiate(element,steamFriendsCont);
                        string[]name=go.name.Split('_');
                        go.name=name[0]+"_"+(i+1);
                        go.GetComponent<DisplayLeaderboard>().rank=i+1;
                        go.GetComponent<DisplayLeaderboard>().username=friendsScores[i].User.Name;
                        go.GetComponent<DisplayLeaderboard>().score=friendsScores[i].Score;
                    }
                }
                if(currentUserScoreSteamFriends!=null){currentUserScoreSteamFriends.DisplayCurrentUserHighscoreSteam(true);}
            }
        }
    }
    void SetGameModesButtons(){
        foreach(GameRules gr in GameCreator.instance.gamerulesetsPrefabs){
            string name=gr.cfgName;     if(name.Contains(" Mode"))name=name.Replace(" Mode","");
            GameObject go=Instantiate(gameModeListElementPrefab,gameModesListTransform);
            gameModesListTransform.GetComponent<ContentSizeFitter>().enabled=true;gameModesListTransform.localPosition=new Vector2(0,-999);
            go.name=name+"-PresetButton";
            go.GetComponent<Button>().onClick.AddListener(()=>SetGamemode(gr.cfgName));
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=name;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text=gr.cfgDesc;
            if(go.transform.GetChild(1).GetChild(0)!=null){Destroy(go.transform.GetChild(1).GetChild(0).gameObject);}
            if(gr.cfgIconsGo!=null){Instantiate(gr.cfgIconsGo,go.transform.GetChild(1));}
            else{go.transform.GetChild(1).gameObject.AddComponent<UnityEngine.UI.Image>().sprite=GameAssets.instance.SprAny(gr.cfgIconAssetName);}
        }
    }
    public void SetGamemode(string name){
        GameSession.instance.SetGamemodeSelectedStr(name);
        ClearLeaderboards();
        DisplayLeaderboards();
        OpenMainPanel();
    }
}

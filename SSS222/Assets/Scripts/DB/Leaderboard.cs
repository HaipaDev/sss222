using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Sirenix.OdinInspector;

public class Leaderboard : MonoBehaviour{
    [SerializeField] DisplayLeaderboard currentUserScore;
    [SerializeField] GameObject container;
    [SerializeField] int containerChildCount;
    [AssetsOnly][SerializeField] GameObject leaderboardElement1;
    [AssetsOnly][SerializeField] GameObject leaderboardElement2;
    [SerializeReference] List<Model_Score> result;
    [SerializeReference] List<Model_Score> resultSorted;
    [SceneObjectsOnly][SerializeField] GameObject mainPanel;
    [SceneObjectsOnly][SerializeField] GameObject gameModesPanel;
    [SceneObjectsOnly][SerializeField] Transform gameModesListTransform;
    [AssetsOnly][SerializeField] GameObject gameModeListElementPrefab;
    void Start(){
        ClearLeaderboards();
        DisplayLeaderboards();
        OpenMainPanel();
        SetGameModesButtons();
    }
    void Update(){if(Input.GetKeyDown(KeyCode.Escape)){Back();}}
    public void OpenMainPanel(){CloseAllPanels();mainPanel.SetActive(true);}
    public void OpenGameModesPanel(){CloseAllPanels();gameModesPanel.SetActive(true);}
    public void Back(){if(gameModesPanel.activeSelf){OpenMainPanel();}else{GSceneManager.instance.LoadSocialsScene();}}
    public void CloseAllPanels(){
        mainPanel.SetActive(false);
        gameModesPanel.SetActive(false);
    }
    public void ClearLeaderboards(){
        for(var i=0;i<container.transform.childCount;i++){Destroy(container.transform.GetChild(i).gameObject);}
    }
    public async void DisplayLeaderboards(){
        var task=DBAccess.instance.GetScoresFromDB();
        result=await task;

        resultSorted=result;
        resultSorted=resultSorted.OrderByDescending(e=>e.score).ToList();

        if(container!=null)if(container.transform.childCount>0){containerChildCount=container.transform.childCount;}

        if(resultSorted.Count>0){
            if(containerChildCount>0){
                containerChildCount=container.transform.childCount;
                for(var i=0;i<containerChildCount;i++){
                    var go=container.transform.GetChild(i);
                    go.GetComponent<DisplayLeaderboard>().rank=i+1;
                    go.GetComponent<DisplayLeaderboard>().username=resultSorted[i].name;
                    go.GetComponent<DisplayLeaderboard>().score=resultSorted[i].score;
                }
            }
            for(var i=containerChildCount;i<resultSorted.Count;i++){
                var element=leaderboardElement2;
                if(i==0){element=leaderboardElement1;}
                GameObject go=Instantiate(element,container.transform);
                string[]name=go.name.Split('_');
                go.name=name[0]+"_0"+(i+1);
                go.GetComponent<DisplayLeaderboard>().rank=i+1;
                go.GetComponent<DisplayLeaderboard>().username=resultSorted[i].name;
                go.GetComponent<DisplayLeaderboard>().score=resultSorted[i].score;
            }
        }
        
        if(currentUserScore!=null){currentUserScore.DisplayCurrentUserHighscore();}
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
            else{go.transform.GetChild(1).gameObject.AddComponent<Image>().sprite=GameAssets.instance.SprAny(gr.cfgIconAssetName);}
        }
    }
    public void SetGamemode(string name){
        GameSession.instance.SetGamemodeSelectedStr(name);
        ClearLeaderboards();
        DisplayLeaderboards();
        OpenMainPanel();
    }
}

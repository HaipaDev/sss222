using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Leaderboard : MonoBehaviour{
    //TextMeshProUGUI txt;
    [SerializeField] GameObject container;
    [SerializeField] int containerChildCount;
    [SerializeField] GameObject leaderboardElement1;
    [SerializeField] GameObject leaderboardElement2;
    [SerializeReference] List<Model_Score> result;
    [SerializeReference] List<Model_Score> resultSorted;
    //[SerializeField] List<int> scores;
    void Start(){
        //txt=GetComponent<TextMeshProUGUI>();

        ClearLeaderboards();
        DisplayLeaderboards();
        //Invoke("DisplayHighscores",0.1f);
    }
    public void ClearLeaderboards(){
        for(var i=0;i<container.transform.childCount;i++){Destroy(container.transform.GetChild(i).gameObject);}
    }
    public async void DisplayLeaderboards(){
        var task=DBAccess.instance.GetScoresFromDB();
        result=await task;
        //var output="";

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
        
        //foreach(var element in resultSorted){output+=element.name+": "+element.score+"\n";}
        //txt.text=output;
    }
}

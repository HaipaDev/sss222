using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Leaderboard : MonoBehaviour{
    DBAccess dBAccess;
    //TextMeshProUGUI txt;
    [SerializeField] GameObject container;
    [SerializeField] int containerChildCount;
    [SerializeField] GameObject leaderboardElement;
    [SerializeReference] List<Model_Score> result;
    [SerializeReference] List<Model_Score> resultSorted;
    //[SerializeField] List<int> scores;
    void Start(){
        dBAccess=FindObjectOfType<DBAccess>();
        //txt=GetComponent<TextMeshProUGUI>();

        Invoke("DisplayHighscores",0.1f);
    }
    private async void DisplayHighscores(){
        var task=dBAccess.GetScoresFromDB();
        result=await task;
        //var output="";

        resultSorted=result;
        resultSorted=resultSorted.OrderByDescending(e=>e.score).ToList();

        if(container.transform.childCount>0){
            containerChildCount=container.transform.childCount;
            for(var i=0;i<containerChildCount;i++){
                var go=container.transform.GetChild(i);
                go.GetComponent<DisplayLeaderboard>().rank=i+1;
                go.GetComponent<DisplayLeaderboard>().username=resultSorted[i].name;
                go.GetComponent<DisplayLeaderboard>().score=resultSorted[i].score;
            }
        }
        for(var i=containerChildCount;i<resultSorted.Count;i++){
            GameObject go=Instantiate(leaderboardElement,container.transform);
            string[]name=go.name.Split('_');
            go.name=name[0]+"_0"+(i+1);
            go.GetComponent<DisplayLeaderboard>().rank=i+1;
            go.GetComponent<DisplayLeaderboard>().username=resultSorted[i].name;
            go.GetComponent<DisplayLeaderboard>().score=resultSorted[i].score;
        }
        
        //foreach(var element in resultSorted){output+=element.name+": "+element.score+"\n";}
        //txt.text=output;
    }
}

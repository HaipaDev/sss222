using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsSocialText : MonoBehaviour{
    [SerializeField] StatsSocialDropdown statsdd;
    string ddvalue="Total";
    TextMeshProUGUI txt;
    void Start(){txt=GetComponent<TextMeshProUGUI>();}
    void Update(){
        if(statsdd!=null){ddvalue=statsdd.value;}
        if(ddvalue=="Total"){
            txt.text=
            "<color=grey>Score Total: </color>"+GetStatTot("scoreTotal")+"\n"
            +"<color=grey>Playtime: </color>"+GameSession.instance.FormatTimeWithHours(int.Parse(GetStatTot("playtime")))+"\n"
            +"<color=grey>Deaths: </color>"+GetStatTot("deaths")+"\n"
            +"<color=grey>Powerups: </color>"+GetStatTot("powerups")+"\n"
            +"<color=grey>Kills Total: </color>"+GetStatTot("killsTotal")+"\n"
            +"<color=grey>Kills Living: </color>"+GetStatTot("killsLiving")+"\n"
            +"<color=grey>Kills Mecha: </color>"+GetStatTot("killsMecha")+"\n"
            //+"<color=grey>Kills Violet: </color>"+GetStatTot("killsViolet")+"\n"
            +"<color=grey>Kills Comets: </color>"+GetStatTot("killsComets")+"\n"
            +"<color=grey>Shots Total: </color>"+GetStatTot("shotsTotal")+"\n";
        }else{
            txt.text=
            "<color=grey>Highscore: </color>"+GameSession.instance.GetHighscoreByName(ddvalue)+"\n"
            +"<color=grey>Score Total: </color>"+GetStatGM("scoreTotal")+"\n"
            +"<color=grey>Longest Session: </color>"+GameSession.instance.FormatTimeWithHours(int.Parse(GetStatGM("longestSession")))+"\n"
            +"<color=grey>Playtime: </color>"+GameSession.instance.FormatTimeWithHours(int.Parse(GetStatGM("playtime")))+"\n"
            +"<color=grey>Deaths: </color>"+GetStatGM("deaths")+"\n"
            +"<color=grey>Powerups: </color>"+GetStatGM("powerups")+"\n"
            +"<color=grey>Kills Total: </color>"+GetStatGM("killsTotal")+"\n"
            +"<color=grey>Kills Living: </color>"+GetStatGM("killsLiving")+"\n"
            +"<color=grey>Kills Mecha: </color>"+GetStatGM("killsMecha")+"\n"
            //+"<color=grey>Kills Violet: </color>"+GetStatGM("killsViolet")+"\n"
            +"<color=grey>Kills Comets: </color>"+GetStatGM("killsComets")+"\n"
            +"<color=grey>Shots Total: </color>"+GetStatGM("shotsTotal")+"\n";
        }
    }
    string GetStatTot(string n){
        string v="";
        StatsTotal st=null;
        st=StatsAchievsManager.instance.statsTotal;
        if(st!=null)v=st.GetType().GetField(n).GetValue(st).ToString();

        return v;
    }
    string GetStatGM(string n){
        string v="";
        StatsGamemode st=null;
        st=StatsAchievsManager.instance.statsGamemodesList.Find(x=>x.gmName.Contains(ddvalue));
        if(st!=null)v=st.GetType().GetField(n).GetValue(st).ToString();

        return v;
    }
}

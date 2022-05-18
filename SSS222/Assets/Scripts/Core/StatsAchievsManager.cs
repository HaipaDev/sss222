using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StatsAchievsManager : MonoBehaviour{   public static StatsAchievsManager instance;
    [Header("Achievements")]
    public List<Achievement> achievsList;
    public int _achievsListCount;
    public bool achievsLoaded;
    public Color uncompletedColor=Color.gray;
    public Color completedColor=new Color(55/255, 255/255, 55/255);//basically green
    public Color epicUncompletedColor=new Color(79/255, 61/255, 97/255);//dark purple
    public Color epicCompletedColor=new Color(160/255, 70/255, 255/255);//light violet
    [Header("Stats")]
    public List<StatsGamemode> statsGamemodesList;
    public StatsTotal statsTotal;
    public int _statsGamemodesListCount;
    public bool statsLoaded;
    public bool statsTotalSummed;
    void Awake(){if(StatsAchievsManager.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
    void Start(){SetStatsList();}
    void SetStatsList(){foreach(GameRules gr in GameCreator.instance.gamerulesetsPrefabs){statsGamemodesList.Add(new StatsGamemode(){gmName=gr.cfgName});}}
    void Update(){
        if(!achievsLoaded)LoadAchievs();
        if(!statsLoaded)LoadStats();
        if(GameSession.instance.gamemodeSelected>0&&achievsLoaded)CheckAllAchievs();
        if(GameSession.instance.gamemodeSelected>0&&statsLoaded)SumStatsTotal();
    }
    void OnValidate(){if(!statsTotalSummed)ClearStatsTotal();}

    #region//Achievs
    void CheckAllAchievs(){
        if((GameSession.instance.GetCurrentGamemodeName().Contains("Arcade")&&GameSession.instance.score>=100)
        ||GameSession.instance.GetHighscoreByName("Arcade")>=100){CompleteAchiev("arcade100");}
        if((GameSession.instance.GetCurrentGamemodeName().Contains("Arcade")&&GameSession.instance.score>=1000)
        ||GameSession.instance.GetHighscoreByName("Arcade")>=1000){CompleteAchiev("arcade1k");}
        if((GameSession.instance.GetCurrentGamemodeName().Contains("Arcade")&&GameSession.instance.score>=10000)
        ||GameSession.instance.GetHighscoreByName("Arcade")>=10000){CompleteAchiev("arcade10k");}
        if(statsTotal.deaths>=100){CompleteAchiev("die100");}
        if(statsTotal.killsComets>=1000){CompleteAchiev("comets1k");}
        if(statsTotal.killsMecha>=500){CompleteAchiev("mechas500");}
    }
    
    public void CompleteAchiev(string str){
        Achievement a;
        if(!GameSession.instance.GetCurrentGamemodeName().Contains("Sandbox")){
            a=GetAchievByName(str,true);//if(a==null)a=GetAchievByDesc(str,true);
            if(a!=null){if(!a._isCompleted()){a.achievData.completed=true;a.achievData.dateAchieved=DateTime.Now;AchievPopups.instance.AddToQueue(a);SaveAchievs();}}
        }
    }
    public Achievement GetAchievByName(string str,bool ignoreWarning=false){var i=achievsList.Find(x=>x.name==str);if(i!=null){return i;}else{if(!ignoreWarning){Debug.LogWarning("No achiev by name: "+str);}return null;}}
    public void SaberBlocked(){CompleteAchiev("saberBlock");}
    #endregion

    #region//Stats
    void SumStatsTotal(){
        if(!statsTotalSummed){
            var i=0;
            for(;i<statsGamemodesList.Count;i++){
                statsTotal.scoreTotal+=statsGamemodesList[i].scoreTotal;
                statsTotal.playtime+=statsGamemodesList[i].playtime;
                statsTotal.deaths+=statsGamemodesList[i].deaths;
                statsTotal.powerups+=statsGamemodesList[i].powerups;
                statsTotal.killsTotal+=statsGamemodesList[i].killsTotal;
                statsTotal.killsLiving+=statsGamemodesList[i].killsLiving;
                statsTotal.killsMecha+=statsGamemodesList[i].killsMecha;
                //statsTotal.killsViolet+=statsGamemodesList[i].killsViolet;
                statsTotal.killsComets+=statsGamemodesList[i].killsComets;
                statsTotal.shotsTotal+=statsGamemodesList[i].shotsTotal;
            }
            if(i==statsGamemodesList.Count){statsTotalSummed=true;}
        }
    }
    public void ClearStatsTotal(){statsTotalSummed=false;statsTotal=new StatsTotal();}
    public StatsGamemode GetStatsForCurrentGamemode(){StatsGamemode r=null;if(GameSession.instance.gamemodeSelected>=0){r=GetStatsForGamemode(GameSession.instance.GetCurrentGamemodeName());}return r;}
    public StatsGamemode GetStatsForGamemode(string str){return statsGamemodesList.Find(x=>x.gmName.Contains(str));}

    public void AddScoreTotal(int i){var s=GetStatsForCurrentGamemode();if(s!=null){s.scoreTotal+=i;ClearStatsTotal();}}
    public void AddPlaytime(int i){var s=GetStatsForCurrentGamemode();if(s!=null){s.playtime+=i;ClearStatsTotal();if(i>s.longestSession){s.longestSession=i;}}}
    public void AddDeaths(){var s=GetStatsForCurrentGamemode();if(s!=null){s.deaths++;ClearStatsTotal();}}
    public void AddPowerups(){var s=GetStatsForCurrentGamemode();if(s!=null){s.powerups++;ClearStatsTotal();}}
    public void AddKills(string name,enemyType type){
        var s=GetStatsForCurrentGamemode();if(s!=null){s.killsTotal++;ClearStatsTotal();}
        if(type==enemyType.living){AddKillsLiving();}
        if(type==enemyType.mecha){AddKillsMecha();}
        if(name.Contains("Comet")){AddKillsComets();}
    }
    public void AddKillsLiving(){var s=GetStatsForCurrentGamemode();if(s!=null)s.killsLiving++;}
    public void AddKillsMecha(){var s=GetStatsForCurrentGamemode();if(s!=null)s.killsMecha++;}
    //public void AddKillsViolet(){var s=GetStatsForCurrentGamemode();s.killsViolet++;}
    public void AddKillsComets(){var s=GetStatsForCurrentGamemode();if(s!=null)s.killsComets++;}
    public void AddShotsTotal(){var s=GetStatsForCurrentGamemode();if(s!=null)s.shotsTotal++;}
    #endregion


    public void SaveAchievs(){      if(SaveSerial.instance!=null)if(SaveSerial.instance.playerData!=null){
        if(SaveSerial.instance.playerData.achievsCompleted.Length!=achievsList.Count){SaveSerial.instance.playerData.achievsCompleted=new AchievData[achievsList.Count];}
        if(SaveSerial.instance.playerData.achievsCompleted[0]==null){
            for(var i=0;i<SaveSerial.instance.playerData.achievsCompleted.Length;i++){
                SaveSerial.instance.playerData.achievsCompleted[i]=new AchievData();}}
        for(var i=0;i<SaveSerial.instance.playerData.achievsCompleted.Length;i++){
            SaveSerial.instance.playerData.achievsCompleted[i].name=achievsList[i].name;}
        foreach(AchievData ad in SaveSerial.instance.playerData.achievsCompleted){var a=achievsList.Find(x=>x.name==ad.name);
            ad.completed=a.achievData.completed;
            ad.dateAchieved=a.achievData.dateAchieved;
        }
        Debug.Log("Saving achievs");
        //for(var i=0;i<SaveSerial.instance.playerData.achievsCompleted.Length;i++){
            //SaveSerial.instance.playerData.achievsCompleted[i]=achievsList.Find(x=>x.id==i).completed;}
    }}
    public void LoadAchievs(){      if(SaveSerial.instance!=null)if(SaveSerial.instance.playerData!=null){
        foreach(AchievData ad in SaveSerial.instance.playerData.achievsCompleted){var a=achievsList.Find(x=>x.name==ad.name);
            a.achievData.completed=ad.completed;
            a.achievData.dateAchieved=ad.dateAchieved;
        }
        //for(var i=0;i<achievsList.Count&&i<SaveSerial.instance.playerData.achievsCompleted.Length;i++){
            //achievsList.Find(x=>x.id==i).completed=SaveSerial.instance.playerData.achievsCompleted[i];}
        }
        achievsLoaded=true;
        Debug.Log("Loading achievs");
    }
    public static int _AchievsListCount(){return StatsAchievsManager.instance._achievsListCount;}


    public void SaveStats(){if(SaveSerial.instance!=null)if(SaveSerial.instance.statsData!=null){
        if(SaveSerial.instance.statsData.statsGamemodesList.Length<achievsList.Count){SaveSerial.instance.statsData.statsGamemodesList=new StatsGamemode[statsGamemodesList.Count];}
        for(var i=0;i<SaveSerial.instance.statsData.statsGamemodesList.Length;i++){
            SaveSerial.instance.statsData.statsGamemodesList[i]=statsGamemodesList[i];}
    }}
    public void LoadStats(){if(SaveSerial.instance!=null)if(SaveSerial.instance.statsData!=null){
        for(var i=0;i<SaveSerial.instance.statsData.statsGamemodesList.Length;i++){
            statsGamemodesList[i]=SaveSerial.instance.statsData.statsGamemodesList[i];}}
        statsLoaded=true;
    }
    public void ResetStatsAchievs(){foreach(Achievement a in achievsList){a.achievData=new AchievData();}SetStatsList();statsTotal=new StatsTotal();}
    public static int GetStatsGMListCount(){return StatsAchievsManager.instance._statsGamemodesListCount;}
}
[System.Serializable]
public class Achievement{
    [Header("Properties")]
    public string name;
    public string displayName;
    public string desc;
    public Sprite icon;
    public bool epic;
    [Header("Values")]
    public AchievData achievData;
    public bool _isCompleted(){return achievData.completed;}
    public DateTime _dateAchieved(){DateTime dt=DateTime.Now;dt=achievData.dateAchieved;return dt;}
}
[System.Serializable]
public class AchievData{
    /*[HideInInspector]*/public string name;
    public bool completed;
    public DateTime dateAchieved;
}


[System.Serializable]
public class StatsGamemode{ public string gmName;
    public int scoreTotal;
    public int longestSession;
    public int playtime;
    public int deaths;
    public int powerups;
    public int killsTotal;
    public int killsLiving;
    public int killsMecha;
    //public int killsViolet;
    public int killsComets;
    public int shotsTotal;
}
[System.Serializable]
public class StatsTotal{
    public int scoreTotal;
    public int playtime;
    public int deaths;
    public int powerups;
    public int killsTotal;
    public int killsLiving;
    public int killsMecha;
    //public int killsViolet;
    public int killsComets;
    public int shotsTotal;
}
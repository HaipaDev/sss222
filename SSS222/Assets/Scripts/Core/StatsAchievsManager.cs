using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StatsAchievsManager : MonoBehaviour{
    public static StatsAchievsManager instance;
    public List<StatsGameMode> statsGameModeList;
    public StatsTotal statsTotal;
    public List<Achievement> achievsList;
    void Awake(){if(StatsAchievsManager.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
    void Start(){foreach(GameRules gr in GameCreator.instance.gamerulesetsPrefabs){statsGameModeList.Add(new StatsGameMode(){gmName=gr.cfgName});}}
    void Update(){
        CheckAllAchievs();
        SumStatsTotal();
    }

    #region//Achievs
    void CheckAllAchievs(){
        if(GameSession.instance.GetHighscoreByName("Arcade")>=100){CompleteAchiev("Get 100 points in Arcade");}
        //if(GetTotalKillsComet()>=1000){CompleteAchiev("Destroy 1k comets");}
    }
    
    public void CompleteAchiev(string str){
        Achievement a;
        a=GetAchievByName(str,true);if(a==null)a=GetAchievByDesc(str,true);
        if(a!=null){if(!a.completed)a.completed=true;}else{Debug.LogWarning("No achiev by name neither desc of: "+str);}
    }
    public Achievement GetAchievByName(string str,bool ignoreWarning=false){var i=achievsList.Find(x=>x.name==str);if(i!=null){return i;}else{if(!ignoreWarning){Debug.LogWarning("No achiev by name: "+str);}return null;}}
    public Achievement GetAchievByDesc(string str,bool ignoreWarning=false){var i=achievsList.Find(x=>x.desc==str);if(i!=null){return i;}else{if(!ignoreWarning){Debug.LogWarning("No achiev by desc: "+str);}return null;}}

    public void SaberBlocked(){CompleteAchiev("Block a projectile with Saber");}
    #endregion

    #region//Stats
    void SumStatsTotal(){
        //statsTotal.killsComets=;
    }
    public StatsGameMode GetStatsForCurrentGameMode(){return statsGameModeList.Find(x=>x.gmName.Contains(GameSession.instance.GetCurrentGameModeName()));}
    public StatsGameMode GetStatsForGameMode(string str){return statsGameModeList.Find(x=>x.gmName.Contains(str));}

    public void AddScoreTotal(int i){var s=GetStatsForCurrentGameMode();s.scoreTotal+=i;}
    public void AddPlaytime(int i){var s=GetStatsForCurrentGameMode();s.playtime+=i;}
    public void AddDeaths(){var s=GetStatsForCurrentGameMode();s.deaths++;}
    public void AddKills(string name){
        var s=GetStatsForCurrentGameMode();s.killsTotal++;
        if(name.Contains("Comet")){AddKillsComets();}
    }
    public void AddKillsComets(){var s=GetStatsForCurrentGameMode();s.killsComets++;}
    #endregion
}
[System.Serializable]
public class Achievement{
    public string name;
    public string desc;
    public Sprite icon;
    public bool completed;
}


[System.Serializable]
public class StatsGameMode{ public string gmName;
    public int scoreTotal;
    public int playtime;
    public int deaths;
    public int killsTotal;
    public int killsComets;
    public int killsMechas;
    public int killsViolet;
}
[System.Serializable]
public class StatsTotal{
    public int scoreTotal;
    public int playtime;
    public int deaths;
    public int killsTotal;
    public int killsComets;
    public int killsMechas;
    public int killsViolet;
}
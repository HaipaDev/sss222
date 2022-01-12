using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StatsAchievsManager : MonoBehaviour{
    public static StatsAchievsManager instance;
    [Header("Achievements")]
    public List<Achievement> achievsList;
    [Header("Stats")]
    public List<StatsGameMode> statsGameModesList;
    public StatsTotal statsTotal;
    public bool statsTotalSummed;
    void Awake(){if(StatsAchievsManager.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
    void Start(){foreach(GameRules gr in GameCreator.instance.gamerulesetsPrefabs){statsGameModesList.Add(new StatsGameMode(){gmName=gr.cfgName});}}
    void Update(){
        CheckAllAchievs();
        SumStatsTotal();
    }
    void OnValidate(){if(!statsTotalSummed)ClearStatsTotal();}

    #region//Achievs
    void CheckAllAchievs(){
        if(GameSession.instance.GetHighscoreByName("Arcade")>=100){CompleteAchiev("Get 100 points in Arcade");}
        if(GameSession.instance.GetHighscoreByName("Arcade")>=1000){CompleteAchiev("Get 1k points in Arcade");}
        if(GameSession.instance.GetHighscoreByName("Arcade")>=10000){CompleteAchiev("Get 10k points in Arcade");}
        if(statsTotal.deaths>=100){CompleteAchiev("Die a 100 times");}
        if(statsTotal.killsComets>=1000){CompleteAchiev("Destroy 1k comets");}
        if(statsTotal.killsMecha>=500){CompleteAchiev("Destroy 500 mechanical enemies");}
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
        if(!statsTotalSummed){
            var i=0;
            for(;i<statsGameModesList.Count;i++){
                statsTotal.scoreTotal+=statsGameModesList[i].scoreTotal;
                statsTotal.playtime+=statsGameModesList[i].playtime;
                statsTotal.deaths+=statsGameModesList[i].deaths;
                statsTotal.killsTotal+=statsGameModesList[i].killsTotal;
                statsTotal.killsLiving+=statsGameModesList[i].killsLiving;
                statsTotal.killsMecha+=statsGameModesList[i].killsMecha;
                //statsTotal.killsViolet+=statsGameModesList[i].killsViolet;
                statsTotal.killsComets+=statsGameModesList[i].killsComets;
            }
            if(i==statsGameModesList.Count){statsTotalSummed=true;}
        }
    }
    public void ClearStatsTotal(){statsTotalSummed=false;statsTotal=new StatsTotal();}
    public StatsGameMode GetStatsForCurrentGameMode(){return statsGameModesList.Find(x=>x.gmName.Contains(GameSession.instance.GetCurrentGameModeName()));}
    public StatsGameMode GetStatsForGameMode(string str){return statsGameModesList.Find(x=>x.gmName.Contains(str));}

    public void AddScoreTotal(int i){var s=GetStatsForCurrentGameMode();s.scoreTotal+=i;ClearStatsTotal();}
    public void AddPlaytime(int i){var s=GetStatsForCurrentGameMode();s.playtime+=i;ClearStatsTotal();}
    public void AddDeaths(){var s=GetStatsForCurrentGameMode();s.deaths++;ClearStatsTotal();}
    public void AddKills(string name,enemyType type){
        var s=GetStatsForCurrentGameMode();s.killsTotal++;ClearStatsTotal();
        if(type==enemyType.living){AddKillsLiving();}
        if(type==enemyType.mecha){AddKillsMecha();}
        if(name.Contains("Comet")){AddKillsComets();}
    }
    public void AddKillsLiving(){var s=GetStatsForCurrentGameMode();s.killsLiving++;}
    public void AddKillsMecha(){var s=GetStatsForCurrentGameMode();s.killsMecha++;}
    //public void AddKillsViolet(){var s=GetStatsForCurrentGameMode();s.killsViolet++;}
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
    public int killsLiving;
    public int killsMecha;
    //public int killsViolet;
    public int killsComets;
}
[System.Serializable]
public class StatsTotal{
    public int scoreTotal;
    public int playtime;
    public int deaths;
    public int killsTotal;
    public int killsLiving;
    public int killsMecha;
    //public int killsViolet;
    public int killsComets;
}
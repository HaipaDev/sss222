using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StatsAchievsManager : MonoBehaviour{
    public static StatsAchievsManager instance;
    [Header("Achievements")]
    public List<Achievement> achievsList;
    [Header("Stats")]
    public List<StatsGamemode> statsGamemodesList;
    public StatsTotal statsTotal;
    public bool statsTotalSummed;
    void Awake(){if(StatsAchievsManager.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
    void Start(){foreach(GameRules gr in GameCreator.instance.gamerulesetsPrefabs){statsGamemodesList.Add(new StatsGamemode(){gmName=gr.cfgName});}}
    void Update(){
        if(GameSession.instance.gamemodeSelected>0)CheckAllAchievs();
        if(GameSession.instance.gamemodeSelected>0)SumStatsTotal();
    }
    void OnValidate(){if(!statsTotalSummed)ClearStatsTotal();}

    #region//Achievs
    void CheckAllAchievs(){
        if((GameSession.instance.GetCurrentGamemodeName().Contains("Arcade")&&GameSession.instance.score>=100)
        ||GameSession.instance.GetHighscoreByName("Arcade")>=100){CompleteAchiev("Get 100 points in Arcade");}
        if((GameSession.instance.GetCurrentGamemodeName().Contains("Arcade")&&GameSession.instance.score>=1000)
        ||GameSession.instance.GetHighscoreByName("Arcade")>=1000){CompleteAchiev("Get 1k points in Arcade");}
        if((GameSession.instance.GetCurrentGamemodeName().Contains("Arcade")&&GameSession.instance.score>=10000)
        ||GameSession.instance.GetHighscoreByName("Arcade")>=10000){CompleteAchiev("Get 10k points in Arcade");}
        if(statsTotal.deaths>=100){CompleteAchiev("Die a 100 times");}
        if(statsTotal.killsComets>=1000){CompleteAchiev("Destroy 1k comets");}
        if(statsTotal.killsMecha>=500){CompleteAchiev("Destroy 500 mechanical enemies");}
    }
    
    public void CompleteAchiev(string str){
        Achievement a;
        a=GetAchievByName(str,true);if(a==null)a=GetAchievByDesc(str,true);
        if(a!=null){if(!a.completed){a.completed=true;AchievPopups.instance.AddToQueue(a);}}else{Debug.LogWarning("No achiev by name neither desc of: "+str);}
    }
    public Achievement GetAchievByName(string str,bool ignoreWarning=false){var i=achievsList.Find(x=>x.name==str);if(i!=null){return i;}else{if(!ignoreWarning){Debug.LogWarning("No achiev by name: "+str);}return null;}}
    public Achievement GetAchievByDesc(string str,bool ignoreWarning=false){var i=achievsList.Find(x=>x.desc==str);if(i!=null){return i;}else{if(!ignoreWarning){Debug.LogWarning("No achiev by desc: "+str);}return null;}}

    public void SaberBlocked(){CompleteAchiev("Block a projectile with Saber");}
    #endregion

    #region//Stats
    void SumStatsTotal(){
        if(!statsTotalSummed){
            var i=0;
            for(;i<statsGamemodesList.Count;i++){
                statsTotal.scoreTotal+=statsGamemodesList[i].scoreTotal;
                statsTotal.playtime+=statsGamemodesList[i].playtime;
                statsTotal.deaths+=statsGamemodesList[i].deaths;
                statsTotal.killsTotal+=statsGamemodesList[i].killsTotal;
                statsTotal.killsLiving+=statsGamemodesList[i].killsLiving;
                statsTotal.killsMecha+=statsGamemodesList[i].killsMecha;
                //statsTotal.killsViolet+=statsGamemodesList[i].killsViolet;
                statsTotal.killsComets+=statsGamemodesList[i].killsComets;
            }
            if(i==statsGamemodesList.Count){statsTotalSummed=true;}
        }
    }
    public void ClearStatsTotal(){statsTotalSummed=false;statsTotal=new StatsTotal();}
    public StatsGamemode GetStatsForCurrentGamemode(){StatsGamemode r=null;if(GameSession.instance.gamemodeSelected>=0){r=GetStatsForGamemode(GameSession.instance.GetCurrentGamemodeName());}return r;}
    public StatsGamemode GetStatsForGamemode(string str){return statsGamemodesList.Find(x=>x.gmName.Contains(str));}

    public void AddScoreTotal(int i){var s=GetStatsForCurrentGamemode();if(s!=null)s.scoreTotal+=i;ClearStatsTotal();}
    public void AddPlaytime(int i){var s=GetStatsForCurrentGamemode();if(s!=null)s.playtime+=i;ClearStatsTotal();}
    public void AddDeaths(){var s=GetStatsForCurrentGamemode();if(s!=null)s.deaths++;ClearStatsTotal();}
    public void AddKills(string name,enemyType type){
        var s=GetStatsForCurrentGamemode();if(s!=null)s.killsTotal++;ClearStatsTotal();
        if(type==enemyType.living){AddKillsLiving();}
        if(type==enemyType.mecha){AddKillsMecha();}
        if(name.Contains("Comet")){AddKillsComets();}
    }
    public void AddKillsLiving(){var s=GetStatsForCurrentGamemode();if(s!=null)s.killsLiving++;}
    public void AddKillsMecha(){var s=GetStatsForCurrentGamemode();if(s!=null)s.killsMecha++;}
    //public void AddKillsViolet(){var s=GetStatsForCurrentGamemode();s.killsViolet++;}
    public void AddKillsComets(){var s=GetStatsForCurrentGamemode();if(s!=null)s.killsComets++;}
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
public class StatsGamemode{ public string gmName;
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
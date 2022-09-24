using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Steamworks;
using Steamworks.Data;

public class StatsAchievsManager : MonoBehaviour{   public static StatsAchievsManager instance;
    [Header("Achievements")]
    [Searchable]public List<Achievement> achievsList;
    //[DisableInEditorMode]public int _achievsListCount;
    public UnityEngine.Color uncompletedColor=UnityEngine.Color.gray;
    public UnityEngine.Color completedColor=new UnityEngine.Color(55/255, 255/255, 55/255);//basically green
    public UnityEngine.Color epicUncompletedColor=new UnityEngine.Color(79/255, 61/255, 97/255);//dark purple
    public UnityEngine.Color epicCompletedColor=new UnityEngine.Color(160/255, 70/255, 255/255);//light violet
    [Header("Stats")]
    public List<StatsGamemode> statsGamemodesList;
    public StatsTotal statsTotal;
    [DisableInEditorMode]public int _statsGamemodesListCount;
    [Header("Other Stats")]
    public float sandboxTime;
    public float sandboxTimeMax=(15*60);
    public int personalityCrisisCount;
    public int personalityCrisisCountMax=20;
    public float personalityCrisisTimeMax=(5*60);
    public float personalityCrisisTimer;
    public bool personalityCrisisTimerOn=false;
    public float determinationTimeMax=(1*60);
    public float determinationHealth=25;
    public float determinationTimer;
    public bool determinationTimerOn=false;
    public List<string> uniquePowerups=new List<string>();
    [Header("Other")]
    public bool achievsLoaded;
    public bool statsLoaded;
    public bool statsTotalSummed;

    void Awake(){if(StatsAchievsManager.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);gameObject.name=gameObject.name.Split('(')[0];}}
    void Start(){SetStatsList();}//uniquePowerups=new List<string>();}
    void SetStatsList(){foreach(GameRules gr in CoreSetup.instance.gamerulesetsPrefabs){statsGamemodesList.Add(new StatsGamemode(){gmName=gr.cfgName});}statsGamemodesList.Add(new StatsGamemode(){gmName="Sandbox Mode"});}
    void Update(){
        if(!achievsLoaded)LoadAchievs();
        if(!statsLoaded)LoadStats();

        if(achievsLoaded)CheckAllAchievs();
        if(statsLoaded)SumStatsTotal();
    }
    void OnValidate(){if(!statsTotalSummed)ClearStatsTotal();}

    #region//Achievs
    void CheckAllAchievs(){
        if((GameManager.instance.GetCurrentGamemodeName().Contains("Arcade")&&GameManager.instance.score>=100)
        ||GameManager.instance.GetHighscoreByName("Arcade").score>=100){CompleteAchiev("arcade_score-1");}
        if((GameManager.instance.GetCurrentGamemodeName().Contains("Arcade")&&GameManager.instance.score>=1000)
        ||GameManager.instance.GetHighscoreByName("Arcade").score>=1000){CompleteAchiev("arcade_score-2");}
        if((GameManager.instance.GetCurrentGamemodeName().Contains("Arcade")&&GameManager.instance.score>=10000)
        ||GameManager.instance.GetHighscoreByName("Arcade").score>=10000){CompleteAchiev("arcade_score-3");GameManager.instance.GoldenMoyaiPopup();}
        if((GameManager.instance.GetCurrentGamemodeName().Contains("Hardcore")&&GameManager.instance.score>=666)
        ||GameManager.instance.GetHighscoreByName("Hardcore").score>=666){CompleteAchiev("hardcore_score-1");}
        if((GameManager.instance.GetCurrentGamemodeName().Contains("Classic")&&GameManager.instance.score>=2077)
        ||GameManager.instance.GetHighscoreByName("Classic").score>=2077){CompleteAchiev("classic_score-1");}
        if((GameManager.instance.GetCurrentGamemodeName().Contains("Meteor")&&GameManager.instance.score>=150)
        ||GameManager.instance.GetHighscoreByName("Meteor").score>=150){CompleteAchiev("meteor_score-1");}

        if(statsTotal.deaths>=100){CompleteAchiev("die-1");}
        if(statsTotal.killsComets>=1000){CompleteAchiev("comets_kills-1");}
        if(statsTotal.killsMecha>=500){CompleteAchiev("mechas_kills-1");}


        if(GameManager.instance._isSandboxMode()/*UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Sandbox")
        ||GameManager.instance.GetCurrentGamemodeName().Contains("Sandbox")*/){sandboxTime+=Time.unscaledDeltaTime;}
        if(sandboxTime>=sandboxTimeMax){CompleteAchiev("sandbox_time-1");}

        if(personalityCrisisTimerOn&&personalityCrisisTimer<personalityCrisisTimeMax){personalityCrisisTimer+=Time.unscaledDeltaTime;}
        else{personalityCrisisTimerOn=false;personalityCrisisTimer=0;personalityCrisisCount=0;}
        if(personalityCrisisCount>=personalityCrisisCountMax){CompleteAchiev("customize-crisis");}

        if(determinationTimerOn&&!GameManager.GlobalTimeIsPaused&&determinationTimer<determinationTimeMax){determinationTimer+=Time.deltaTime;}
        else if(Player.instance!=null&&determinationTimer>=determinationTimeMax){CompleteAchiev("determination");StopDeterminationTimer();}
        if(Player.instance==null||UnityEngine.SceneManagement.SceneManager.GetActiveScene().name!="Game"){StopDeterminationTimer();}

        if(Player.instance!=null){if(Player.instance.health<=determinationHealth&&(GameManager.instance.GetCurrentGamemodeName().Contains("Arcade")||GameManager.instance.GetCurrentGamemodeName().Contains("Hardcore")))SetDeterminationTimer();}
    }
    
    public void CompleteAchiev(string str){
        Achievement a;
        //if(!GameManager.instance._isSandboxMode()){
            a=GetAchievByName(str,true);//if(a==null)a=GetAchievByDesc(str,true);
            if(a!=null){
                if(!a._isCompleted()){
                    a.achievData.completed=true;a.achievData.dateAchieved=DateTime.Now;AchievPopups.instance.AddToQueue(a.name);SaveAchievs();
                    AchievCosmeticsUnlocks(a);
                }
                if(GameManager.instance.steamAchievsStatsLeaderboards){
                    var sa=new Steamworks.Data.Achievement(str);
                    //if(Steamworks.SteamUserStats.Achievements.Intersect<Steamworks.Data.Achievement>(str).Any()>0){
                        if(!sa.State)sa.Trigger();
                        if(GameManager.instance.GetCurrentGamemodeName().Contains("Arcade")){SteamUserStats.SetStat("arcadeScore",GameManager.instance.score);}//string _strp1=str.Split('-')[0]+int.Parse(str.Split('-')[1])+1;if(Steamworks.SteamUserStats.Achievements.Contains(x=>x.name==_strp1))SteamUserStats.IndicateAchievementProgress(_strp1,);}
                        if(GameManager.instance.GetCurrentGamemodeName().Contains("Hardcore")){SteamUserStats.SetStat("hardcoreScore",GameManager.instance.score);}
                        if(GameManager.instance.GetCurrentGamemodeName().Contains("Classic")){SteamUserStats.SetStat("classicScore",GameManager.instance.score);}
                        if(GameManager.instance.GetCurrentGamemodeName().Contains("Meteor")){SteamUserStats.SetStat("meteorScore",GameManager.instance.score);}
                        /*if(str.Contains("comets_kills")){*/SteamUserStats.SetStat("cometsDestroyed",statsTotal.killsComets);//}
                        /*if(str.Contains("mechas_kills")){*/SteamUserStats.SetStat("mechasDestroyed",statsTotal.killsMecha);//}
                        /*if(str.Contains("sandbox_time")){*/SteamUserStats.SetStat("sandboxTime",Mathf.RoundToInt(sandboxTime));//}
                    //}
                    SteamUserStats.StoreStats();
                }
            }
        //}
    }
    public Achievement GetAchievByName(string str,bool ignoreWarning=false){var i=achievsList.Find(x=>x.name==str);if(i!=null){return i;}else{if(!ignoreWarning){Debug.LogWarning("No achiev by name: "+str);}return null;}}
    public void SaberBlocked(){CompleteAchiev("saberBlock");}
    public void CoreCollected(){CompleteAchiev("coreCollect");}
    public void ModuleUnlocked(){CompleteAchiev("module");}
    public void DeepFried(){CompleteAchiev("deepFried");}
    public void Customized(){CompleteAchiev("customize");SetPersonalityCrisisTimer();AddPersonalityCrisisCount();}
    public void CustomizedAll(){CompleteAchiev("customize-all");}
    public void BossDefeated(string name){switch(name){
        case "Moon of Lunacy":CompleteAchiev("boss1");break;
    }}
    void AchievCosmeticsUnlocks(Achievement a){
        if(a.name=="arcade_score-3"){CustomizationInventory._unlockGoldenMoyai();}
        if(a.name=="boss1"){CustomizationInventory._unlockMOL();}
    }

    [Button]public void ClearSteamAchievs(){foreach(Steamworks.Data.Achievement sa in SteamUserStats.Achievements){sa.Clear();}}
    #endregion

    #region//Stats
    public void SumStatsTotal(){
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
    public void SetSteamStats(){
        if(GameManager.instance.steamAchievsStatsLeaderboards){
            //SteamUserStats.RequestCurrentStats();
            SteamUserStats.AddStat("deaths",1);
            var arcadeHighscore=SaveSerial.instance.playerData.highscore[GameManager.instance.GetGamemodeID("Arcade")];
            SteamUserStats.SetStat("arcadeScore",arcadeHighscore.score);
            var hardcoreHighscore=SaveSerial.instance.playerData.highscore[GameManager.instance.GetGamemodeID("Hardcore")];
            SteamUserStats.SetStat("hardcoreScore",hardcoreHighscore.score);
            var meteorHighscore=SaveSerial.instance.playerData.highscore[GameManager.instance.GetGamemodeID("Meteor")];
            SteamUserStats.SetStat("meteorScore",meteorHighscore.score);
            var cometsDestroyed=statsTotal.killsComets;
            SteamUserStats.SetStat("cometsDestroyed",cometsDestroyed);
            var mechasDestroyed=statsTotal.killsMecha;
            SteamUserStats.SetStat("mechasDestroyed",mechasDestroyed);
            var shotsFired=statsTotal.shotsTotal;
            SteamUserStats.SetStat("shotsFired",shotsFired);
            SteamUserStats.SetStat("sandboxTime",Mathf.RoundToInt(sandboxTime));
            SteamUserStats.StoreStats();
        }
    }
    public void ClearStatsTotal(){statsTotalSummed=false;statsTotal=new StatsTotal();}
    [Button]public void ClearSteamStats(){SteamUserStats.ResetAll(false);SteamUserStats.StoreStats();SteamUserStats.RequestCurrentStats();}
    [Button]public void ClearSteamAllStatsAndAchievs(){SteamUserStats.ResetAll(true);SteamUserStats.StoreStats();SteamUserStats.RequestCurrentStats();}
    public StatsGamemode GetStatsForCurrentGamemode(){StatsGamemode r=null;if(GameManager.instance.gamemodeSelected>=0){r=GetStatsForGamemode(GameManager.instance.GetCurrentGamemodeName());}return r;}
    public StatsGamemode GetStatsForGamemode(string str){return statsGamemodesList.Find(x=>x.gmName.Contains(str));}

    public void AddScoreTotal(int i){var s=GetStatsForCurrentGamemode();if(s!=null){s.scoreTotal+=i;ClearStatsTotal();}}
    public void AddPlaytime(int i){var s=GetStatsForCurrentGamemode();if(s!=null){s.playtime+=i;ClearStatsTotal();if(i>s.longestSession){s.longestSession=i;}}}
    public void AddDeaths(){var s=GetStatsForCurrentGamemode();if(s!=null){s.deaths++;ClearStatsTotal();}}
    public void AddPowerups(string name){
        if(uniquePowerups!=null){if(!uniquePowerups.Contains(name)){string _name=name;if(name.Contains("(Clone)")){_name=name.Split("(")[0];}
            uniquePowerups.Add(name);if(GameManager.instance.steamAchievsStatsLeaderboards){SteamUserStats.AddStat("uniquePowerups",1);SteamUserStats.StoreStats();}}}//else{uniquePowerups=new List<string>();AddPowerups(name);}
        var s=GetStatsForCurrentGamemode();if(s!=null){s.powerups++;ClearStatsTotal();}
    }
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

    
    public void AddPersonalityCrisisCount(){personalityCrisisCount++;}
    public void SetPersonalityCrisisTimer(){if(!personalityCrisisTimerOn)personalityCrisisTimerOn=true;}
    public void SetDeterminationTimer(){if(!determinationTimerOn)determinationTimerOn=true;}
    public void StopDeterminationTimer(){determinationTimerOn=false;determinationTimer=0;}
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
    //public static int _AchievsListCount(){return StatsAchievsManager.instance._achievsListCount;}


    public void SaveStats(){if(SaveSerial.instance!=null)if(SaveSerial.instance.statsData!=null){
        if(SaveSerial.instance.statsData.statsGamemodesList.Length<achievsList.Count){SaveSerial.instance.statsData.statsGamemodesList=new StatsGamemode[statsGamemodesList.Count];}
        for(var i=0;i<SaveSerial.instance.statsData.statsGamemodesList.Length;i++){
            SaveSerial.instance.statsData.statsGamemodesList[i]=statsGamemodesList[i];}
        SaveSerial.instance.statsData.sandboxTime=sandboxTime;
        SaveSerial.instance.statsData.uniquePowerups=uniquePowerups;
        SetSteamStats();
        /*if(GameManager.instance.steamAchievsStatsLeaderboards){
            SteamUserStats.SetStat("sandboxTime",Mathf.RoundToInt(sandboxTime));
            SteamUserStats.StoreStats();
        }*/
    }}
    public void LoadStats(){if(SaveSerial.instance!=null)if(SaveSerial.instance.statsData!=null){
        for(var i=0;i<SaveSerial.instance.statsData.statsGamemodesList.Length;i++){
            statsGamemodesList[i]=SaveSerial.instance.statsData.statsGamemodesList[i];}}
        sandboxTime=SaveSerial.instance.statsData.sandboxTime;
        uniquePowerups=SaveSerial.instance.statsData.uniquePowerups;
        statsLoaded=true;
    }
    [Button]public void ResetStatsAchievs(){statsGamemodesList=new List<StatsGamemode>();SetStatsList();statsTotal=new StatsTotal();foreach(Achievement a in achievsList){a.achievData=new AchievData();}}
    public static int GetStatsGMListCount(){return StatsAchievsManager.instance._statsGamemodesListCount;}
}
[System.Serializable]
public class Achievement{
    [Header("Properties")]
    public string name;
    public string displayName;
    public string desc;
    public Sprite icon;
    public Sprite iconInc;
    public bool epic;
    public bool hidden;
    [Header("Values")]
    [DisableInEditorMode]public AchievData achievData;
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
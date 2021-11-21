using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnReqsMono:MonoBehaviour{
    public List<spawnReqs> spawnReqsList=new List<spawnReqs>();
    //public static void AddEnergy(float val){foreach(ScriptableObject l in Array.Find(FindObjectsOfType<ScriptableObject>(),x=>x.GetType().GetField("spawnReqs")!=null)){var rd=l.GetType().GetField("spawnReqs");var sr=(spawnReqs)rd.GetValue(l);if(sr is spawnEnergy){var ss=(spawnEnergy)sr;ss.energy+=val;}}}
    public static void AddEnergy(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnEnergy){var ss=(spawnEnergy)sr;ss.energy+=val;}}}
    public static void AddMissed(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnEnergy){var ss=(spawnEnergy)sr;ss.energy+=val;}}}//Not sure how to get to that, since its the same class
    public static void AddPwrups(int val=1){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnPwrups){var ss=(spawnPwrups)sr;ss.pwrups+=val;}}}
    public static void AddKills(int val=1){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnKills){var ss=(spawnKills)sr;ss.kills+=val;}}}
    public static void AddDmg(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnDmg){var ss=(spawnDmg)sr;ss.dmg+=val;}}}
    public static void AddCounts(WaveConfig waveConfig){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnCounts){var ss=(spawnCounts)sr;if(waveConfig==ss.countsWave)ss.counts+=1;}}}

    public static spawnReqsMono instance;
    void Awake(){if(spawnReqsMono.instance!=null){Destroy(this);}else{instance=this;}}
    public void CheckSpawns(spawnReqs x, spawnReqsType xt, MonoBehaviour mb, IEnumerator cor){
        if(!spawnReqsMono.instance.spawnReqsList.Contains(x)){spawnReqsMono.instance.spawnReqsList.Add(x);}
        if(x.timeEnabled){
            if(x.timer==-4){RestartTime(x);}
            if(x.timer>0&&!GameSession.GlobalTimeIsPaused){x.timer-=Time.deltaTime;}
            if(x.timer<=0&&x.timer!=-4&&!x.bothNeeded){Debug.Log(mb);Debug.Log(cor);mb.StartCoroutine(cor);RestartTime(x);}
        }else if((x.secondEnabled&&!x.bothNeeded)||(x.secondEnabled&&x.bothNeeded&&x.timeEnabled)){
            if(xt==spawnReqsType.energy||xt==spawnReqsType.missed){
                var xs=(spawnEnergy)x;
                if(xs.energy>=xs.energyNeeded){if(x.startTimeAfterSecond&&x.timer==-4){RestartTime(x);}if(x.timer<=0&&x.timer!=-4){xs.energy=0;mb.StartCoroutine(cor);}}
            }
            else if(xt==spawnReqsType.pwrups){
                var xs=(spawnPwrups)x;
                if(xs.pwrups>=xs.pwrupsNeeded){if(x.startTimeAfterSecond&&x.timer==-4){RestartTime(x);}if(x.timer<=0&&x.timer!=-4){xs.pwrups=0;mb.StartCoroutine(cor);}}
            }
            else if(xt==spawnReqsType.kills){
                var xs=(spawnKills)x;
                if(xs.kills>=xs.killsNeeded){if(x.startTimeAfterSecond&&x.timer==-4){RestartTime(x);}if(x.timer<=0&&x.timer!=-4){xs.kills=0;mb.StartCoroutine(cor);}}
            }
            else if(xt==spawnReqsType.dmg){
                var xs=(spawnDmg)x;
                if(xs.dmg>=xs.dmgNeeded){if(x.startTimeAfterSecond&&x.timer==-4){RestartTime(x);}if(x.timer<=0&&x.timer!=-4){xs.dmg=0;mb.StartCoroutine(cor);}}
            }
            else if(xt==spawnReqsType.counts){
                var xs=(spawnCounts)x;
                if(xs.counts>=xs.countsNeeded){if(x.startTimeAfterSecond&&x.timer==-4){RestartTime(x);}if(x.timer<=0&&x.timer!=-4){xs.counts=0;mb.StartCoroutine(cor);}}
            }
        }
        void RestartTime(spawnReqs x){spawnReqsMono.instance.RestartTime(x);}
    }
    public void RestartTime(spawnReqs x){SetTime(x,UnityEngine.Random.Range(x.time.x,x.time.y));}
    public void SetTime(spawnReqs x, float tim){x.timer=tim;}
}

public enum spawnReqsType{time,energy,missed,pwrups,kills,dmg,counts}
//public enum spawnerType{waves,powerups,disrupters}
//public interface ISpawnerConfig{}
[System.Serializable]public class spawnReqs{
    //[HideInInspector]public spawnReqsMono srm=new spawnReqsMono();
    public bool timeEnabled=true;
    public Vector2 time=new Vector2(10f,20f);
    public float timer=-4;
    public int repeat=1;
    public float repeatInterval=0.75f;
    public bool secondEnabled=true;
    public bool bothNeeded=true;
    public bool startTimeAfterSecond=false;
}
[System.Serializable]public class spawnScore:spawnReqs{
    public float EVscoreMax=100;
    public float EVscore;
}
[System.Serializable]public class spawnEnergy:spawnReqs{
    public float energyNeeded=100;
    public float energy;
}
[System.Serializable]public class spawnPwrups:spawnReqs{
    public int pwrupsNeeded=2;
    public int pwrups;
}
[System.Serializable]public class spawnKills:spawnReqs{
    public float killsNeeded=50;
    public float kills;
}
[System.Serializable]public class spawnDmg:spawnReqs{
    public float dmgNeeded=200;
    public float dmg;
}
[System.Serializable]public class spawnCounts:spawnReqs{
    public WaveConfig countsWave;
    public float countsNeeded=3;
    public float counts;
}
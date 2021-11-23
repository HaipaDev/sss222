using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnReqsMono:MonoBehaviour{
    public List<spawnReqs> spawnReqsList=new List<spawnReqs>();
    //public static void AddEnergy(float val){foreach(ScriptableObject l in Array.Find(FindObjectsOfType<ScriptableObject>(),x=>x.GetType().GetField("spawnReqs")!=null)){var rd=l.GetType().GetField("spawnReqs");var sr=(spawnReqs)rd.GetValue(l);if(sr is spawnEnergy){var ss=(spawnEnergy)sr;ss.energy+=val;}}}
    public static void AddEnergy(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnEnergy){var ss=(spawnEnergy)sr;ss.energy+=val;}}}
    public static void AddMissed(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnEnergy){var ss=(spawnEnergy)sr;ss.energy+=val;}}}//Not sure how to get to that, since its the same class
    public static void AddScore(int val,int id=0){
        if(id==-1||id==-2){
            List<spawnReqs> list=spawnReqsMono.instance.spawnReqsList.FindAll(x=>x is spawnScore);
            List<spawnScore> list2=new List<spawnScore>();foreach(spawnReqs ssl in list){list2.Add((spawnScore)ssl);}
            spawnScore ss=list2.Find(x=>x.specialId==id);if(ss.scoreMax!=-5&&val!=-5){ss.score+=val;}else if(val==-5){ss.score=ss.scoreMax;}
        }else if(id>=0){
            foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnScore){var ss=(spawnScore)sr;if(ss.scoreMax!=-5&&val!=-5){ss.score+=val;}else if(val==-5){ss.score=ss.scoreMax;}}}
        }
    }
    public static void AddDmg(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnDmg){var ss=(spawnDmg)sr;ss.dmg+=val;}}}
    public static void AddPwrups(int val=1){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnPwrups){var ss=(spawnPwrups)sr;ss.pwrups+=val;}}}
    public static void AddKills(int val=1){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnKills){var ss=(spawnKills)sr;ss.kills+=val;}}}
    public static void AddWaveCounts(WaveConfig waveConfig){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnWaveCounts){var ss=(spawnWaveCounts)sr;if(waveConfig==ss.wave)ss.counts+=1;}}}
    public static void AddPowerupCounts(PowerupItem powerupItem){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnPowerupCounts){var ss=(spawnPowerupCounts)sr;if(powerupItem==ss.powerupItem)ss.counts+=1;}}}

    public static void RandomizeScoreMax(int id=0){
        if(id==-1||id==-2){
            List<spawnReqs> list=spawnReqsMono.instance.spawnReqsList.FindAll(x=>x is spawnScore);
            List<spawnScore> list2=new List<spawnScore>();foreach(spawnReqs ssl in list){list2.Add((spawnScore)ssl);}
            spawnScore ss=list2.Find(x=>x.specialId==id);ss.scoreMax=UnityEngine.Random.Range(ss.scoreMaxSetRange.x,ss.scoreMaxSetRange.y);
        }else if(id>=0){
            foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnScore){var ss=(spawnScore)sr;ss.scoreMax=UnityEngine.Random.Range(ss.scoreMaxSetRange.x,ss.scoreMaxSetRange.y);}}
        }
    }
    public static spawnReqsMono instance;
    void Awake(){if(spawnReqsMono.instance!=null){Destroy(this);}else{instance=this;}}
    public void CheckSpawns(spawnReqs x, spawnReqsType xt, MonoBehaviour mb, IEnumerator cor){
        if(!spawnReqsMono.instance.spawnReqsList.Contains(x)){spawnReqsMono.instance.spawnReqsList.Add(x);}
        if(x.timeEnabled){
            if(x.timer==-4&&!x.startTimeAfterSecond){RestartTime(x);}
            if(x.timer>0&&!GameSession.GlobalTimeIsPaused){x.timer-=Time.deltaTime;}
            if(x.timer<=0&&x.timer!=-4&&!x.bothNeeded){mb.StartCoroutine(cor);RestartTime(x);}
        }else if((!x.bothNeeded)||(x.bothNeeded&&x.timeEnabled)){
            if(xt==spawnReqsType.energy||xt==spawnReqsType.missed){
                var xs=(spawnEnergy)x;
                if(xs.energy>=xs.energyNeeded){ConditionCheck<spawnEnergy>(x,"energy");}
            }
            else if(xt==spawnReqsType.score){
                var xs=(spawnScore)x;
                if(xs.score>=xs.scoreMax){ConditionCheck<spawnScore>(x,"score");}
            }
            else if(xt==spawnReqsType.pwrups){
                var xs=(spawnPwrups)x;
                if(xs.pwrups>=xs.pwrupsNeeded){ConditionCheck<spawnPwrups>(x,"pwrups");}
            }
            else if(xt==spawnReqsType.kills){
                var xs=(spawnKills)x;
                if(xs.kills>=xs.killsNeeded){ConditionCheck<spawnKills>(x,"kills");}
            }
            else if(xt==spawnReqsType.dmg){
                var xs=(spawnDmg)x;
                if(xs.dmg>=xs.dmgNeeded){ConditionCheck<spawnDmg>(x,"dmg");}
            }
            else if(xt==spawnReqsType.waveCounts){
                var xs=(spawnWaveCounts)x;
                if(xs.counts>=xs.countsNeeded){ConditionCheck<spawnWaveCounts>(x,"counts");}
            }
            else if(xt==spawnReqsType.powerupCounts){
                var xs=(spawnPowerupCounts)x;
                if(xs.counts>=xs.countsNeeded){ConditionCheck<spawnPowerupCounts>(x,"counts");}
            }
        }
        void RestartTime(spawnReqs x){spawnReqsMono.instance.RestartTime(x);}
        void ConditionCheck<T>(spawnReqs x, string val)where T:spawnReqs{T xs=(T)x;if(x.startTimeAfterSecond&&x.timer==-4){RestartTime(x);return;}if((xs.bothNeeded&&x.timer<=0&&x.timer!=-4)||(!xs.bothNeeded)){xs.GetType().GetField(val).SetValue(xs,0);mb.StartCoroutine(cor);}}
    }
    public void RestartTime(spawnReqs x){SetTime(x,UnityEngine.Random.Range(x.time.x,x.time.y));}
    public void SetTime(spawnReqs x, float tim){x.timer=tim;}

    public static void Validate(ref spawnReqs spawnReqsRef, ref spawnReqsType spawnReqsTypeRef){
    if(spawnReqsTypeRef==spawnReqsType.time){spawnReqsRef=new spawnReqs();spawnReqsRef.bothNeeded=false;}
    if(spawnReqsTypeRef==spawnReqsType.energy){spawnReqsRef=new spawnEnergy();}
    if(spawnReqsTypeRef==spawnReqsType.missed){spawnReqsRef=new spawnEnergy();}
    if(spawnReqsTypeRef==spawnReqsType.score){spawnReqsRef=new spawnScore();spawnReqsRef.timeEnabled=false;spawnReqsRef.bothNeeded=false;}
    if(spawnReqsTypeRef==spawnReqsType.pwrups){spawnReqsRef=new spawnPwrups();}
    if(spawnReqsTypeRef==spawnReqsType.kills){spawnReqsRef=new spawnKills();}
    if(spawnReqsTypeRef==spawnReqsType.dmg){spawnReqsRef=new spawnDmg();}
    if(spawnReqsTypeRef==spawnReqsType.waveCounts){spawnReqsRef=new spawnWaveCounts();}
    if(spawnReqsTypeRef==spawnReqsType.powerupCounts){spawnReqsRef=new spawnPowerupCounts();}
    }
}

public enum spawnReqsType{time,score,energy,missed,pwrups,kills,dmg,waveCounts,powerupCounts}
//public enum spawnerType{waves,powerups,disrupters}
//public interface ISpawnerConfig{}
[System.Serializable]public class spawnReqs{
    //[HideInInspector]public spawnReqsMono srm=new spawnReqsMono();
    public bool timeEnabled=true;
    public Vector2 time=new Vector2(10f,20f);
    public float timer=-4;
    public int repeat=1;
    public float repeatInterval=0.75f;
    public bool bothNeeded=true;
    public bool startTimeAfterSecond=false;
}
[System.Serializable]public class spawnScore:spawnReqs{
    public Vector2Int scoreMaxSetRange=new Vector2Int(15,30);
    public int scoreMax=-4;
    public int score;
    public int specialId=0;//-1 for Waves & -2 for Shop
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
[System.Serializable]public class spawnWaveCounts:spawnReqs{
    public WaveConfig wave;
    public float countsNeeded=3;
    public float counts;
}
[System.Serializable]public class spawnPowerupCounts:spawnReqs{
    public PowerupItem powerupItem;
    public float countsNeeded=3;
    public float counts;
}
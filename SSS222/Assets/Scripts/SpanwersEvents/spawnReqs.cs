using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class spawnReqsMono:MonoBehaviour{
    public List<spawnReqs> spawnReqsList=new List<spawnReqs>();
    public static void AddScore(int val,int id=0){
        if(id==-1||id==-2){
            List<spawnReqs> list=spawnReqsMono.instance.spawnReqsList.FindAll(x=>x is spawnScore);
            List<spawnScore> list2=new List<spawnScore>();foreach(spawnReqs ssl in list){list2.Add((spawnScore)ssl);}
            spawnScore ss=list2.Find(x=>x.specialId==id);if(ss.score!=-5){if(ss.scoreNeeded!=-5&&val!=-5){ss.score+=val;}else if(val==-5){ss.score=ss.scoreNeeded;}}
        }else if(id>=0){
            foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnScore){var ss=(spawnScore)sr;if(ss.score!=-5){if(ss.scoreNeeded!=-5&&val!=-5){ss.score+=val;}else if(val==-5){ss.score=ss.scoreNeeded;}}}}
        }
    }
    public static void AddEnergy(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnEnergy&&!sr.GetType().IsSubclassOf(typeof(spawnEnergy))){var ss=(spawnEnergy)sr;if(ss.energy!=-5)ss.energy+=val;}}}
    public static void AddMissed(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnMissed){var ss=(spawnMissed)sr;if(ss.energy!=-5)ss.energy+=val;}}}
    public static void AddDmg(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnDmg){var ss=(spawnDmg)sr;if(ss.dmg!=-5)ss.dmg+=val;}}}
    public static void AddKills(int val=1){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnKills){var ss=(spawnKills)sr;if(ss.kills!=-5)ss.kills+=val;}}}
    public static void AddWaves(int val=1){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnWavesTotal){var ss=(spawnWavesTotal)sr;if(ss.waves!=-5)ss.waves+=val;}}}
    public static void AddWaveCounts(WaveConfig waveConfig){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnWaveCounts){var ss=(spawnWaveCounts)sr;if(waveConfig==ss.wave)if(ss.counts!=-5)ss.counts+=1;}}}
    public static void AddPwrups(string name,int val=1){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnPowerupsTotal){var ss=(spawnPowerupsTotal)sr;if(ss.pwrups!=-5)ss.pwrups+=val;}}
        if(!String.IsNullOrEmpty(name))foreach(spawnReqs sr2 in spawnReqsMono.instance.spawnReqsList){if(sr2 is spawnPowerupCounts){var ss2=(spawnPowerupCounts)sr2;if(name.Contains(ss2.powerupItem.item.name)&&ss2.counts!=-5)ss2.counts+=val;}}
    }
    
    public static void AddPowerupCounts(PowerupItem powerupItem){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnPowerupCounts){var ss=(spawnPowerupCounts)sr;if(powerupItem==ss.powerupItem)if(ss.counts!=-5)ss.counts+=1;}}}
    public static void AddStayingTime(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnStayingTime&&!sr.GetType().IsSubclassOf(typeof(spawnStayingTime))){var ss=(spawnStayingTime)sr;if(ss.timer2!=-5)ss.timer2+=val;}}}
    public static void AddMovingTime(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnMovingTime){var ss=(spawnMovingTime)sr;if(ss.timer2!=-5)ss.timer2+=val;}}}

    public static void RandomizeScoreMax(int id=0){
        if(id==-1||id==-2){
            List<spawnReqs> list=spawnReqsMono.instance.spawnReqsList.FindAll(x=>x is spawnScore);
            List<spawnScore> list2=new List<spawnScore>();foreach(spawnReqs ssl in list){list2.Add((spawnScore)ssl);}
            spawnScore ss=list2.Find(x=>x.specialId==id);if(ss!=null)ss.scoreNeeded=UnityEngine.Random.Range(ss.scoreMaxSetRange.x,ss.scoreMaxSetRange.y);else{Debug.LogError("No spawnReqs with specialId = "+id);}
        }else if(id>=0){
            foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnScore){var ss=(spawnScore)sr;ss.scoreNeeded=UnityEngine.Random.Range(ss.scoreMaxSetRange.x,ss.scoreMaxSetRange.y);}}
        }
    }
    public static spawnReqsMono instance;
    void Awake(){if(spawnReqsMono.instance!=null){Destroy(this);}else{instance=this;}}
    public void CheckSpawns(spawnReqs x, spawnReqsType xt, MonoBehaviour mb, string cor, object corInfo=null){  if(!GameSession.GlobalTimeIsPaused){
        if(!spawnReqsMono.instance.spawnReqsList.Contains(x)){spawnReqsMono.instance.spawnReqsList.Add(x);}
        if(x.timeEnabled){
            if(x.timer==-4&&!x.startTimeAfterSecond){RestartTimer(x);}
            if(x.timer>0&&!GameSession.GlobalTimeIsPaused){x.timer-=Time.deltaTime;}
            if(x.timer<=0&&x.timer!=-4&&!x.bothNeeded){
                if(xt==spawnReqsType.time)ConditionCheck<spawnReqs>(x,"","");
            }
        }if(x.GetType().IsSubclassOf(typeof(spawnReqs))){
            switch(xt){
                case spawnReqsType.score:ConditionCheck<spawnScore>(x,"score","scoreNeeded");break;
                case spawnReqsType.energy:ConditionCheck<spawnEnergy>(x,"energy","energyNeeded");break;
                case spawnReqsType.missed:ConditionCheck<spawnMissed>(x,"energy","energyNeeded");break;
                case spawnReqsType.dmg:ConditionCheck<spawnDmg>(x,"dmg","dmgNeeded");break;
                case spawnReqsType.kills:ConditionCheck<spawnKills>(x,"kills","killsNeeded");break;
                case spawnReqsType.powerupsTotal:ConditionCheck<spawnPowerupsTotal>(x,"pwrups","pwrupsNeeded");break;
                case spawnReqsType.wavesTotal:ConditionCheck<spawnWavesTotal>(x,"waves","wavesNeeded");break;
                case spawnReqsType.waveCounts:ConditionCheck<spawnWaveCounts>(x,"counts","countsNeeded");break;
                case spawnReqsType.powerupCounts:ConditionCheck<spawnPowerupCounts>(x,"counts","countsNeeded");break;
                case spawnReqsType.stayingTime:ConditionCheck<spawnStayingTime>(x,"timer2","timeNeeded");break;
                case spawnReqsType.movingTime:ConditionCheck<spawnMovingTime>(x,"timer2","timeNeeded");break;
            }
        }if(!x.timeEnabled){x.bothNeeded=false;x.startTimeAfterSecond=false;}
        
        void RestartTimer(spawnReqs x){spawnReqsMono.instance.RestartTimer(x);}
        void SetTimer(spawnReqs x,float time){spawnReqsMono.instance.SetTimer(x,time);}
        void ConditionCheck<T>(spawnReqs x, string val, string valMax)where T:spawnReqs{ T xs=(T)x;
            string valCur="1";if(!String.IsNullOrEmpty(val))valCur=xs.GetType().GetField(val).GetValue(xs).ToString();
            string valMaxx="0";if(!String.IsNullOrEmpty(valMax))valMaxx=xs.GetType().GetField(valMax).GetValue(xs).ToString();
            if(float.Parse(valCur)!=-4&&(float.Parse(valCur)>=float.Parse(valMaxx))){
                if(xs.startTimeAfterSecond&&xs.timer==-4){RestartTimer(xs);return;}
                if(((xs.bothNeeded&&(xs.timeEnabled&&xs.timer<=0&&xs.timer>-4))||(!xs.bothNeeded))||xs.timer==-5){
                    if(xs.repeat>1&&xs.timer!=-5){
                        StartCoroutine(RepSpawns());
                        SetTimer(xs,-5);
                        if(xs.GetType().IsSubclassOf(typeof(spawnReqs)))if(!String.IsNullOrEmpty(val)){xs.GetType().GetField(val).SetValue(xs,-5);}
                    }else{
                        mb.StartCoroutine(cor,corInfo);
                        if(xs.timeEnabled){RestartTimer(xs);}
                        if(xs.GetType().IsSubclassOf(typeof(spawnReqs)))if(!String.IsNullOrEmpty(val))xs.GetType().GetField(val).SetValue(xs,0);
                    }

                    if(xs.repI>xs.repeat){
                        if(xs.timeEnabled){RestartTimer(xs);}else xs.timer=-4;
                        if(!String.IsNullOrEmpty(val))xs.GetType().GetField(val).SetValue(xs,0);
                        xs.repI=1;
                    }
                }
            }
            IEnumerator RepSpawns(){for(xs.repI=1;xs.repI<=xs.repeat;xs.repI++){yield return new WaitForSeconds(xs.repeatInterval);mb.StartCoroutine(cor,corInfo);}yield break;}
        }
    }}
    
    public void RestartTimer(spawnReqs x){if(x.timeEnabled){SetTimer(x,UnityEngine.Random.Range(x.time.x,x.time.y));}}
    public void SetTimer(spawnReqs x, float time){x.timer=time;}

    public static void Validate(ref spawnReqs x, ref spawnReqsType xt){
        if(xt==spawnReqsType.time){x=new spawnReqs();x.bothNeeded=false;}
        if(xt==spawnReqsType.score){x=new spawnScore();x.timeEnabled=false;x.bothNeeded=false;}
        if(xt==spawnReqsType.energy){x=new spawnEnergy();}
        if(xt==spawnReqsType.missed){x=new spawnMissed();}
        if(xt==spawnReqsType.dmg){x=new spawnDmg();}
        if(xt==spawnReqsType.kills){x=new spawnKills();}
        if(xt==spawnReqsType.wavesTotal){x=new spawnWavesTotal();}
        if(xt==spawnReqsType.waveCounts){x=new spawnWaveCounts();}
        if(xt==spawnReqsType.powerupsTotal){x=new spawnPowerupsTotal();}
        if(xt==spawnReqsType.powerupCounts){x=new spawnPowerupCounts();}
        if(xt==spawnReqsType.stayingTime){x=new spawnStayingTime();x.timeEnabled=false;}
        if(xt==spawnReqsType.movingTime){x=new spawnMovingTime();x.timeEnabled=false;}
        if(!x.timeEnabled){x.bothNeeded=false;x.startTimeAfterSecond=false;}
    }

    public static void RestartAllValues(){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){
        if(sr is spawnReqs&&!sr.GetType().IsSubclassOf(typeof(spawnReqs))){var ss=(spawnReqs)sr;if(ss.timer!=-5)ss.timer=-4;}
        if(sr is spawnScore){var ss=(spawnScore)sr;if(ss.score!=-5)ss.score=0;}
        if(sr is spawnEnergy&&!sr.GetType().IsSubclassOf(typeof(spawnEnergy))){var ss=(spawnEnergy)sr;if(ss.energy!=-5)ss.energy=0;}
        if(sr is spawnMissed){var ss=(spawnMissed)sr;if(ss.energy!=-5)ss.energy=0;}
        if(sr is spawnDmg){var ss=(spawnDmg)sr;if(ss.dmg!=-5)ss.dmg=0;}
        if(sr is spawnKills){var ss=(spawnKills)sr;if(ss.kills!=-5)ss.kills=0;}
        if(sr is spawnWavesTotal){var ss=(spawnWavesTotal)sr;if(ss.waves!=-5)ss.waves=0;}
        if(sr is spawnWaveCounts){var ss=(spawnWaveCounts)sr;if(ss.counts!=-5)ss.counts=0;}
        if(sr is spawnPowerupsTotal){var ss=(spawnPowerupsTotal)sr;if(ss.pwrups!=-5)ss.pwrups=0;}
        if(sr is spawnPowerupCounts){var ss=(spawnPowerupCounts)sr;if(ss.counts!=-5)ss.counts=0;}
        if(sr is spawnStayingTime&&!sr.GetType().IsSubclassOf(typeof(spawnStayingTime))){var ss=(spawnStayingTime)sr;if(ss.timer2!=-5)ss.timer2=0;}
        if(sr is spawnMovingTime){var ss=(spawnMovingTime)sr;if(ss.timer2!=-5)ss.timer2=0;}
    }}
    public static void ResetSpawnReqsList(){spawnReqsMono.instance.spawnReqsList=new List<spawnReqs>();}
}

public enum spawnReqsType{time,score,energy,missed,dmg,kills,wavesTotal,waveCounts,powerupsTotal,powerupCounts,stayingTime,movingTime}
//public enum spawnerType{waves,powerups,disrupters}
//public interface ISpawnerConfig{}
[PropertyOrder(-1)]
[System.Serializable]public class spawnReqs{
    //[HideInInspector]public spawnReqsMono srm=new spawnReqsMono();
    public bool timeEnabled=true;
    [HideIf("@this.timeEnabled == false")]public Vector2 time=new Vector2(10f,20f);
    [EnableIf("timeEnabled")][DisableInEditorMode]public float timer=-4;
    public int repeat=1;
    [HideIf("@this.repeat <= 1")]public float repeatInterval=0.75f;
    [HideIf("@this.repeat <= 1")][DisableInEditorMode]public int repI=1;
    [EnableIf("@this.GetType().IsSubclassOf(typeof(spawnReqs)) && timeEnabled")]public bool bothNeeded=true;
    [EnableIf("@this.GetType().IsSubclassOf(typeof(spawnReqs)) && timeEnabled")]public bool startTimeAfterSecond=false;
}
public class _spawnReqsCounts:spawnReqs{
    public int countsNeeded=3;
    [DisableInEditorMode]public int counts;
}

[System.Serializable]public class spawnScore:spawnReqs{
    public Vector2Int scoreMaxSetRange=new Vector2Int(15,30);
    public int scoreNeeded=1;
    [DisableInEditorMode]public int score=0;
    [InfoBox("-1 for Waves & -2 for Shop")]
    [DisableInPlayModeAttribute]public int specialId=0;
}
[System.Serializable]public class spawnEnergy:spawnReqs{
    public float energyNeeded=100;
    [DisableInEditorMode]public float energy;
}
[System.Serializable]public class spawnMissed:spawnEnergy{}
[System.Serializable]public class spawnDmg:spawnReqs{
    public float dmgNeeded=200;
    [DisableInEditorMode]public float dmg;
}
[System.Serializable]public class spawnKills:spawnReqs{
    public int killsNeeded=50;
    [DisableInEditorMode]public int kills;
}
[System.Serializable]public class spawnWavesTotal:spawnReqs{
    public int wavesNeeded=2;
    [DisableInEditorMode]public int waves;
}
[System.Serializable]public class spawnWaveCounts:_spawnReqsCounts{
    public WaveConfig wave;
}
[System.Serializable]public class spawnPowerupsTotal:spawnReqs{
    public int pwrupsNeeded=2;
    [DisableInEditorMode]public int pwrups;
}
[System.Serializable]public class spawnPowerupCounts:_spawnReqsCounts{
    public PowerupItem powerupItem;
}
[System.Serializable]public class spawnStayingTime:spawnReqs{
    public float timeNeeded=20f;
    [DisableInEditorMode]public float timer2;
}
[System.Serializable]public class spawnMovingTime:spawnStayingTime{}
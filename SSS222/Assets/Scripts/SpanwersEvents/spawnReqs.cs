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
            spawnScore ss=list2.Find(x=>x.specialId==id);if(ss.score!=-5){if(ss.scoreMax!=-5&&val!=-5){ss.score+=val;}else if(val==-5){ss.score=ss.scoreMax;}}
        }else if(id>=0){
            foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnScore){var ss=(spawnScore)sr;if(ss.score!=-5){if(ss.scoreMax!=-5&&val!=-5){ss.score+=val;}else if(val==-5){ss.score=ss.scoreMax;}}}}
        }
    }
    public static void AddEnergy(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnEnergy){var ss=(spawnEnergy)sr;if(ss.energy!=-5)ss.energy+=val;}}}
    public static void AddMissed(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnMissed){var ss=(spawnMissed)sr;if(ss.energy!=-5)ss.energy+=val;}}}
    public static void AddDmg(float val){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnDmg){var ss=(spawnDmg)sr;if(ss.dmg!=-5)ss.dmg+=val;}}}
    public static void AddPwrups(int val=1){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnPwrups){var ss=(spawnPwrups)sr;if(ss.pwrups!=-5)ss.pwrups+=val;}}}
    public static void AddKills(int val=1){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnKills){var ss=(spawnKills)sr;if(ss.kills!=-5)ss.kills+=val;}}}
    public static void AddWaveCounts(WaveConfig waveConfig){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnWaveCounts){var ss=(spawnWaveCounts)sr;if(waveConfig==ss.wave)if(ss.counts!=-5)ss.counts+=1;}}}
    public static void AddPowerupCounts(PowerupItem powerupItem){foreach(spawnReqs sr in spawnReqsMono.instance.spawnReqsList){if(sr is spawnPowerupCounts){var ss=(spawnPowerupCounts)sr;if(powerupItem==ss.powerupItem)if(ss.counts!=-5)ss.counts+=1;}}}

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
    public void CheckSpawns(spawnReqs x, spawnReqsType xt, MonoBehaviour mb, string cor, object corInfo=null){
        if(!spawnReqsMono.instance.spawnReqsList.Contains(x)){spawnReqsMono.instance.spawnReqsList.Add(x);}
        if(x.timeEnabled){
            if(x.timer==-4&&!x.startTimeAfterSecond){RestartTimer(x);}
            if(x.timer>0&&!GameSession.GlobalTimeIsPaused){x.timer-=Time.deltaTime;}
            if(x.timer<=0&&x.timer!=-4&&!x.bothNeeded){
                if(xt==spawnReqsType.time)ConditionCheck<spawnReqs>(x,"","");
                //if(x.repeat>1){StartCoroutine(RepSpawns());}
                //else{mb.StartCoroutine(cor);RestartTimer(x);}
            }
        }else if((!x.bothNeeded)||(x.bothNeeded&&x.timeEnabled)){
            switch(xt){
                //case spawnReqsType.score:ConditionCheck<spawnScore>(x,"score","scoreMax");break;
                case spawnReqsType.energy:ConditionCheck<spawnEnergy>(x,"energy","energyNeeded");break;
                case spawnReqsType.missed:ConditionCheck<spawnMissed>(x,"energy","energyNeeded");break;
                case spawnReqsType.pwrups:ConditionCheck<spawnPwrups>(x,"pwrups","pwrupsNeeded");break;
                case spawnReqsType.kills:ConditionCheck<spawnKills>(x,"kills","killsNeeded");break;
                case spawnReqsType.dmg:ConditionCheck<spawnDmg>(x,"dmg","dmgNeeded");break;
                case spawnReqsType.waveCounts:ConditionCheck<spawnWaveCounts>(x,"counts","countsNeeded");break;
                case spawnReqsType.powerupCounts:ConditionCheck<spawnPowerupCounts>(x,"counts","countsNeeded");break;
            }
        }
        
        void RestartTimer(spawnReqs x){spawnReqsMono.instance.RestartTimer(x);}
        void SetTimer(spawnReqs x,float time){spawnReqsMono.instance.SetTimer(x,time);}
        void ConditionCheck<T>(spawnReqs x, string val, string valMax)where T:spawnReqs{ T xs=(T)x;
            string valCur="1";if(!String.IsNullOrEmpty(val))val=xs.GetType().GetField(val).GetValue(xs).ToString();
            string valMaxx="0";if(!String.IsNullOrEmpty(valMax))valMaxx=xs.GetType().GetField(valMax).GetValue(xs).ToString();
            if(float.Parse(valCur)>float.Parse(valMaxx)){
                if(xs.startTimeAfterSecond&&xs.timer==-4){RestartTimer(xs);return;}
                if(((xs.bothNeeded&&(xs.timeEnabled&&xs.timer<=0&&xs.timer>-4))||(!xs.bothNeeded))||xs.timer==-5){
                    //mb.StartCoroutine(cor);
                    if(xs.repeat>1&&xs.timer!=-5){
                        if(!String.IsNullOrEmpty(val)){xs.GetType().GetField(val).SetValue(xs,-5);}
                        StartCoroutine(RepSpawns());
                        Debug.Log(cor);
                        //mb.InvokeRepeating(cor, 0, xs.repeatInterval);
                        //InvokeRepeating("RepSpawns", 0, xs.repeatInterval);
                        SetTimer(xs,-5);
                    }
                    if(xs.repI>xs.repeat){
                        //mb.CancelInvoke(cor.ToString());
                        if(xs.timeEnabled){RestartTimer(xs);}else xs.timer=-4;
                        if(!String.IsNullOrEmpty(val))xs.GetType().GetField(val).SetValue(xs,0);
                        xs.repI=1;
                    }
                }
            }
            IEnumerator RepSpawns(){
                //if(xs.repI==1){mb.StartCoroutine(cor);xs.repI=2;}
                for(xs.repI=1;xs.repI<=xs.repeat;xs.repI++){Debug.Log("Before: "+cor);yield return new WaitForSeconds(xs.repeatInterval);mb.StartCoroutine(cor,corInfo);Debug.Log("After: "+cor);}//yield return StartCoroutine(RepSpawns());}
                //for(;xs.repI<xs.repeat;){xs.repI++;}
                /*if(xs.repI>=xs.repeat){
                    mb.CancelInvoke(cor.ToString());
                    if(xs.timeEnabled){RestartTimer(xs);}else xs.timer=-4;
                    if(!String.IsNullOrEmpty(val))xs.GetType().GetField(val).SetValue(xs,0);
                    xs.repI=1;
                }*/
                yield break;
            }
            //if(xs.repI>xs.repeat)xs.repI=1;
        }
    }
    
    public void RestartTimer(spawnReqs x){if(x.timeEnabled){SetTimer(x,UnityEngine.Random.Range(x.time.x,x.time.y));}}
    public void SetTimer(spawnReqs x, float time){x.timer=time;}

    public static void Validate(ref spawnReqs spawnReqsRef, ref spawnReqsType spawnReqsTypeRef){
        if(spawnReqsTypeRef==spawnReqsType.time){spawnReqsRef=new spawnReqs();spawnReqsRef.bothNeeded=false;}
        if(spawnReqsTypeRef==spawnReqsType.energy){spawnReqsRef=new spawnEnergy();}
        if(spawnReqsTypeRef==spawnReqsType.missed){spawnReqsRef=new spawnMissed();}
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
[PropertyOrder(-1)]
[System.Serializable]public class spawnReqs{
    //[HideInInspector]public spawnReqsMono srm=new spawnReqsMono();
    public bool timeEnabled=true;
    [EnableIf("timeEnabled")]public Vector2 time=new Vector2(10f,20f);
    [EnableIf("timeEnabled")][DisableInEditorMode]public float timer=-4;
    public int repeat=1;
    [HideIf("@this.repeat <= 1")]public float repeatInterval=0.75f;
    [HideIf("@this.repeat <= 1")][DisableInEditorMode]public int repI=1;
    [EnableIf("@this.GetType().IsSubclassOf(typeof(spawnReqs))")]public bool bothNeeded=true;
    [EnableIf("@this.GetType().IsSubclassOf(typeof(spawnReqs)) && timeEnabled")]public bool startTimeAfterSecond=false;
}
[System.Serializable]public class spawnScore:spawnReqs{
    public Vector2Int scoreMaxSetRange=new Vector2Int(15,30);
    public int scoreMax=-4;
    [DisableInEditorMode]public int score;
    [InfoBox("-1 for Waves & -2 for Shop")]
    [DisableInPlayModeAttribute]public int specialId=0;
}
[System.Serializable]public class spawnEnergy:spawnReqs{
    public float energyNeeded=100;
    [DisableInEditorMode]public float energy;
}
[System.Serializable]public class spawnMissed:spawnEnergy{}
[System.Serializable]public class spawnPwrups:spawnReqs{
    public int pwrupsNeeded=2;
    [DisableInEditorMode]public int pwrups;
}
[System.Serializable]public class spawnKills:spawnReqs{
    public float killsNeeded=50;
    [DisableInEditorMode]public float kills;
}
[System.Serializable]public class spawnDmg:spawnReqs{
    public float dmgNeeded=200;
    [DisableInEditorMode]public float dmg;
}

public class _spawnReqsCounts:spawnReqs{
    public float countsNeeded=3;
    [DisableInEditorMode]public float counts;
}
[System.Serializable]public class spawnWaveCounts:_spawnReqsCounts{
    public WaveConfig wave;
}
[System.Serializable]public class spawnPowerupCounts:_spawnReqsCounts{
    public PowerupItem powerupItem;
}
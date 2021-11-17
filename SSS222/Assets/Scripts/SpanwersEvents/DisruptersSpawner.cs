using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptersSpawner : MonoBehaviour{
    public List<DisrupterConfig> disruptersList=new List<DisrupterConfig>();
    [SerializeField]int rep=1;
    [SerializeField]DisrupterConfig currentCfg;
    [SerializeField]DisrupterConfig repCfg;
    private void Awake() {
        StartCoroutine(SetValues());
    }
    public IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.05f);
        List<DisrupterConfig> dcs=new List<DisrupterConfig>();
        foreach(DisrupterConfig dc in disruptersList){
            var da=Instantiate(dc);
            dcs.Add(da);
        }
        this.disruptersList=dcs;
        yield return new WaitForSecondsRealtime(0.01f);
        foreach(DisrupterConfig dc in disruptersList){var d=dc.spawnProps;if(d.timeEnabled){RestartTime(d);}
        if(d.startTimeAfterSecond){d.time=-4;}}
    }
    IEnumerator Start(){do{yield return StartCoroutine(CheckSpawns());}while(true);}

    public IEnumerator CheckSpawns(){
    foreach(DisrupterConfig dc in disruptersList){
    var d=dc.spawnProps;
    var dt=dc.disrupterType;
        if(d.timeEnabled&&d.time<=0&&d.time!=-4&&!d.bothNeeded){yield return StartCoroutine(SpawnWave(dc));}
        else if((d.secondEnabled&&!d.bothNeeded)||(d.secondEnabled&&d.bothNeeded&&d.timeEnabled)){
            if(dt==disrupterType.energy||dt==disrupterType.missed){
                var ds=(DisrupterConfig.spawnEnergy)d;
                if(ds.energy>=ds.energyNeeded){if(d.startTimeAfterSecond&&d.time==-4){RestartTime(d);}if(d.time<=0&&d.time!=-4){ds.energy=0;yield return StartCoroutine(SpawnWave(dc));}}
            }
            else if(dt==disrupterType.pwrups){
                var ds=(DisrupterConfig.spawnPwrups)d;
                if(ds.pwrups>=ds.pwrupsNeeded){if(d.startTimeAfterSecond&&d.time==-4){RestartTime(d);}if(d.time<=0&&d.time!=-4){ds.pwrups=0;yield return StartCoroutine(SpawnWave(dc));}}
            }
            else if(dt==disrupterType.kills){
                var ds=(DisrupterConfig.spawnKills)d;
                if(ds.kills>=ds.killsNeeded){if(d.startTimeAfterSecond&&d.time==-4){RestartTime(d);}if(d.time<=0&&d.time!=-4){ds.kills=0;yield return StartCoroutine(SpawnWave(dc));}}
            }
            else if(dt==disrupterType.dmg){
                var ds=(DisrupterConfig.spawnDmg)d;
                if(ds.dmg>=ds.dmgNeeded){if(d.startTimeAfterSecond&&d.time==-4){RestartTime(d);}if(d.time<=0&&d.time!=-4){ds.dmg=0;yield return StartCoroutine(SpawnWave(dc));}}
            }
            else if(dt==disrupterType.counts){
                var ds=(DisrupterConfig.spawnCounts)d;
                if(ds.counts>=ds.countsNeeded){if(d.startTimeAfterSecond&&d.time==-4){RestartTime(d);}if(d.time<=0&&d.time!=-4){ds.counts=0;yield return StartCoroutine(SpawnWave(dc));}}
            }
        }
    }
    int repI=1;
    if(repCfg!=null){repI=repCfg.spawnProps.repeat;
    if(rep<=repI){if(rep>1)StartCoroutine(SpawnWave(repCfg));yield return new WaitForSeconds(repCfg.spawnProps.repeatInterval);}
    //if(rep>=repI){rep=0;}
    }
    }
    void RestartTime(DisrupterConfig.disrupterSpawnProps d){d.time=Random.Range(d.timeS,d.timeE);}
    void SetTime(DisrupterConfig.disrupterSpawnProps d, float tim){d.time=tim;}
    IEnumerator AddRep(float time){yield return new WaitForSeconds(time);rep++;}
    IEnumerator SpawnWave(DisrupterConfig dc){
        currentCfg=dc;
        var d=dc.spawnProps;
        if(d.repeat>1){repCfg=dc;
        if(rep>=d.repeat){
            rep=1;repCfg=null;RestartTime(d);}
            else{StartCoroutine(AddRep(d.repeatInterval));}
        }
        else{if(d.timeEnabled&&d.time<=0&&d.time!=-4){RestartTime(d);}}
        if(d.startTimeAfterSecond){d.time=-4;}
        yield return StartCoroutine(FindObjectOfType<Waves>().SpawnAllEnemiesInWave(dc.waveConfig));
    }
    void Update(){
        if(!GameSession.GlobalTimeIsPaused){foreach(DisrupterConfig dc in disruptersList){var d=dc.spawnProps;if(d.timeEnabled){if(d.time>0)d.time-=Time.deltaTime;}}}
    }
    public void AddEnergy(float val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.energy){var ds=(DisrupterConfig.spawnEnergy)dc.spawnProps;ds.energy+=val;}}}
    public void AddMissed(float val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.missed){var ds=(DisrupterConfig.spawnEnergy)dc.spawnProps;ds.energy+=val;}}}
    public void AddPwrups(int val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.pwrups){var ds=(DisrupterConfig.spawnPwrups)dc.spawnProps;ds.pwrups+=val;}}}
    public void AddKills(int val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.kills){var ds=(DisrupterConfig.spawnKills)dc.spawnProps;ds.kills+=val;}}}
    public void AddDmg(float val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.dmg){var ds=(DisrupterConfig.spawnDmg)dc.spawnProps;ds.dmg+=val;}}}
    public void AddCounts(WaveConfig waveConfig){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.counts){var ds=(DisrupterConfig.spawnCounts)dc.spawnProps;if(waveConfig==ds.countsWave)ds.counts+=1;}}}
    public void DestroyAll(){foreach(DisrupterConfig dc in disruptersList){Destroy(dc);}disruptersList.Clear();}
}
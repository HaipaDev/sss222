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
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.05f);
        List<DisrupterConfig> dcs=new List<DisrupterConfig>();
        foreach(DisrupterConfig dc in disruptersList){
            var da=Instantiate(dc);
            dcs.Add(da);
        }
        this.disruptersList=dcs;
        yield return new WaitForSecondsRealtime(0.01f);
        foreach(DisrupterConfig dc in disruptersList){var d=dc.spawnReqs;if(d.timeEnabled){}
        if(d.startTimeAfterSecond){d.timer=-4;}}
    }
    IEnumerator Start(){do{yield return StartCoroutine(CheckSpawns());}while(true);}
    IEnumerator CheckSpawns(){
    foreach(DisrupterConfig dc in disruptersList){
        spawnReqs x=dc.spawnReqs;
        spawnReqsType xt=dc.spawnReqsType;
        spawnReqsMono.instance.CheckSpawns(x,xt,this,SpawnWave(dc));
        for(var i=0;i<disruptersList.Count;i++){if(!disruptersList[i].name.Contains("(Clone)"))disruptersList[i]=Instantiate(dc);}
    }
    int repI=1;
    if(repCfg!=null){repI=repCfg.spawnReqs.repeat;
    if(rep<=repI){if(rep>1)StartCoroutine(SpawnWave(repCfg));yield return new WaitForSeconds(repCfg.spawnReqs.repeatInterval);}
    //if(rep>=repI){rep=0;}
    }
    }
    private void RestartTime(DisrupterConfig dc){FindObjectOfType<spawnReqsMono>().RestartTime(dc.spawnReqs);}
    IEnumerator AddRep(float time){yield return new WaitForSeconds(time);rep++;}
    IEnumerator SpawnWave(DisrupterConfig dc){
        currentCfg=dc;
        var d=dc.spawnReqs;
        if(d.repeat>1){repCfg=dc;
        if(rep>=d.repeat){
            rep=1;repCfg=null;RestartTime(dc);}
            else{StartCoroutine(AddRep(d.repeatInterval));}
        }
        else{if(d.timeEnabled&&d.timer<=0&&d.timer!=-4){RestartTime(dc);}}
        if(d.startTimeAfterSecond){d.timer=-4;}
        yield return StartCoroutine(FindObjectOfType<Waves>().SpawnAllEnemiesInWave(dc.waveConfig));
    }
    public void DestroyAll(){foreach(DisrupterConfig dc in disruptersList){Destroy(dc);}disruptersList.Clear();}
}
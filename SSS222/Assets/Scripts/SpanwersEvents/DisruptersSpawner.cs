using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DisruptersSpawner : MonoBehaviour{
    public List<DisrupterConfig> disruptersList=new List<DisrupterConfig>();
    [SerializeField]int rep=1;
    [SerializeField]DisrupterConfig currentCfg;
    [SerializeField]DisrupterConfig repCfg;
    void Awake() {StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.05f);
        foreach(DisrupterConfig dc in disruptersList){var d=dc.spawnReqs;if(d.timeEnabled){}
        if(d.startTimeAfterSecond){d.timer=-4;}}
    }

    IEnumerator Start(){do{yield return StartCoroutine(CheckSpawns());}while(true);}
    IEnumerator CheckSpawns(){
    if(currentCfg!=null){
        if(!currentCfg.name.Contains("(Clone)")){this.currentCfg=Instantiate(currentCfg);}
        else if(currentCfg.name.Contains("(Clone)")){
            foreach(DisrupterConfig dc in disruptersList){
                spawnReqs x=dc.spawnReqs;
                spawnReqsType xt=dc.spawnReqsType;
                spawnReqsMono.instance.CheckSpawns(x,xt,this,SpawnWave(dc));
                for(var i=0;i<disruptersList.Count;i++){if(!disruptersList[i].name.Contains("(Clone)"))disruptersList[i]=Instantiate(dc);}
            }
        }
    }
    for(int i=0;i<disruptersList.Count;i++){
        if(!disruptersList[i].name.Contains("(Clone)")){this.disruptersList[i]=Instantiate(disruptersList[i]);}
    }
    int repI=1;
    if(repCfg!=null){repI=repCfg.spawnReqs.repeat;
    if(rep<=repI){if(rep>1)StartCoroutine(SpawnWave(repCfg));yield return new WaitForSeconds(repCfg.spawnReqs.repeatInterval);}
    //if(rep>=repI){rep=0;}
    }
    }
    private void RestartTime(DisrupterConfig dc){FindObjectOfType<spawnReqsMono>().RestartTime(dc.spawnReqs);}
    IEnumerator AddRep(float time){yield return new WaitForSeconds(time);rep++;}
    [Button("Spawn CurrentCfg")][ContextMenu("Spawn CurrentCfg")]public void SpawnWaveCallCurrent(){
        if(currentCfg==null){currentCfg=disruptersList[(int)Random.Range(0,disruptersList.Count)];}
        StartCoroutine(SpawnWave(currentCfg));}
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
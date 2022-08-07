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
    void Update(){if(!GameSession.GlobalTimeIsPaused)if(GameSession.instance._noBreak())CheckSpawnReqs();}
    void CheckSpawnReqs(){
        if(currentCfg!=null){if(!currentCfg.name.Contains("(Clone)")){currentCfg=Instantiate(currentCfg);}}
        for(int i=0;i<disruptersList.Count;i++){
            if(disruptersList[i]!=null){
                if(!disruptersList[i].name.Contains("(Clone)")){disruptersList[i]=Instantiate(disruptersList[i]);}
                else{
                    spawnReqs x=disruptersList[i].spawnReqs;
                    spawnReqsType xt=disruptersList[i].spawnReqsType;
                    spawnReqsMono.instance.CheckSpawns(x,xt,this,"SpawnWave",disruptersList[i]);
                }
            }
        }
    }
    private void RestartTimer(DisrupterConfig dc){FindObjectOfType<spawnReqsMono>().RestartTimer(dc.spawnReqs);}
    IEnumerator AddRep(float time){yield return new WaitForSeconds(time);rep++;}
    [Button("Spawn CurrentCfg")][ContextMenu("Spawn CurrentCfg")]public void SpawnWaveCallCurrent(){
        if(currentCfg==null){currentCfg=disruptersList[(int)Random.Range(0,disruptersList.Count)];}
        SpawnWave(currentCfg);}
    void SpawnWave(DisrupterConfig dc){
        currentCfg=dc;
        FindObjectOfType<Waves>().SpawnAllEnemiesInWave(dc.waveConfig);
    }
    //void OnDestroy(){DestroyAll();}
    public void DestroyAll(){foreach(DisrupterConfig dc in disruptersList){Destroy(dc);}disruptersList.Clear();}
}
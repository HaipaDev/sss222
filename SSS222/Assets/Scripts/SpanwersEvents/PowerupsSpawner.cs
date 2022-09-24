using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PowerupsSpawner : MonoBehaviour{
    [SerializeField] public spawnReqsType spawnReqsType;
    [SerializeReference] public spawnReqs spawnReqs;
    [SerializeField] public Vector2 powerupSpawnPosRange=new Vector2(-3f,3f);
    LootTablePowerups lootTable;
    IEnumerator Start(){
        yield return new WaitForSeconds(0.05f);
        lootTable=GetComponent<LootTablePowerups>();
    }
    void CheckSpawnReqs(){
        //if(spawnReqs!=GameRules.instance.waveSpawnReqs)spawnReqs=GameRules.instance.spawnReqs;
        //if(spawnReqsType!=GameRules.instance.waveSpawnReqsType)spawnReqsType=GameRules.instance.waveSpawnReqsType;
        spawnReqs x=spawnReqs;
        spawnReqsType xt=spawnReqsType;
        if(x!=null)spawnReqsMono.instance.CheckSpawns(x,xt,this,"SpawnPowerup");
    }
    void Update(){
        if(!GameManager.GlobalTimeIsPaused)if(GameManager.instance._noBreak())CheckSpawnReqs();
    }
    [Button("Spawn Powerup")][ContextMenu("Spawn Powerup")]public void SpawnPowerupCall(){StartCoroutine(SpawnPowerup());}
    IEnumerator SpawnPowerup(){
        Vector2 powerupsPos=new Vector2(Random.Range(powerupSpawnPosRange.x, powerupSpawnPosRange.y), 7f);
        GameObject newPowerup=null;
        PowerupItem lootItem=null;
        if(lootTable!=null){
            lootItem=lootTable.GetItem();
            if(lootItem!=null){
                if(!System.String.IsNullOrEmpty(lootItem.assetName)||AssetsManager.instance.Get(lootTable.GetItem().assetName)==null){
                    newPowerup=Instantiate(
                        AssetsManager.instance.Get(lootTable.GetItem().assetName),
                        powerupsPos,
                        Quaternion.identity);
                    //Debug.Log("Powerup spawned: "+newPowerup.name);
                }else Debug.LogWarning("Asset name not set or wrong in "+lootItem+" !");
            }else Debug.LogWarning("Loottable randomized a null item!");
        }else{Debug.LogError("Loottable not assigned!");lootTable=GetComponent<LootTablePowerups>();
            if(lootTable!=null){Debug.LogWarning("Fine I assigned it myself");yield return StartCoroutine(SpawnPowerup());}
        }
        yield break;
    }
}
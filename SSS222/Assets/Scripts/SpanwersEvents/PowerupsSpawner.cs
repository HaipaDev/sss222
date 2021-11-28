using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PowerupsSpawner : MonoBehaviour{
    [SerializeField] public PowerupsSpawnerConfig powerupsSpawner;
    [SerializeField] public Vector2 powerupSpawnPosRange=new Vector2(-3f,3f);
    LootTablePowerups lootTable;
    IEnumerator Start(){
        yield return new WaitForSeconds(0.05f);
        lootTable=GetComponent<LootTablePowerups>();
    }
    void CheckSpawnReqs(){if(powerupsSpawner!=null){
        if(!powerupsSpawner.name.Contains("(Clone)")){gameObject.name=powerupsSpawner.name;this.powerupsSpawner=Instantiate(powerupsSpawner);}
        else if(powerupsSpawner.name.Contains("(Clone)")){
            spawnReqs x=powerupsSpawner.spawnReqs;
            spawnReqsType xt=powerupsSpawner.spawnReqsType;
            spawnReqsMono.instance.CheckSpawns(x,xt,this,"SpawnPowerup");
        }
    }}
    void Update(){
        if(!GameSession.GlobalTimeIsPaused)CheckSpawnReqs();
    }
    [Button("Spawn Powerup")][ContextMenu("Spawn Powerup")]public void SpawnPowerupCall(){StartCoroutine(SpawnPowerup());}
    IEnumerator SpawnPowerup(){
        Vector2 powerupsPos=new Vector2(Random.Range(powerupSpawnPosRange.x, powerupSpawnPosRange.y), 7f);
        GameObject newPowerup=null;
        PowerupItem lootItem=null;
        if(lootTable!=null){
            lootItem=lootTable.GetItem();
            if(lootItem!=null){
                if(lootItem.item!=null){
                    newPowerup=Instantiate(
                        lootTable.GetItem().item,
                        powerupsPos,
                        Quaternion.identity);
                    Debug.Log("Powerup spawned: "+newPowerup.name);
                }else Debug.LogWarning("Powerup prefab not assigned to "+lootItem+" !");
            }else Debug.LogWarning("Loottable randomized a null item!");
        }else{Debug.LogError("Loottable not assigned!");lootTable=GetComponent<LootTablePowerups>();
            if(lootTable!=null){Debug.LogWarning("Fine I assigned it myself");yield return StartCoroutine(SpawnPowerup());}
        }
        yield break;
    }
}
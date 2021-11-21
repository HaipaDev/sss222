using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsSpawner : MonoBehaviour{
    [SerializeField] public PowerupsSpawnerConfig powerupsSpawner;
    LootTablePowerups lootTable;
    private void Awake() {
        StartCoroutine(SetValues());
    }
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.05f);
        this.powerupsSpawner=Instantiate(powerupsSpawner);
        yield return new WaitForSecondsRealtime(0.01f);
        //if(powerupSpawnerType==powerupSpawnerType.time){powerupSpawner.timer=0;}
    }
    //void Start(){do{CheckSpawns();}while(true);}
    void CheckSpawns(){
    if(powerupsSpawner!=null){
        spawnReqs x=powerupsSpawner.spawnReqs;
        spawnReqsType xt=powerupsSpawner.spawnReqsType;
        spawnReqsMono.instance.CheckSpawns(x,xt,this,SpawnPowerups());
    }
    }
    void Update(){
        CheckSpawns();
        //if(!GameSession.GlobalTimeIsPaused){if(timer>0)timer-=Time.deltaTime;}
        //if(powerupsSpawner.spawnReqs.timer<=0){SpawnPowerups();}
    }
    [ContextMenu("Spawn Powerup")]public void SpawnPowerup(){
        var powerupsPos = new Vector3(Random.Range(-3f, 3f), 7f, 0);
        GameObject newPowerup;
        if(lootTable!=null)
        if(lootTable.GetItem()!=null)
        if(lootTable.GetItem().item!=null)
        newPowerup=Instantiate(
            //GetRandomPowerup(),
            lootTable.GetItem().item,
            //powerups[index],
            powerupsPos,
            Quaternion.identity);
        }
    public IEnumerator SpawnPowerups(){
        //var index = Random.Range(0, powerups.Count);
        var powerupsPos = new Vector3(Random.Range(-3f, 3f), 7f, 0);
        GameObject newPowerup;
        if(lootTable!=null)
        if(lootTable.GetItem()!=null)
        if(lootTable.GetItem().item!=null)
        newPowerup=Instantiate(
            //GetRandomPowerup(),
            lootTable.GetItem().item,
            //powerups[index],
            powerupsPos,
            Quaternion.identity);
        yield return null;
        /*if(powerupSpawnerType==powerupSpawnerType.time){
            powerupSpawnerTime ps=(powerupSpawnerTime)powerupSpawner;
            timer=Mathf.RoundToInt(Random.Range(ps.spawnTime.x,ps.spawnTime.y));
        }else if(powerupSpawnerType==powerupSpawnerType.kills){
            powerupSpawnerKills ps=(powerupSpawnerKills)powerupSpawner;
            timer=0.1f;
            enemiesCount=0;
        }*/
    }
}

/*public enum powerupSpawnerType{
    time,
    kills
}

public class powerupSpawnerTypesPoly{}

public class powerupSpawnerTime:powerupSpawnerTypesPoly{
    [SerializeField] public Vector2 spawnTime=new Vector2(9f,16f);
}

public class powerupSpawnerKills:powerupSpawnerTypesPoly{
    [SerializeField] public int enemiesCountReq=20;
}*/
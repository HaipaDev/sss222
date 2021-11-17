using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsSpawner : MonoBehaviour{
    [SerializeField] public powerupSpawnerType powerupSpawnerType;
    [SerializeReference] public powerupSpawnerTypesPoly powerupSpawner=new powerupSpawnerTypesPoly();
    public float timer;
    public int enemiesCount;
    public float sum;
    LootTablePowerups lootTable;

    void Update(){
        if(!GameSession.GlobalTimeIsPaused){if(timer>0)timer-=Time.deltaTime;}
        if(timer<=0){SpawnPowerups();}
    }

    void SpawnPowerups(){
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
        if(powerupSpawnerType==powerupSpawnerType.time){
            powerupSpawnerTime ps=(powerupSpawnerTime)powerupSpawner;
            timer=Mathf.RoundToInt(Random.Range(ps.spawnTime.x,ps.spawnTime.y));
        }else if(powerupSpawnerType==powerupSpawnerType.kills){
            powerupSpawnerKills ps=(powerupSpawnerKills)powerupSpawner;
            timer=0.1f;
            enemiesCount=0;
        }
    }
}

public enum powerupSpawnerType{
    time,
    kills
}
[System.Serializable]
public class powerupSpawnerTypesPoly{}
[System.Serializable]
public class powerupSpawnerTime:powerupSpawnerTypesPoly{
    [SerializeField] public Vector2 spawnTime=new Vector2(9f,16f);
}
[System.Serializable]
public class powerupSpawnerKills:powerupSpawnerTypesPoly{
    [SerializeField] public int enemiesCountReq=20;
}
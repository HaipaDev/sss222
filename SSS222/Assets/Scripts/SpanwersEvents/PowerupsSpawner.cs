using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum spawnerType{
    powerupStatus,
    powerupWeapon,
    wave
}
public class PowerupsSpawner : MonoBehaviour{
    [SerializeField] public spawnerType spawnerType;
    //[SerializeField] public List<GameObject> powerups;
    //[SerializeField] bool looping = false;
    [SerializeField] public float mTimePowerupSpawns = 10f;
    [SerializeField] public float mTimePowerupSpawnsS = 9f;
    [SerializeField] public float mTimePowerupSpawnsE = 16f;
    [SerializeField] public float firstSpawn = 15f;
    [SerializeField] public int enemiesCountReq = -1;
    public float timer;
    public int enemiesCount;

    public float sum;
    LootTablePowerups lootTable;
    void Start(){
        lootTable=GetComponent<LootTablePowerups>();
        timer=firstSpawn;
    }
    private void Update(){
        if(!GameSession.GlobalTimeIsPaused&&Time.timeScale>0.0001f){if(timer>0){timer-=Time.deltaTime;}}
        if(timer<=0){SpawnPowerups();}
    }

    private void SpawnPowerups(){
        if((enemiesCountReq==-1&&timer<=0)||(enemiesCountReq>-1&&enemiesCount>=enemiesCountReq)){
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
            if(enemiesCountReq==-1){
                if(mTimePowerupSpawns!=-1){timer=mTimePowerupSpawns;}
                else{timer=Mathf.RoundToInt(Random.Range(mTimePowerupSpawnsS,mTimePowerupSpawnsE));}
            }else{
                timer=0.1f;
                enemiesCount=0;
            }
        }
    }
}

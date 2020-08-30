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

    public float sum = 0f;
    LootTablePowerups lootTable;
    private void Awake()
    {
        /*foreach (GameObject powerup in powerups)
        {
            sum += powerup.GetComponent<PowerupWeights>().spawnRate;
        }*/
        lootTable=GetComponent<LootTablePowerups>();
    }
    IEnumerator Start(){
        timer = firstSpawn;
        do
        {
            yield return StartCoroutine(SpawnPowerupFirst());
        }while (true);
    }

    /*public GameObject GetRandomPowerup()
    {
        float randomWeight = 0;
        do
        {
            //No weight on any number?
            if (sum == 0) return null;
            randomWeight = Random.Range(0, sum);
        } while (randomWeight == sum);
        foreach (GameObject powerup in powerups)
        {
            if (randomWeight < powerup.GetComponent<PowerupWeights>().spawnRate) return powerup;
            randomWeight -= powerup.GetComponent<PowerupWeights>().spawnRate;
        }
        return null;
    }*/

    /*public IEnumerator SpawnAllWaves(){
        if(timeSpawns <= 0){
            for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++){
                currentWave = waveConfigs[waveIndex];
                yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            }
            timeSpawns = mTimeSpawns;
        }
    }*/
    private IEnumerator SpawnPowerupFirst()
    {
        //if (timer<=0||(enemiesCount>=enemiesCountReq&&enemiesCountReq!=-1&&timer!=-4)){
        if(timer<=0){
            yield return StartCoroutine(SpawnPowerups());
        }
    }

    private IEnumerator SpawnPowerups(){
        if((enemiesCountReq==-1&&timer<=0)||(enemiesCountReq>-1&&enemiesCount>=enemiesCountReq)){
        //var index = Random.Range(0, powerups.Count);
        var powerupsPos = new Vector3(Random.Range(-3f, 3f), 7f, 0);
        var newPowerup=Instantiate(
            //GetRandomPowerup(),
            lootTable.GetItem().item,
            //powerups[index],
            powerupsPos,
            Quaternion.identity);
            if(enemiesCountReq==-1){
                if(mTimePowerupSpawns!=-1){timer=mTimePowerupSpawns;}
                else {timer=Mathf.RoundToInt(Random.Range(mTimePowerupSpawnsS,mTimePowerupSpawnsE));}
            }else{
                timer=0.1f;
                enemiesCount=0;
            }
            //yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        /*if(enemiesCountReq==-1){
            if(mTimePowerupSpawns!=-1){
                var time=mTimePowerupSpawns;
                yield return //new WaitForSeconds(0.001f);
                timer=time;
            }else{
                var time=Random.Range(mTimePowerupSpawnsS,mTimePowerupSpawnsE);
                yield return //new WaitForSeconds(0.001f);
                timer=time;
            }
        }else{
            yield return new WaitForSeconds(0.001f);
            if(enemiesCount>=enemiesCountReq){
            //timer=0.05f;
            enemiesCount=0;
            }
        }*/
    }
    private void Update(){
        //if(currentWave >= waveConfigs.Count)
        if(Time.timeScale>0.0001f){if(timer>0){timer -= Time.deltaTime;}}
        //if(enemiesCount==0&&enemiesCountReq==-1)timer=-4;
        //Debug.Log(timer);
    }
}

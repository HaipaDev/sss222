using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsSpawner : MonoBehaviour{
    [SerializeField] public List<GameObject> powerups;
    //[SerializeField] bool looping = false;
    [SerializeField] float mTimePowerupSpawns = 10f;
    [SerializeField] float firstSpawn = 15f;
    float timer = 0f;
    // Start is called before the first frame update
    IEnumerator Start(){
        timer = firstSpawn;
        do
        {
                yield return StartCoroutine(SpawnPowerupFirst());
        }
        while (true);
    }

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
        if (timer<=0){
            yield return StartCoroutine(SpawnPowerups());
        }
    }

    private IEnumerator SpawnPowerups(){
        var index = Random.Range(0, powerups.Count);
        var powerupsPos = new Vector3(Random.Range(-2.5f, 4f), 7f, 0);
        var newPowerup=Instantiate(
            powerups[index],
            powerupsPos,
            Quaternion.identity);
        yield return new WaitForSeconds(mTimePowerupSpawns);
    }
    private void Update(){
        //if(currentWave >= waveConfigs.Count)
        if(timer>0)timer -= Time.deltaTime;
        //Debug.Log(timer);
    }
}
